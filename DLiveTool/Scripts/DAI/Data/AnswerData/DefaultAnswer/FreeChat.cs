using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI
{
    public class FreeChat
    {
        List<string> _chatMsg = new List<string>
        {
            "汪!",
            "早上好汪!",
            "汪上好!",
            "汪! 关汪主播汪! 关汪主播谢谢汪!",
            "汪, 汪汪!",
            "本汪, 可不是坏汪",
            "别走呀汪, 多看一眼呀汪",
            "汪! 还认识这个字吗>_< 汪",
            "想说一句不含汪的话"
        };
        public string GetRandomMsg()
        {
            Random random = new Random();
            return _chatMsg[random.Next(0, _chatMsg.Count)];
        }
    }
}
