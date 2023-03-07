using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace DLiveTool
{
    /// <summary>
    /// 进行各种请求
    /// </summary>
    public class BiliRequester
    {
        /// <summary>
        /// 请求房间初始化信息
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public static async Task<string> GetRoomInitInfo(string roomId)
        {
            string url = BiliAPI.RoomInitInfo + "?id=" + roomId;
            return GetStringFromResponse(await HttpGet(url));
        }
        /// <summary>
        /// 请求用户基本信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static async Task<string> GetUserInfo(string UserId)
        {
            string url = BiliAPI.UserInfo + "?mid=" + UserId;
            return GetStringFromResponse(await HttpGet(url));
        }

        /// <summary>
        /// 进行Http GET 请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<HttpWebResponse> HttpGet(string url)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = BiliAPI.UserAgent;
            request.KeepAlive = false;
            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
            return response;
        }

        /// <summary>
        /// 读取 http响应 中的字符串
        /// </summary>
        /// <param name="response"></param>
        /// <param name="isDisposeResponse">读取完后是否释放响应资源</param>
        /// <returns></returns>
        private static string GetStringFromResponse(WebResponse response, bool isDisposeResponse = true)
        {
            Stream stream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(stream, Encoding.UTF8);
            
            string result = streamReader.ReadToEnd();
            
            stream.Dispose();
            if (isDisposeResponse)
            {
                response.Dispose();
            }
            return result;
        }
    }
}
