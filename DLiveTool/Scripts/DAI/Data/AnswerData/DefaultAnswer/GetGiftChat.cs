using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI
{
    public class GetGiftChat
    {
        /// <summary>
        /// 长度为偶数, 且奇数索引不包含通配符
        /// 默认使用偶数索引, 如果偶数索引字符超过20, 使用下一个索引
        /// </summary>
        private List<string> _chatMsg = new List<string>
        {
            "{giftName}收到了, 谢谢{userName}汪!",
            "礼物收到了, 谢谢汪!",
            "汪, 收到{giftName}啦, 汪汪!",
            "汪, 收到礼物啦, 汪汪!",
            "谢谢{userName}的{giftName}汪",
            "谢谢你的礼物汪",
            "你怎么知道我喜欢{giftName}的汪? 谢谢汪",
            "你怎么知道我喜欢这个的汪? 谢谢汪",
            "嗅, 嗅, 发现{giftName}, 收下了汪",
            "嗅, 嗅, 发现礼物, 收下了汪",
            "汪! 是{giftName}, 汪汪汪!",
            "汪! 是礼物, 汪汪汪!",
        };
        private string _userName = "{userName}";
        private string _giftName = "{giftName}";

        public string GetRandomMsg(string userName, string giftName)
        {
            Random random = new Random();
            int index = random.Next(0, _chatMsg.Count / 2) * 2;
            string msg = _chatMsg[index].Replace(_userName, userName).Replace(_giftName, giftName);
            if (msg.Length <= 20) return msg;
            else return _chatMsg[index + 1];
        }
    }
}
