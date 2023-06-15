using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using dsyn;
using System.Text.RegularExpressions;
using DLiveTool.Data;

namespace DLiveTool
{
    /// <summary>
    /// 进行各种请求
    /// </summary>
    public class BiliRequester
    {
        #region 字段区域
        /// <summary>
        /// 用于计算计算 w_rid参数 的乱序索引
        /// </summary>
        private static byte[] _mixinKeyTabel = new byte[] {46, 47, 18, 2, 53, 8, 23, 32, 15, 50, 10, 31, 58, 3, 45, 35, 27, 43, 5, 49,
                                                      33, 9, 42, 19, 29, 28, 14, 39, 12, 38, 41, 13, 37, 48, 7, 16, 24, 55, 40,
                                                      61, 26, 17, 0, 1, 60, 51, 30, 4, 22, 25, 54, 21, 56, 59, 6, 63, 57, 62, 11,
                                                      36, 20, 34, 44, 52};
        /// <summary>
        /// 通过nav请求返回的实时口令动态生成的混合密钥
        /// </summary>
        private static string _mixinKey;
        /// <summary>
        /// 用于发送http请求的客户端
        /// </summary>
        static HttpClient _client = new HttpClient();
        #endregion

        #region 公开函数
        /// <summary>
        /// 请求房间初始化信息
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        public static async Task<string> GetRoomInitInfoAsync(string roomId)
        {
            string url = BiliAPI.RoomInitInfo + "?id=" + roomId;
            return GetStringFromResponse(await HttpGetAsync(url));
        }
        /// <summary>
        /// 请求用户基本信息
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public static async Task<string> GetUserInfoAsync(string UserId)
        {
            //如果没有密钥, 先获取
            if (string.IsNullOrEmpty(_mixinKey))
            {
                _mixinKey = await RequestMixinKeyAsync();
            }

            //获取时间戳
            string wts = DateTimeOffset.Now.ToUnixTimeSeconds().ToString();
            //生成源字符串
            string oriStr = $"mid={UserId}&wts={wts}{_mixinKey}";
            //源字符串进行md5加密获得参数w_rid
            string w_rid = MD5Encoder.GetMD5(oriStr);

            string param = $"mid={UserId}&w_rid={w_rid}&wts={wts}";

            string url = BiliAPI.GetUserInfo + $"?{param}";
            return GetStringFromResponse(await HttpGetAsync(url));
        }

        /// <summary>
        /// 进行Http GET 请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<HttpWebResponse> HttpGetAsync(string url)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.UserAgent = BiliAPI.UserAgent;
            request.KeepAlive = false;
            HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
            return response;
        }

        static Dictionary<string, string> _postDataDict = new Dictionary<string, string>();
        /// <summary>
        /// 在已连接房间发送弹幕
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="callback">发送完回调 bool 是否发送成功</param>
        public static async void SendDanmakuAsync(string msg, Action<bool> callback = null)
        {
            string roomId = AnchorData.RoomId.Value;
            string cookie = ConfigDataMgr.Instance.Data.DAIConfig.Cookie.ToString();
            //csrf 就是在 cookie 中的 bili_jct 的值
            string csrf = ConfigDataMgr.Instance.Data.DAIConfig.InCookie_csrf;
            if (string.IsNullOrEmpty(cookie) || string.IsNullOrEmpty(roomId))
            {
                Console.WriteLine("发送弹幕失败, cookie或房间号为空");
                return;
            }
            _postDataDict.Clear();
            _postDataDict["bubble"] = "0";
            _postDataDict["msg"] = msg;
            _postDataDict["mode"] = "1";
            _postDataDict["roomid"] = roomId;
            _postDataDict["csrf"] = csrf;
            _postDataDict["csrf_token"] = csrf;
            _postDataDict["rnd"] = Time.GetTimeStamp().ToString();
            _postDataDict["color"] = "16777215";
            _postDataDict["fontsize"] = "25";
            FormUrlEncodedContent content = new FormUrlEncodedContent(_postDataDict);
            content.Headers.Add("cookie", cookie);
            HttpResponseMessage response = await _client.PostAsync(BiliAPI.SendDanmaku, content);
            string responseStr = await response.Content.ReadAsStringAsync();
            SendDanmakuResponse res = new SendDanmakuResponse(responseStr);
            Console.WriteLine($"[{msg}] 发送{(res.Code == 0 ? "成功" : "失败")} {res.ErrorMsg}");
            response.Dispose();
            callback?.Invoke(res.Code == 0);
        }
        #endregion

        #region 其他函数
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
        /// <summary>
        /// 获取实时口令并生成混合密钥
        /// </summary>
        /// <returns></returns>
        private static async Task<string> RequestMixinKeyAsync()
        {
            //获取wbi实时口令
            string nvaInfo = GetStringFromResponse(await HttpGetAsync(BiliAPI.NavInfo));
            JObject nvaJObj = JObject.Parse(nvaInfo);
            //拿到伪装成图片地址的实时口令
            string imgUrl = nvaJObj["data"]["wbi_img"]["img_url"].ToString();
            string subUrl = nvaJObj["data"]["wbi_img"]["sub_url"].ToString();
            //对实时口令进行处理
            //只取图片地址名字的部分
            string imgKey = Path.GetFileNameWithoutExtension(imgUrl);
            string subKey = Path.GetFileNameWithoutExtension(subUrl);
            //拼接两个Key, 
            string mixKey = imgKey + subKey;
            //然后按照_mixinKeyTabel索引取前32位
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < 32; i++)
            {
                sb.Append(mixKey[_mixinKeyTabel[i]]);
            }
            return sb.ToString();
        }
        #endregion
    }
}
