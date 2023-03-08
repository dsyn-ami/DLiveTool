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
    "cmd":"WATCHED_CHANGE",
    "data":{
        "num":203,
        "text_small":"203",
        "text_large":"203人看过"
        }
    }
     */
    /// <summary>
    /// 看过的人数变更
    /// </summary>
    public class ReceiveWatchedChanged
    {
        /// <summary>
        /// 看过的人数量
        /// </summary>
        public int WatchedCount { get; private set; }
        
        public ReceiveWatchedChanged(string json)
        {
            JObject jo = JObject.Parse(json);
            WatchedCount = (int)jo["data"]["num"];
        }
    }
}
