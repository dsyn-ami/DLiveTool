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
        string _cookie = "buvid3=0481CE3C-1E8F-A6B6-91ED-438612DE626F12699infoc; i-wanna-go-back=-1; _uuid=E17CF1C4-422F-45EE-661E-CE8F71FC58CE16480infoc; buvid_fp_plain=undefined; CURRENT_BLACKGAP=0; nostalgia_conf=-1; LIVE_BUVID=AUTO7516597645955506; b_ut=5; blackside_state=1; hit-dyn-v2=1; b_nut=100; fingerprint3=875270516b30e2fdf8ef06263343c578; rpdid=|(J~Rlmlukl)0J'uY~kummJ|R; hit-new-style-dyn=0; buvid4=C7A08736-3EA8-0C17-2D62-2118F44138D020993-022080610-%2ByHNrXw7i71TSnQI3oFAng%3D%3D; Hm_lvt_8a6e55dbd2870f0f5bc9194cddf32a02=1675130689,1675828299,1676273850; CURRENT_FNVAL=4048; header_theme_version=CLOSE; CURRENT_QUALITY=112; bp_article_offset_226638530=765828115446366200; bsource=search_bing; b_lsid=7646F899_18692234B53; home_feed_column=4; innersign=1; fingerprint=596582c8cb6376e4239a32a3e5222c9f; SESSDATA=348aaf77%2C1693043077%2Ca5872%2A21; bili_jct=a830e98d41602a90f8e89669daa563b6; DedeUserID=226638530; DedeUserID__ckMd5=db34e0f8ed4c2993; sid=5ojbrj4f; _dfcaptcha=5d15aa427e12b2b9520e720aa24b4cda; bp_video_offset_226638530=767306395949203500; PEA_AU=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJiaWQiOjIyNjYzODUzMCwicGlkIjo0NTQ4OTYsImV4cCI6MTcwOTAyOTMzNCwiaXNzIjoidGVzdCJ9.YzFdI78C_5PmlSLnxEvBDtyRyQ4KbcqcZva9xKHMsJE; PVID=2; buvid_fp=596582c8cb6376e4239a32a3e5222c9f";
        public async void SendDanmakuAsync(string msg)
        {
            //生成要发送的数据
            Dictionary<string, string> dict = new Dictionary<string, string>();

            dict["msg"] = msg;
            dict["roomid"] = "8804378";
            dict["csrf"] = "a830e98d41602a90f8e89669daa563b6";
            dict["csrf_token"] = "a830e98d41602a90f8e89669daa563b6";
            dict["rnd"] = "56846425472";
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

