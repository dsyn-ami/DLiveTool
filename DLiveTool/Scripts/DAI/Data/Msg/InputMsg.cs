using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI
{
    public enum InputType
    {
        /// <summary>
        /// 直播间弹幕
        /// </summary>
        LiveRoom = 1,
        /// <summary>
        /// 控制台
        /// </summary>
        Console = 2,
    }
    public class InputMsg
    {
        /// <summary>
        /// 输入消息
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 输入来源
        /// </summary>
        public byte InputType { get; set; }
    }
}
