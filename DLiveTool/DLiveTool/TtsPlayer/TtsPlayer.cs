using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using dsyn;

namespace DLiveTool
{
    /// <summary>
    /// 管理播放语音
    /// </summary>
    public class TtsPlayer : Singleton<TtsPlayer>
    {
        MediaPlayer _player = new MediaPlayer();
        private Queue<TtsInstance> _playQueue = new Queue<TtsInstance>();
        /// <summary>
        /// 当前正在播放或正在等待播放的实例
        /// </summary>
        TtsInstance _curIns;

        private long _voiceFileId = 1000;

        private TtsPlayer()
        {
            _player.MediaEnded += OnAudioPlayEnd;
            _player.Volume = 1;
        }


        private void OnAudioPlayEnd(object sender, EventArgs e)
        {
            _curIns?.DestroySelf();
            _curIns = null;
            if(Count > 0)
            {
                var instance = GetPlayInstance();
                PlayInstance(instance);
            }
        }

        /// <summary>
        /// 播放声音
        /// </summary>
        /// <param name="ins"></param>
        private void PlayInstance(TtsInstance ins)
        {
            if (_curIns != null) return;
            //加载失败，不播放
            if(ins._loadState == 2)
            {
                OnAudioPlayEnd(ins, null);
            }
            //加载完成直接播放
            else if(ins._loadState == 1)
            {
                _curIns = ins;
                _player.Open(new Uri(ins._path));
                _player.Play();
            }
            //加载中，等待加载完成后播放
            else if(ins._loadState == 0)
            {
                _curIns = ins;
                _curIns.OnVoiceLoaded += i =>
                {
                    if(ins._loadState == 1)
                    {
                        _player.Open(new Uri(ins._path));
                        _player.Play();
                    }
                    else
                    {
                        OnAudioPlayEnd(ins, null);
                    }
                };
            }

        }

        /// <summary>
        /// 添加待播放语音的文本到队列
        /// </summary>
        /// <param name="text"></param>
        public void AddPlayInstance(string text)
        {
            //暂时关闭语音播报
            //return;
            //等待队列大于5，不再生成语音
            if (_playQueue.Count > 5) return;

            _playQueue.Enqueue(new TtsInstance(text, _voiceFileId++));
            if(_curIns == null)
            {
                var ins = _playQueue.Dequeue();
                PlayInstance(ins);
            }
        }
        /// <summary>
        /// 取出
        /// </summary>
        /// <returns></returns>
        private TtsInstance GetPlayInstance()
        {
            if(_playQueue.Count > 0)
            {
                return _playQueue.Dequeue();
            }
            return null;
        }
        public int Count => _playQueue.Count;
    }
}
