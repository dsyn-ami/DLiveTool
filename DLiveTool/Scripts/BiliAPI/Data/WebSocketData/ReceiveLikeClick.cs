using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLiveTool.Data
{
    /*
     {
    "cmd":"LIKE_INFO_V3_CLICK",
    "data":{
        "show_area":0,
        "msg_type":6,
        "like_icon":"https://i0.hdslb.com/bfs/live/23678e3d90402bea6a65251b3e728044c21b1f0f.png",
        "uid":173380123,
        "like_text":"为主播点赞了",
        "uname":"阿佳妮劣",
        "uname_color":"",
        "identities":[
            1
        ],
        "fans_medal":Object{...},
        "contribution_info":{
            "grade":0
        },
        "dmscore":20
    }
}
     */
    public class ReceiveLikeClick
    {
        public string UserId { get; private set; }
        public string UserName { get; private set; }
        public ReceiveLikeClick(string json)
        {
            JObject jo = JObject.Parse(json);
            UserId = jo["data"]["uid"].ToString();
            UserName = jo["data"]["uname"].ToString();
        }
    }
}
