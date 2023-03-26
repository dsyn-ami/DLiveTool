using System;
using System.IO;
using System.Web;
using System.Net;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Net.Http;
using System.Collections;

namespace TestAudio
{
    class TtsDemo
    {
        HttpClient _client = new HttpClient();
        string _url = "https://aidemo.youdao.com/ttsapi";
        public async Task<string> Start(string msg)
        {
            Dictionary<string, string> _dict = new Dictionary<string, string>();
            _dict["text"] = msg;
            _dict["lan"] = "zh-CHS";
            //_dict["appKey"] = "3f13cc8595f8df49";
            //_dict["salt"] = DateTime.Now.Millisecond.ToString();

            //string sign = _dict["appKey"] + _dict["q"] + _dict["salt"] + "GvRslXNot8IdlkSRY42vlL3QXoul7rof";
            //sign = ComputeHash(sign, new MD5CryptoServiceProvider());
            //_dict["sign"] = sign;

            _dict["voice"] = "6";
            _dict["speed"] = "1";
            //_dict["volume"] = "1";

            FormUrlEncodedContent content = new FormUrlEncodedContent(_dict);
            HttpResponseMessage response = await _client.PostAsync(_url, content);

            string responseJson = await response.Content.ReadAsStringAsync();

            string audioUrl = responseJson.Split('\"')[3];

            response.Dispose();

            response = await _client.GetAsync(audioUrl);
            Stream stream = await response.Content.ReadAsStreamAsync();

            string path = SaveBinaryFile(stream, "E:/audio.mp3");
            return path;
        }


        protected static string ComputeHash(string input, HashAlgorithm algorithm)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "");
        }

        private static string SaveBinaryFile(Stream inStream, string FileName)
        {

            string FilePath = FileName + ".mp3";
            byte[] buffer = new byte[1024];
            try
            {
                if (File.Exists(FilePath))
                    File.Delete(FilePath);
                Stream outStream = System.IO.File.Create(FilePath);

                int l;
                do
                {
                    l = inStream.Read(buffer, 0, buffer.Length);
                    if (l > 0)
                        outStream.Write(buffer, 0, l);
                }
                while (l > 0);

                outStream.Close();
                inStream.Close();


            }
            catch (Exception ex)
            {

            }
            return FilePath;
        }
    }
    class TtsReal
    {
        HttpClient _client = new HttpClient();
        string _url = "https://openapi.youdao.com/ttsapi";
        public async Task<string> Start(string msg)
        {
            Dictionary<string, string> _dict = new Dictionary<string, string>();
            _dict["q"] = msg;
            _dict["langType"] = "zh-CHS";
            _dict["appKey"] = "3f13cc8595f8df49";
            _dict["salt"] = DateTime.Now.Millisecond.ToString();

            string sign = _dict["appKey"] + _dict["q"] + _dict["salt"] + "GvRslXNot8IdlkSRY42vlL3QXoul7rof";
            sign = ComputeHash(sign, new MD5CryptoServiceProvider());
            _dict["sign"] = sign;

            _dict["voice"] = "6";
            _dict["speed"] = "1";
            _dict["volume"] = "1";

            FormUrlEncodedContent content = new FormUrlEncodedContent(_dict);
            HttpResponseMessage response = await _client.PostAsync(_url, content);
            Stream stream = await response.Content.ReadAsStreamAsync();
            string path = SaveBinaryFile(stream, "E:/audio.mp3");
            return path;
        }


        protected static string ComputeHash(string input, HashAlgorithm algorithm)
        {
            Byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            Byte[] hashedBytes = algorithm.ComputeHash(inputBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "");
        }

        private static string SaveBinaryFile(Stream inStream, string FileName)
        {
            string FilePath = FileName + ".mp3";
            byte[] buffer = new byte[1024];
            try
            {
                if (File.Exists(FilePath))
                    File.Delete(FilePath);
                Stream outStream = System.IO.File.Create(FilePath);

                int l;
                do
                {
                    l = inStream.Read(buffer, 0, buffer.Length);
                    if (l > 0)
                        outStream.Write(buffer, 0, l);
                }
                while (l > 0);

                outStream.Close();
                inStream.Close();

            }
            catch (Exception ex)
            {

            }
            return FilePath;
        }
    }
}

