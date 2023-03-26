using dsyn;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DLiveTool
{
    public class TtsRequester
    {
        static HttpClient _client = new HttpClient();

        public static async Task<bool> RequsetTtsDeemoAsync(TtsInstance instance)
        {
            Dictionary<string, string> _dict = new Dictionary<string, string>();
            _dict["text"] = instance._text;
            _dict["lan"] = "zh-CHS";
            _dict["voice"] = "6";
            _dict["speed"] = "1";
            _dict["volume"] = "3";
            FormUrlEncodedContent content = new FormUrlEncodedContent(_dict);
            HttpResponseMessage response = await _client.PostAsync(TtsApi.TtsDemoUrl, content);

            string responseJson = await response.Content.ReadAsStringAsync();
            string audioUrl = responseJson.Split('\"')[3];
            response.Dispose();
            try
            {
                using (response = await _client.GetAsync(audioUrl))
                {
                    Stream stream = await response.Content.ReadAsStreamAsync();
                    return await FileWriter.WriteFileAsync(instance._path, stream);
                }
            }
            catch (Exception)
            {
                MessageBox.Show($"load voice failed : \njson : {responseJson} \nurl : {audioUrl}");
     ;          return false;
            }

        }

        public static async Task<bool> RequsetTtsAsync(TtsInstance instance)
        {
            Dictionary<string, string> _dict = new Dictionary<string, string>();
            _dict["text"] = instance._text;
            _dict["lan"] = "zh-CHS";
            _dict["voice"] = "6";
            _dict["speed"] = "1";
            _dict["volume"] = "3";


            //string url = "https://openapi.youdao.com/ttsapi";
            //string q = "待输入的文字";
            //string appKey = "您的应用ID";
            //string appSecret = "您的应用密钥";
            //string salt = DateTime.Now.Millisecond.ToString();
            //string langType = "合成文本的语言类型";
            //dic.Add("langType", langType);
            //string signStr = appKey + q + salt + appSecret; ;
            //string sign = ComputeHash(signStr, new MD5CryptoServiceProvider());
            //dic.Add("q", System.Web.HttpUtility.UrlEncode(q));
            //dic.Add("appKey", appKey);
            //dic.Add("salt", salt);
            //dic.Add("sign", sign);



            FormUrlEncodedContent content = new FormUrlEncodedContent(_dict);
            HttpResponseMessage response = await _client.PostAsync(TtsApi.TtsUrl, content);

            string responseJson = await response.Content.ReadAsStringAsync();
            string audioUrl = responseJson.Split('\"')[3];
            response.Dispose();
            try
            {
                using (response = await _client.GetAsync(audioUrl))
                {
                    Stream stream = await response.Content.ReadAsStreamAsync();
                    return await FileWriter.WriteFileAsync(instance._path, stream);
                }
            }
            catch (Exception)
            {
                MessageBox.Show($"load voice failed : \njson : {responseJson} \nurl : {audioUrl}");
                ; return false;
            }

        }

        protected static string ComputeHash(string input, HashAlgorithm algorithm)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "");
        }
    }
}
