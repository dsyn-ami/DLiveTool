using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace dsyn
{
    /// <summary>
    /// 读取文件相关
    /// </summary>
    public static class FileReader
    {
        #region 同步操作
        /// <summary>
        /// 将文本文件读取为字符串
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string LoadString(string path)
        {
            if (LoadFile(path, out byte[] data))
            {
                return Encoding.UTF8.GetString(data);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 读取Json文件,并将其转换为指定数据模型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T LoadJsonObj<T>(string path) where T : class
        {
            string text = LoadString(path);
            if (string.IsNullOrEmpty(text)) return null;
            return JsonConvert.DeserializeObject<T>(text);
        }

        #endregion

        #region 异步操作

        #endregion

        #region 私有函数
        public static bool LoadFile(string path, out byte[] data)
        {
            try
            {
                FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);

                fileStream.Seek(0, SeekOrigin.Begin);

                data = new byte[fileStream.Length]; //创建文件长度的buffer
                fileStream.Read(data, 0, (int)fileStream.Length);
                fileStream.Close();
                fileStream.Dispose();
                fileStream = null;
                return true;
            }
            catch (Exception e)
            {
                data = null;
                return false;
            }
        }
        #endregion
    }
}
