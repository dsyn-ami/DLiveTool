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
        [Obsolete("请使用GetUserInfo代替")]
        public const string UserInfo = "https://api.bilibili.com/x/space/acc/info";
        /// <summary>
        /// 获取用户基本信息
        /// GET
        /// mid : 用户UID
                /// platform : "web"
                /// web_location : "150101"
        /// w_rid : wbi权鉴中特殊的字段, 通过某种加密算法生成
        /// wts : wbi权鉴中特殊的字段, 秒级时间戳
        /// </summary>
        public const string GetUserInfo = "https://api.bilibili.com/x/space/wbi/acc/info";
        /// <summary>
        /// 获取导航栏用户信息, 响应数据包含用于计算 wbi签名 的实时口令 : data/wbi_img
        /// </summary>
        public const string NavInfo = "https://api.bilibili.com/x/web-interface/nav";
    }
}
