using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLiveTool
{
    /// <summary>
    /// 连接的主播数据
    /// </summary>
    public class AnchorData
    {
        /// <summary>
        /// 主播的uid
        /// </summary>
        public static EventValue<string> UserId;
        /// <summary>
        /// 主播的名字
        /// </summary>
        public static EventValue<string> UserName;
        /// <summary>
        /// 主播头像地址
        /// </summary>
        public static EventValue<string> UserFace;
        /// <summary>
        /// web端主页顶部图片
        /// </summary>
        public static EventValue<string> TopPhoto;
        /// <summary>
        /// 真实直播间房间号
        /// </summary>
        public static EventValue<string> RoomId;
        /// <summary>
        /// 短房间号，部分主播才有的，更好看的房间号
        /// </summary>
        public static EventValue<string> ShotRoomId;
        /// <summary>
        /// 是否直播
        /// </summary>
        public static EventValue<bool> LiveState;
        /// <summary>
        /// 主播直播间地址
        /// </summary>
        public static EventValue<string> RoomUrl;
        /// <summary>
        /// 主播直播间标题
        /// </summary>
        public static EventValue<string> RoomTitle;
        /// <summary>
        /// 主播直播间封面地址
        /// </summary>
        public static EventValue<string> RoomCover;
        /// <summary>
        /// 直播间有多少人看过
        /// </summary>
        public static EventValue<int> WatchedCount;
    }
}
