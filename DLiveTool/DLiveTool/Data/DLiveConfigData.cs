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

        public DanmakuWindowConfig DanmakuWindowConfig = new DanmakuWindowConfig();

    }
}
