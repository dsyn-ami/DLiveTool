using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DLiveTool
{
    public class BiliWebSocket
    {
        ClientWebSocket _ws;
        CancellationTokenSource _tokenSource = new CancellationTokenSource();
        Uri _uri = new Uri(BiliAPI.LiveWebSocketUrl);

        /// <summary>
        /// 连接到指定直播间，并开始接收消息
        /// </summary>
        /// <param name="roomId">要连接的直播间</param>
        public async void ConnectAsync(string roomId)
        {
            //获取房间基本信息
            string roomInfo = await BiliRequester.GetRoomInitInfo(roomId);
            //保存房间信息
            JObject jObj = JObject.Parse(roomInfo);
            AnchorData.RoomId.Value = jObj["data"]["room_id"]?.ToString(); 
            AnchorData.UserId.Value = jObj["data"]["uid"]?.ToString();
            AnchorData.ShotRoomId.Value = jObj["data"]["short_id"]?.ToString();
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString() + "获取房间基本信息 : " + AnchorData.RoomId.Value);

            //获取主播基本信息
            string userInfo = await BiliRequester.GetUserInfo(AnchorData.UserId.Value);
            //保存用户信息
            jObj = JObject.Parse(userInfo);
            AnchorData.UserName.Value = jObj["data"]["name"].ToString();
            AnchorData.UserFace.Value = jObj["data"]["face"].ToString();
            AnchorData.TopPhoto.Value = jObj["data"]["top_photo"]?.ToString();
            AnchorData.LiveState.Value = jObj["data"]["live_room"]["liveStatus"].ToString().Equals("1");
            AnchorData.RoomUrl.Value = jObj["data"]["live_room"]["url"]?.ToString();
            AnchorData.RoomTitle.Value = jObj["data"]["live_room"]["title"]?.ToString();
            AnchorData.RoomCover.Value = jObj["data"]["live_room"]["cover"]?.ToString();
            AnchorData.WatchedCount.Value = int.Parse(jObj["data"]["live_room"]["watched_show"]["num"]?.ToString());
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString() + "获取用户基本信息 : " + AnchorData.UserName.Value);

            //直播间人数，点赞数，人气等信息通过接收 ws 的消息获取
            //连接到直播间
            _ws?.Dispose();
            _ws = new ClientWebSocket();
            await _ws.ConnectAsync(new Uri(BiliAPI.LiveWebSocketUrl), _tokenSource.Token);
            //连接后马上发送认证消息
            string authMsg = "{\"uid\":" + AnchorData.UserId.Value + ",\"roomid\":" + AnchorData.RoomId.Value + ",\"protover\":3,\"platform\":\"web\",\"clientver\":\"1.4.0\",\"type\":\"2\"}";
            byte[] sendData = new Packet(Operation.Authority, Encoding.UTF8.GetBytes(authMsg)).ToBytes();
            await _ws.SendAsync(new ArraySegment<byte>(sendData), WebSocketMessageType.Binary, true, _tokenSource.Token);
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString() + "连接并发送验证信息");
            //开始心跳
            HeartBeatAsync();
            //开始接收消息
            ReceiveAsync();
        }

        public async void HeartBeatAsync()
        {
            while (_ws != null && _ws.State == WebSocketState.Open) //发送心跳包保持连接
            {
                byte[] beatHeartData = Packet._heartBeatPacket.ToBytes();
                string s = "";
                foreach (byte b in beatHeartData)
                {
                    s += (" " + b.ToString());
                }
                Console.WriteLine(s);
                await _ws.SendAsync(new ArraySegment<byte>(beatHeartData), WebSocketMessageType.Binary, true, _tokenSource.Token);
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString() + "发送心跳数据包：");
                await Task.Delay(30 * 1000);
            }
            Console.WriteLine("心跳中断");
        }

        public async void ReceiveAsync()
        {
            //接受消息的临时缓存
            byte[] buffer = new byte[2048];
            //存储一条完整的消息
            byte[] realData = new byte[2048 * 4];
            //完整消息的实际长度
            int realLength = 0;
            while (_ws != null && _ws.State == WebSocketState.Open)
            {
                var result = await _ws.ReceiveAsync(new ArraySegment<byte>(buffer), new CancellationToken());
                //收到的数据写入缓存
                Array.Copy(buffer, 0, realData, 0, result.Count);
                realLength = result.Count;

                //如果一条消息没接受完整，继续接收
                while (!result.EndOfMessage)
                {
                    result = await _ws.ReceiveAsync(new ArraySegment<byte>(buffer), _tokenSource.Token);
                    //TODO: realData数据溢出处理
                    Array.Copy(buffer, 0, realData, realLength, result.Count);
                    realLength += result.Count;
                }

                //这里可能 不止一条数据，要拆包再用
                Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString() + "receive data" + realData.Take(realLength).Count());
                
            }
        }
    }
}
