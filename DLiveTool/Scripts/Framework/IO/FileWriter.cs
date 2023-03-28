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
            catch
            {
                return false;
            }
        }
        #endregion

        #region 异步操作
        /// <summary>
        /// 将流写入文件
        /// </summary>
        /// <param name="path">文件目标路径</param>
        /// <param name="dataStream">数据流</param>
        /// <returns></returns>
        public async static Task<bool> WriteFileAsync(string path, Stream dataStream)
        {
            //读取字节流,并写入本地文件
            byte[] buffer = new byte[1024];
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
                {
                    int length = 0;
                    do
                    {
                        length = await dataStream.ReadAsync(buffer, 0, 1024);
                        await fs.WriteAsync(buffer, 0, length);
                    }
                    while (length > 0);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("WriteFile Error" + e);
                return false;
            }
            return true;
        }
        #endregion
    }
}
