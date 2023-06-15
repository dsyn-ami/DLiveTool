using DLiveTool.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DAI;
using System.Threading.Tasks;

namespace DLiveTool
{
    /// <summary>
    /// 负责监听弹幕, 和DAI对接返回DAI消息
    /// </summary>
    public class DAISystem
    {
        DAIConfig _daiConfig = ConfigDataMgr.Instance.Data.DAIConfig;

        #region 初始化
        public void Init()
        {
            AddListener();
            StartTimerAsync();
        }
        void AddListener()
        {
            DConnection.BiliWS.OnReceiveDanmaku += OnReceiveDanmaku;
            DConnection.BiliWS.OnReceiveGift += OnReceiveGift;
            DConnection.BiliWS.OnUserEnter += OnUserEnter;
            OnTimerUpdataAction += OnTimerUpdata;
        }


        #endregion

        #region 计时系统
        /// <summary>
        /// 当前计时系统经过的时间
        /// </summary>
        long _curTime;
        /// <summary>
        /// 上一次发送弹幕的时间
        /// </summary>
        long _lastSendDanmakuTime;
        /// <summary>
        /// 超过指定时间没发弹幕, 发送闲聊弹幕
        /// </summary>
        long _freeChatTime;
        /// <summary>
        ///计时器更新回调
        /// </summary>
        public Action<long> OnTimerUpdataAction;
        Random _rand = new Random();
        private async void StartTimerAsync()
        {
            _curTime = 0;
            _lastSendDanmakuTime = -5;
            _freeChatTime = _rand.Next(120, 1200);
            while (true)
            {
                await Task.Delay(1000);
                _curTime += 1;
                OnTimerUpdataAction?.Invoke(_curTime);
            }
        }
        #endregion

        #region 监听函数
        private void OnTimerUpdata(long obj)
        {
            //超过一定时间没发弹幕, 发闲聊弹幕
            if(_curTime - _lastSendDanmakuTime > _freeChatTime)
            {
                _freeChatTime = _rand.Next(120, 1200);
                OutputMsg output = DAIMgr.Instance.DAICore.FreeChat();
                if(output != null)
                {
                    SendDanmaku(output.Msg);
                }
            }
        }

        private void OnUserEnter(ReceiveInterAct msg)
        {
            long intimacy = FansDataMgr.Instance.GetFansIntimacy(msg.UserId, msg.UserName);
            //亲密度足够回复欢迎弹幕
            OutputMsg output = DAIMgr.Instance.DAICore.WelcomeFansChat(msg.UserName, intimacy);
            if(output != null)
            {
                SendDanmaku(output.Msg);
            }
        }

        private void OnReceiveGift(ReceiveSendGift msg)
        {
            //收到礼物,增加亲密度
            long add = msg.GiftCount * msg.Price / 100;
            FansDataMgr.Instance.AddFansIntimacy(msg.UserId, msg.UserName, add);

            //获取收到礼物回复
            OutputMsg output = DAIMgr.Instance.DAICore.GiftChat(msg.UserName, msg.GiftName);
            if(output != null)
            {
                SendDanmaku(output.Msg);
            }
        }

        private void OnReceiveDanmaku(ReceiveDanmakuMsg msg)
        {
            //收到弹幕,增加亲密度
            FansDataMgr.Instance.AddFansIntimacy(msg.UserId, msg.UserName, 1);
            //弹幕机器人发的弹幕, 跳过处理
            if (_daiConfig.InCookie_UserId.Equals(msg.UserId)) return;
            //获取弹幕回复, 只处理文本类型弹幕
            if (msg.Type == ReceiveDanmakuMsg.DanmakuType.Text)
            {
                InputMsg input = new InputMsg();
                input.UID = msg.UserId;
                input.UserName = msg.UserName;
                input.Msg = msg.Message;
                input.InputType = MaskConvert.GetInputMask(InputType.LiveRoom);
                OutputMsg output = DAIMgr.Instance.HandleInput(input);
                if (output != null)
                {
                    SendDanmaku(output.Msg);
                }
            }
        }
        #endregion

        #region 其他函数
        long _sendDanmakuCd = 3;
        /// <summary>
        /// 使用填的cookie账号向当前连接直播间发送弹幕
        /// </summary>
        /// <param name="msg"></param>
        void SendDanmaku(string msg)
        {
            if (_daiConfig.IsAllowDanmakuResponse)
            {
                //三秒内不会连续发弹幕
                if (_curTime - _lastSendDanmakuTime <= _sendDanmakuCd) return;
                _lastSendDanmakuTime = _curTime;
                BiliRequester.SendDanmakuAsync(msg, isSuccess =>
                {
                    if (isSuccess)
                    {
                        _sendDanmakuCd = 3;
                    }
                    else
                    {
                        //每失败一次, cd加5, 避免连续发送失败请求
                        _sendDanmakuCd += 5;
                    }
                });
            }
        }
        #endregion
    }
}
