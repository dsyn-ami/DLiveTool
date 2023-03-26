using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLiveTool
{
    /// <summary>
    /// 软件配置数据
    /// </summary>
    [Serializable]
    public class DLiveConfigData
    {
        /// <summary>
        /// 记录上一次连接的房间id
        /// </summary>
        public string RoomId;
        /// <summary>
        /// 是否显示进入直播间信息
        /// </summary>
        public bool IsShowEnterInfo = true;
        /// <summary>
        /// 屏蔽用户名关键词列表
        /// </summary>
        public List<string> AvoidNameKeyWordList = new List<string>();
    }
}
