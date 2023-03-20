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
    "cmd":"INTERACT_WORD",
    "data":{
        "contribution":{
            "grade":0
        },
        "core_user_type":0,
        "dmscore":16,
        "fans_medal":Object{...},
        "identities":[
            3,
            1
        ],
        "is_spread":0,
        "msg_type":1,
        "privilege_type":0,
        "roomid":923833,
        "score":1728279551512,
        "spread_desc":"",
        "spread_info":"",
        "tail_icon":0,
        "timestamp":1678266086,
        "trigger_time":1678266085443714300,
        "uid":4184057,
        "uname":"汐然然ヽ",
        "uname_color":""
    }
}
     */

    /// <summary>
    /// 有人进入直播间
    /// </summary>
    public class ReceiveInterAct : IBiliMsg
    {
        public string UserId { get; private set; }
        public string UserName { get; private set; }
        public ReceiveInterAct(string json)
        {
            JObject jo = JObject.Parse(json);
            UserId = jo["data"]["uid"].ToString();
            UserName = jo["data"]["uname"].ToString();
        }
    }
}
