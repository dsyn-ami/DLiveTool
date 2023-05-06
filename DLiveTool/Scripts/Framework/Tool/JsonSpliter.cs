using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dsyn
{
    /// <summary>
    /// 拆分一个字符串中的多个json结构
    /// </summary>
    public class JsonSpliter
    {
        string _oriStr;

        int _count = 0;
        /// <summary>
        /// json结构数量
        /// </summary>
        public int Count => _count;

        /// <summary>
        /// 记录拆分索引的字典
        /// int:第一个索引
        /// IndexInfo:索引信息
        /// </summary>
        List<IndexInfo> _indexInfoList = new List<IndexInfo>();

        public JsonSpliter(string jsonStrs)
        {
            char leftChar = '{';
            char rightChar = '}';
            char strChar = '\"';
            char escapeChar = '\\';
            _oriStr = jsonStrs;

            int curIndex = 0;
            while(curIndex < jsonStrs.Length)
            {
                int startIndex = curIndex;
                int deep = 0;
                bool isInStr = false;
                for( ; curIndex < jsonStrs.Length; curIndex++)
                {
                    //判断当前字符是否是在字符串中
                    if (jsonStrs[curIndex].Equals(strChar) && !jsonStrs[curIndex - 1].Equals(escapeChar))
                    {
                        isInStr = !isInStr;
                    }

                    //如果当前字符在字符串中, 不处理,
                    //如 {"ke}y\"s":12} 其中 "ke}y\"s" 里的 } 和中间的 " 不作为分割依据
                    if (!isInStr)
                    {
                        if (jsonStrs[curIndex].Equals(leftChar)) deep += 1;
                        else if (jsonStrs[curIndex].Equals(rightChar))
                        {
                            deep -= 1;
                            if (deep <= 0)
                            {
                                _indexInfoList.Add(new IndexInfo(startIndex, curIndex));
                                _count += 1;
                                curIndex += 1;
                                break;
                            }
                        }
                    }
                }
            }
        }

        public string this[int index]
        {
            get
            {
                IndexInfo indexInfo = _indexInfoList[index];
                return _oriStr.Substring(indexInfo.start, indexInfo.end - indexInfo.start + 1); 
            }
        }

        private class IndexInfo
        {
            public int start;
            public int end;
            public IndexInfo(int start, int end)
            {
                this.start = start;
                this.end = end;
            }
        }
    }
}
