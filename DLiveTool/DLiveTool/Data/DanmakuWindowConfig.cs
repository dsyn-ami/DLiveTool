using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLiveTool
{
    /// <summary>
    /// 弹幕窗口配置信息
    /// </summary>
    [Serializable]
    public class DanmakuWindowConfig
    {
        /// <summary>
        /// 总是显示在最上层
        /// </summary>
        public bool IsAlwaysTop = false;
        /// <summary>
        /// 最大显示消息数量
        /// </summary>
        public int MaxItemCount = 30;
        /// <summary>
        /// 消息显示时间
        /// </summary>
        public int ItemAliveTime = 30;
        /// <summary>
        /// 是否显示进入直播间信息
        /// </summary>
        public bool IsShowEnterInfo = true;
        /// <summary>
        /// 屏蔽用户名关键词列表
        /// </summary>
        public List<string> AvoidNameKeyWordList = new List<string>();
        /// <summary>
        /// 字体大小
        /// </summary>
        public int FontSize = 18;
        /// <summary>
        /// 行距
        /// </summary>
        public int LinePadding = 10;
        /// <summary>
        /// 滚动动画持续时间 ms
        /// </summary>
        public int RollAnimTime = 150;

        //背景颜色
        public byte BGColorA;
        public byte BGColorR;
        public byte BGColorG;
        public byte BGColorB;

        //消息背景颜色
        public byte MsgBGColorA;
        public byte MsgBGColorR;
        public byte MsgBGColorG;
        public byte MsgBGColorB;
    }
}
