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
        /// 弹幕窗口相关配置
        /// </summary>
        public DanmakuWindowConfig DanmakuWindowConfig = new DanmakuWindowConfig();
        /// <summary>
        /// DAI相关配置
        /// </summary>
        public DAIConfig DAIConfig = new DAIConfig();
    }
}
