using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace dsyn
{
    public static class FileWriter
    {
        #region 同步操作
        public static bool WriteJsonObj<T>(string path, T obj) where T : class
        {
            string str = JsonConvert.SerializeObject(obj);
            return WriteString(path, str);
        }
        public static bool WriteString(string path, string data)
        {
            return WriteFile(path, Encoding.UTF8.GetBytes(data));
        }
        #endregion

        #region 异步操作

        #endregion

        #region 私有函数
        public static bool WriteFile(string path, byte[] data)
        {
            try
            {
                FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
                //清空原文件内容
                fileStream.SetLength(0);
                //写入新内容
                fileStream.Write(data, 0, data.Length);

                fileStream.Dispose();
                fileStream = null;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion
    }
}
