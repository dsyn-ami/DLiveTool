using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLiveTool
{
    public class BiliAPI
    {
        /// <summary>
        /// 访问的设备代理
        /// </summary>
        public const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/110.0.0.0 Safari/537.36 Edg/110.0.1587.57";
        
        /// <summary>
        /// 连接b站直播弹幕消息的ws地址
        /// </summary>
        public const string LiveWebSocketUrl = "wss://broadcastlv.chat.bilibili.com:443/sub";

        /// <summary>
        /// 获取房间基本信息
        /// GET
        /// id : 房间号
        /// </summary>
        public const string RoomInitInfo = "https://api.live.bilibili.com/room/v1/Room/room_init";

        /// <summary>
        /// 获取用户基本信息
        /// GET
        /// mid : 用户uid
        /// </summary>
        public const string UserInfo = "https://api.bilibili.com/x/space/acc/info";
    }
}
