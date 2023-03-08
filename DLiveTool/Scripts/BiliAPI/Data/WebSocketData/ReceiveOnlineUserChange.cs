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
    "cmd":"ONLINE_RANK_COUNT",
    "data":{
        "count":14
        }
    }
     */

    /// <summary>
    /// 在高能榜上的观众
    /// </summary>
    public class ReceiveOnlineRankChange
    {
        /// <summary>
        /// 在高能榜上观众数量
        /// </summary>
        public int OnRankUserCount { get; private set; }
        public ReceiveOnlineRankChange(string json)
        {
            JObject jo = JObject.Parse(json);
            OnRankUserCount = (int)jo["data"]["count"];
        }
    }
}
