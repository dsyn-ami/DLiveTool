using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace dsyn
{
    /// <summary>
    /// MD5: 计算字符串的哈希值, 然后转换为16进制字符串
    /// </summary>
    public static class MD5Encoder
    {
        private static StringBuilder _sb;
        private static MD5 _md5;

        /// <summary>
        /// 获得指定字符串的MD5
        /// </summary>
        /// <param name="oriStr"></param>
        /// <param name="format">转换格式, 对于每个byte, x表示小写字母16进制, 2表示使用两个字符, X表示大写字母16进制</param>
        /// <returns></returns>
        public static string GetMD5(string oriStr, string format = "x2")
        {
            //初始化
            if(_sb == null) _sb = new StringBuilder();
            if(_md5 == null) _md5 = MD5.Create();
            _sb.Clear();
            
            //计算哈希值, hash.Length = 16
            byte[] hash = _md5.ComputeHash(Encoding.UTF8.GetBytes(oriStr));
            //然后转换为16进制字符串, format = "x2" 表示生成由小写字母和数字组成的32个字符的字符串
            for (int i = 0; i < hash.Length; i++)
            {
                _sb.Append(hash[i].ToString(format));
            }
            return _sb.ToString();
        }
    }
}
