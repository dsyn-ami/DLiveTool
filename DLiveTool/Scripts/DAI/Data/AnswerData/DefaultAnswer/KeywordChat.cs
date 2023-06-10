using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI
{
    public class KeywordChat
    {
        List<string> _chatMsg = new List<string>
        {
            "{answer}",
            "{answer}汪",
        };
        string _answer = "{answer}";
        /// <summary>
        /// 如果没有匹配的关键词, 返回空
        /// </summary>
        /// <param name="chatMsg">弹幕消息</param>
        /// <returns></returns>
        public string GetRandomMsg(string chatMsg)
        {
            Random random = new Random();
            var answers = KeywordAnswerDataMgr.Instance.FindKeywordAnswer(chatMsg);
            if(answers != null && answers.Length > 0)
            {
                var tarAnswer = answers[random.Next(0, answers.Length)];
                if(tarAnswer.Answer != null && tarAnswer.Answer.Count > 0)
                {
                    string answer = tarAnswer.Answer[random.Next(0, tarAnswer.Answer.Count)];
                    return _chatMsg[random.Next(0, _chatMsg.Count)].Replace(_answer, answer);
                }
            }
            return null;
        }
    }
}
