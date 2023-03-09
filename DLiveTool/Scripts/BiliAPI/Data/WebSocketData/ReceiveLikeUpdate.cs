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
    "cmd":"LIKE_INFO_V3_UPDATE",
    "data":{
        "click_count":40716
    }
}
     */

    /// <summary>
    /// 点赞数更新
    /// </summary>
    public class ReceiveLikeUpdate
    {
        /// <summary>
        /// 总共的点赞数
        /// </summary>
        public int LikeCount { get; private set; }
        public ReceiveLikeUpdate(string json)
        {
            JObject jo = JObject.Parse(json);
            LikeCount = (int)jo["data"]["click_count"];
        }
    }
}
