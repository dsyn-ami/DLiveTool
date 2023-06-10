using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI
{
    [Serializable]
    public class KeywordAnswer
    {
        /// <summary>
        /// 创建的用户ID, 只记录第一个
        /// </summary>
        public string CreaterUID { get; set; }
        /// <summary>
        /// 创建时的用户名
        /// </summary>
        public string CreaterName { get; set; }
        /// <summary>
        /// 创建的时间戳
        /// </summary>
        public long CreateTimeStamp { get; set; }
        /// <summary>
        /// 关键词
        /// </summary>
        public string Keyword { get; set; }
        /// <summary>
        /// 回答, 一个关键词可以匹配多个回答
        /// </summary>
        public List<string> Answer { get; set; }
    }
    [Serializable]
    public class KeywordAnswerDatas
    {
        public List<KeywordAnswer> KeywordAnswer = new List<KeywordAnswer>();
    }
}
