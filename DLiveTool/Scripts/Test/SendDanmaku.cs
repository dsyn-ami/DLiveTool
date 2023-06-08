using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Server.TestBiliDanmaku
{
    public class SendBiliDanmaku
    {
        string _url = "https://api.live.bilibili.com/msg/send";
        HttpClient _client = new HttpClient();
        string _cookie = "";
        public async void SendDanmakuAsync(string msg)
        {
            //生成要发送的数据
            Dictionary<string, string> dict = new Dictionary<string, string>();

            dict["msg"] = msg;
            dict["roomid"] = "8804378";
            dict["csrf"] = "";
            dict["csrf_token"] = "";
            dict["rnd"] = "56846425472";//时间戳
            dict["color"] = "16777215";
            dict["fontsize"] = "25";
            FormUrlEncodedContent content = new FormUrlEncodedContent(dict);
            content.Headers.Add("cookie", _cookie);
            //post发送
            HttpResponseMessage responseMsg = await _client.PostAsync(_url, content);
            Console.WriteLine("客户端发起http请求 ： " + content);

            string str = await responseMsg.Content.ReadAsStringAsync();
            Console.WriteLine("收到数据 ： " + str);
            responseMsg.Dispose();
        }
    }
}

