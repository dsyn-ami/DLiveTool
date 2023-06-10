using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI
{
    public enum OutputType
    {
        /// <summary>
        /// 直播间弹幕
        /// </summary>
        LiveRoom = 1,
        /// <summary>
        /// 控制台
        /// </summary>
        Console = 2,
        /// <summary>
        /// 本地WebSocket推送
        /// </summary>
        WebSocket = 4,
    }
    public class OutputMsg
    {
        /// <summary>
        /// 输出内容
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 输出目标, 可以包括多个目标
        /// </summary>
        public byte OutPutTargetMask { get; set; }
    }
}
