using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI
{
    /// <summary>
    /// 不同亲密度触发不同对话, 目前只会触发100级别对话
    /// </summary>
    public class WelcomeFansChat
    {
        private List<string> _chatMsg_100 = new List<string>
        {
            "汪! {userName}来啦!",
            "欢迎{userName}汪",
            "欢迎{userName}, 汪上好汪",
            "{userName}来啦汪, 欢迎汪",
            "汪, 汪, 欢迎{userName}!",
            "你好汪,{userName}",
        };
        private List<string> _chatMsg_300 = new List<string>
        {

        };
        private List<string> _chatMsg_500 = new List<string>
        {

        };
        string _userName = "{userName}";

        public string GetRandomMsg(string userName, long initmacy)
        {
            if(initmacy >= 100)
            {
                Random rand = new Random();

                string msg = _chatMsg_100[rand.Next(0, _chatMsg_100.Count)];
                int msgCount = msg.Length - _userName.Length;
                int maxNameLength = 20 - msgCount;
                if (userName.Length > maxNameLength)
                {
                    userName = userName.Substring(0, maxNameLength);
                }
                msg.Replace(_userName, userName);
                return msg;
            }
            else
            {
                return null;
            }
        }
    }
}
