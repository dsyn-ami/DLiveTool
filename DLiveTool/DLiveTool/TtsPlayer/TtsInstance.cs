using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dsyn;

namespace DLiveTool
{
    /// <summary>
    /// 一个待播放的Tts语音实例
    /// </summary>
    public class TtsInstance
    {
        /// <summary>
        /// 音频加载完成的回调
        /// </summary>
        public Action<TtsInstance> OnVoiceLoaded;

        /// <summary>
        /// 语音加载状态
        /// 0 ：未加载
        /// 1 ：加载成功
        /// 2 ：加载失败
        /// </summary>
        public byte _loadState;
        /// <summary>
        /// 播放语音的文本
        /// </summary>
        public string _text;
        /// <summary>
        /// 语音文件本地路径
        /// </summary>
        public string _path;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="text">语音内容文本</param>
        /// <param name="fileId">语音文件名字id</param>
        public TtsInstance(string text, long fileId)
        {
            _text = text;
            _path = Path.Combine(DPath.VoiceCacheRootPath, fileId.ToString() + ".mp3");
            _loadState = 0;
            RequestVoice();
        }

        private async void RequestVoice()
        {
            bool isSuccess = await TtsRequester.RequsetTtsDeemoAsync(this);
            if (isSuccess)
            {
                _loadState = 1;
                OnVoiceLoaded?.Invoke(this);
            }
            else
            {
                _loadState = 2;
                OnVoiceLoaded?.Invoke(this);
            }
        }
        /// <summary>
        /// 清空本地缓存，使用完该实例后调用
        /// </summary>
        public void DestroySelf()
        {
            if (File.Exists(_path))
            {
                File.Delete(_path);
            }
        }
    }
}
