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
        static string _appKey = "";
        static string _appSecret = "";

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
            _dict["q"] = instance._text;
            _dict["salt"] = DateTime.Now.Millisecond.ToString();
            _dict["langType"] = "zh-CHS";
            _dict["voice"] = "6";
            _dict["speed"] = "1";
            _dict["volume"] = "3";
            _dict["appKey"] = _appKey;
            
            string signStr = _dict["appKey"] + _dict["q"] + _dict["salt"] + _appSecret;

            _dict["sign"] = ComputeHash(signStr, MD5.Create());

            //dic.Add("q", System.Web.HttpUtility.UrlEncode(q));

            FormUrlEncodedContent content = new FormUrlEncodedContent(_dict);
            try
            {
                using (HttpResponseMessage response = await _client.PostAsync(TtsApi.TtsUrl, content))
                {
                    System.Net.Http.Headers.MediaTypeHeaderValue contentType = response.Content.Headers.ContentType;
                    Stream stream = await response.Content.ReadAsStreamAsync();
                    return await FileWriter.WriteFileAsync(instance._path, stream);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"load voice failed : " + e.Message);
                return false;
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
