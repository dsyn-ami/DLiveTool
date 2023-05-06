using BrotliSharpLib;
using Newtonsoft.Json.Linq;
using dsyn;
using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using DLiveTool.Data;
using System.Threading;
using System.Threading.Tasks;

namespace DLiveTool
{
    public class BiliWebSocket
    {
        #region 事件定义
        /// <summary>
        /// 观众进入直播间
        /// </summary>
        public event Action<ReceiveInterAct> OnUserEnter;
        /// <summary>
        /// 收到弹幕
        /// </summary>
        public event Action<ReceiveDanmakuMsg> OnReceiveDanmaku;
        /// <summary>
        /// 收到礼物
        /// </summary>
        public event Action<ReceiveSendGift> OnReceiveGift;
        /// <summary>
        /// 高能榜用户数量刷新
        /// </summary>
        public event Action<ReceiveOnlineRankChange> OnOnlineRankChange;
        /// <summary>
        /// 看过的人数量刷新
        /// </summary>
        public event Action<ReceiveWatchedChanged> OnWatchedChanged;
        /// <summary>
        /// 有人点赞了
        /// </summary>
        public event Action<ReceiveLikeClick> OnLikeClick;
        /// <summary>
        /// 点赞数更新
        /// </summary>
        public event Action<ReceiveLikeUpdate> OnLikeUpdate;
        #endregion

        ClientWebSocket _ws;
        CancellationTokenSource _tokenSource = new CancellationTokenSource();

        /// <summary>
        /// 连接到指定直播间，并开始接收消息
        /// Action<int, string> :  int:code,   string:msg
        /// </summary>
        /// <param name="roomId">要连接的直播间</param>
        public async void ConnectAsync(string roomId, Action<int, string> OnConnected = null)
        {
            //获取房间基本信息
            string roomInfo = await BiliRequester.GetRoomInitInfoAsync(roomId);
            //保存房间信息
            JObject jObj = JObject.Parse(roomInfo);

            int code = int.Parse(jObj["code"].ToString());
            string msg = jObj["message"].ToString();
            if(code != 0)
            {
                //连接房间失败
                OnConnected?.Invoke(code, msg);
                return;
            }
            AnchorData.RoomId.Value = jObj["data"]["room_id"]?.ToString(); 
            AnchorData.UserId.Value = jObj["data"]["uid"]?.ToString();
            AnchorData.ShotRoomId.Value = jObj["data"]["short_id"]?.ToString();
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString() + "获取房间基本信息 : " + roomInfo);

            //获取主播基本信息
            //这里马上请求主播信息, 可能会返回请求过于频繁
            string userInfoStr = await BiliRequester.GetUserInfoAsync(AnchorData.UserId.Value);
            
            //请求过于频繁时会返回两个json结构,如果有两个,使用第二个
            JsonSpliter spliter = new JsonSpliter(userInfoStr);
            string jsonStr = spliter[spliter.Count - 1];
            jObj = JObject.Parse(jsonStr);
            //保存用户信息
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
            OnConnected?.Invoke(code, msg);
        }

        public void DisConnect()
        {
            _ws?.Dispose();
            _ws = null;
        }

        public async void HeartBeatAsync()
        {
            while (_ws != null && _ws.State == WebSocketState.Open) //发送心跳包保持连接
            {
                byte[] beatHeartData = Packet.HeatBeat.ToBytes();
                string s = "";
                foreach (byte b in beatHeartData)
                {
                    s += (" " + b.ToString());
                }
                Console.WriteLine(s);

                try
                {
                    await _ws.SendAsync(new ArraySegment<byte>(beatHeartData), WebSocketMessageType.Binary, true, _tokenSource.Token);
                }
                catch
                {
                    _ws?.Dispose();
                    _ws = null;
                    return;
                }
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
            byte[] realData = new byte[2048 * 8];
            //完整消息的实际长度
            int realLength = 0;
            while (_ws != null && _ws.State == WebSocketState.Open)
            {
                WebSocketReceiveResult result = null;
                try
                {
                    result = await _ws.ReceiveAsync(new ArraySegment<byte>(buffer), new CancellationToken());
                }
                catch
                {
                    _ws?.Dispose();
                    _ws = null;
                    return;
                }
                
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

                Console.WriteLine(Thread.CurrentThread.ManagedThreadId.ToString() + "receive data" + realData.Take(realLength).Count());
                //处理接收到的消息
                HandleReceiveData(realData.Take(realLength).ToArray());
            }
        }

        /// <summary>
        /// 处理接收到的消息
        /// </summary>
        /// <param name="data">消息数组</param>
        private void HandleReceiveData(byte[] data)
        {
            //子包数据的第一位索引
            int headIndex = 0;

            while (headIndex < data.Length)
            {
                //前四位表示包的长度
                byte[] packetLengthByte = data.Skip(headIndex).Take(4).ToArray();
                if (BitConverter.IsLittleEndian)
                {
                    packetLengthByte = packetLengthByte.Reverse().ToArray();
                }
                //获取包的长度
                int packetLength = BitConverter.ToInt32(packetLengthByte, 0);

                //打包
                Packet packet = new Packet(data.Skip(headIndex).Take(packetLength).ToArray());

                //未压缩，直接使用数据
                if (packet.Header._protocolVersion == ProtocolVersion.UnCompressed)
                {
                    HandleDecodedJson(Encoding.UTF8.GetString(packet.Body));
                }
                //经过压缩，解压后再生成 Packet(可能有多个)
                else if (packet.Header._protocolVersion == ProtocolVersion.Brotli)
                {
                    byte[] decompressedData = Brotli.DecompressBuffer(packet.Body, 0, packet.Body.Length);

                    Console.WriteLine("decompression Length : " + decompressedData.Length);
                    HandleReceiveData(decompressedData);
                }
                //子包第一位索引移动到下一个子包位置，
                headIndex += packetLength;
                //这时如果 headIndex >= data.Length,说明没有下一个子包了，处理结束
            }
        }
        /// <summary>
        /// 处理解包后得到的json字符串
        /// </summary>
        /// <param name="json"></param>
        private void HandleDecodedJson(string json)
        {
            Console.WriteLine($"收到消息 : " + json);
            Console.WriteLine("");

            ReceiveMsg msg = new ReceiveMsg(json);
            switch (msg.CMD)
            {
                //进入直播间
                case "INTERACT_WORD":
                    ReceiveInterAct receiveInterAct = new ReceiveInterAct(json);
                    OnUserEnter?.Invoke(receiveInterAct);
                    break;
                //收到弹幕
                case "DANMU_MSG":
                    ReceiveDanmakuMsg receiveDanmakuMsg = new ReceiveDanmakuMsg(json);
                    BiliMsgWriter.RecordJson(receiveDanmakuMsg.Message, json);
                    OnReceiveDanmaku?.Invoke(receiveDanmakuMsg);
                    break;
                //收到礼物
                case "SEND_GIFT":
                    ReceiveSendGift receiveSendGift = new ReceiveSendGift(json);
                    OnReceiveGift?.Invoke(receiveSendGift);
                    break;
                //高能榜在线观众刷新
                case "ONLINE_RANK_COUNT":
                    ReceiveOnlineRankChange receiveOnlineRankChange = new ReceiveOnlineRankChange(json);
                    AnchorData.OnlineRankCount.Value = receiveOnlineRankChange.OnRankUserCount;
                    OnOnlineRankChange?.Invoke(receiveOnlineRankChange);
                    break;
                //看过的人数量刷新
                case "WATCHED_CHANGE":
                    ReceiveWatchedChanged receiveWatchedChanged = new ReceiveWatchedChanged(json);
                    AnchorData.WatchedCount.Value = receiveWatchedChanged.WatchedCount;
                    OnWatchedChanged?.Invoke(receiveWatchedChanged);
                    break;
                //有观众点赞了
                case "LIKE_INFO_V3_CLICK":
                    ReceiveLikeClick receiveLikeClick = new ReceiveLikeClick(json);
                    OnLikeClick?.Invoke(receiveLikeClick);
                    break;
                //点赞数量更新
                case "LIKE_INFO_V3_UPDATE":
                    ReceiveLikeUpdate receiveLikeUpdate = new ReceiveLikeUpdate(json);
                    AnchorData.LikeCount.Value = receiveLikeUpdate.LikeCount;
                    OnLikeUpdate?.Invoke(receiveLikeUpdate);
                    break;
                default:
                    
                    break;
            }
            BiliMsgWriter.RecordMsg(msg.CMD, json);
        }
    }
}
