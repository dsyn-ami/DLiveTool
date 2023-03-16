using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLiveTool
{
    /// <summary>
    /// 辅助工具，将数据写入本地文件
    /// </summary>
    public class BiliMsgWriter
    {
        static string _rootPath = DPath.RootPath;

        /// <summary>
        /// 用于记录未处理的消息
        /// </summary>
        /// <param name="cmd">消息类型，作为文件名</param>
        /// <param name="json">消息的内容</param>
        public static void RecordMsg(string cmd, string json)
        {
            string dirPath = Path.Combine(_rootPath, "msgCache");
            string filePath = Path.Combine(dirPath, cmd + ".txt");
            if (File.Exists(filePath)) return;
            if(!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            using(StreamWriter sw = new StreamWriter(filePath))
            {
                sw.Write(json);
            }
        }
    }
}
