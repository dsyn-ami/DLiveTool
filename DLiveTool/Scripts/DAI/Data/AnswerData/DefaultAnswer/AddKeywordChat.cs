using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI
{
    public class AddKeywordChat
    {
        List<string> _chatMsg = new List<string>
        {
            "原来要这样回答呀, 学会了汪",
            "不知道这是什么, 总之先记下了汪",
            "学会了汪",
            "学到新知识了汪, 开心汪",
            "你都在教我些什么呀汪",
            "是新知识汪",
            "要长脑子了汪, 要长脑子了汪汪",
            "这么复杂的东西是汪可以学的吗",
            "嗯, 我懂, 我懂汪",
            "难道, 我其实是天才吗汪",
            "要回答{answer}吗, 汪?",
            "原来是{answer}呀汪",
            "没错, 是{answer}汪",
            "{userName}教的, 我会好好记住的汪",
            "是{userName}的新知识汪!",
            "谢谢{userName}, 学会了汪",
            "太简单了汪, 有难点的吗, 就难一点汪",
            "学会了, 这就去对线汪",
            "{keyword}, 然后是{answer}汪",
            "奇怪的知识增加了汪",
            "原来如此, 学会了汪",
            "谢谢{userName}老师的新知识汪",
        };
        string _userName = "{userName}";
        string _keyword = "{keyword}";
        string _answer = "{answer}";
        /// <summary>
        /// 如果没有匹配的关键词, 返回空
        /// </summary>
        /// <param name="chatMsg">弹幕消息</param>
        /// <returns></returns>
        public string GetRandomMsg(string userName, string keyword, string answer)
        {
            Random random = new Random();
            int index = random.Next(0, _chatMsg.Count);
            string msg = _chatMsg[index].Replace(_userName, userName).Replace(_keyword, keyword).Replace(_answer, answer);
            if (msg.Length <= 20) return msg;
            //前三条长度必定小于20
            else return _chatMsg[random.Next(0, 10)].Replace(_userName, userName).Replace(_keyword, keyword).Replace(_answer, answer);
        }
    }
}
