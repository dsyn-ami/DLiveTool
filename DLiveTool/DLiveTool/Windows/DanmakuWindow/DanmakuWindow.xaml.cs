using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DLiveTool.Data;
using System.Windows.Media.Animation;
using dsyn;
using System.IO;

namespace DLiveTool
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class DanmakuWindow : Window
    {
        DanmakuWindowDataModel _model;
        DanmakuWindowConfig _config;
        MediaPlayer _mediaPlayer = new MediaPlayer();
        public DanmakuWindow()
        {
            InitializeComponent();

            _model = new DanmakuWindowDataModel(_mainPanel);
            _config = ConfigDataMgr.Instance.Data.DanmakuWindowConfig;
            _anim.Completed += Anim_Completed;

            DConnection.BiliWS.OnReceiveDanmaku += AddMsgToQueue;
            DConnection.BiliWS.OnUserEnter += AddMsgToQueue;
            DConnection.BiliWS.OnReceiveGift += AddMsgToQueue;
        }

        private void Anim_Completed(object sender, EventArgs e)
        {
            if (_msgQueue.Count > 0)
            {
                IBiliMsg data = _msgQueue.Dequeue();
                HandleMsg(data);
            }
            else
            {
                _isAniming = false;
            }
        }

        /// <summary>
        /// 收到弹幕消息,加入队列
        /// </summary>
        /// <param name="msgData"></param>
        private void AddMsgToQueue(IBiliMsg msgData)
        {
            _msgQueue.Enqueue(msgData);

            if (!_isAniming)
            {
                var data = _msgQueue.Dequeue();
                //动画锁开启
                _isAniming = true;

                HandleMsg(msgData);
            }
        }
        private void HandleMsg(IBiliMsg msg)
        {
            if (msg is ReceiveDanmakuMsg)
            {
                ShowDanmakuMsgAsync(msg as ReceiveDanmakuMsg);
            }
            else if (msg is ReceiveInterAct)
            {
                ShowEnterInfo(msg as ReceiveInterAct);
            }
            else if(msg is ReceiveSendGift)
            {
                ShowReceiveGiftInfo(msg as ReceiveSendGift);
            }
        }

        #region 弹幕消息显示到窗口
        /// <summary>
        /// 是否正在播放显示弹幕的动画
        /// </summary>
        bool _isAniming = false;
        DoubleAnimation _anim = new DoubleAnimation();
        /// <summary>
        /// 待显示的弹幕消息队列
        /// </summary>
        Queue<IBiliMsg> _msgQueue = new Queue<IBiliMsg>();
        /// <summary>
        /// 展示一条弹幕消息
        /// </summary>
        /// <param name="msgData"></param>
        private async void ShowDanmakuMsgAsync(ReceiveDanmakuMsg msgData)
        {
            var data = msgData;

            TtsPlayer.Instance.AddPlayInstance($"{msgData.UserName}说: {msgData.Message}");

            if (data.Type == ReceiveDanmakuMsg.DanmakuType.Text)
            {
                ShowDanmakuMsgText(data.UserName, data.Message);
            }
            else if (data.Type == ReceiveDanmakuMsg.DanmakuType.ImgEmoticon)
            {
                string fileName = data.Emoticon.ImgUrl.Split("/").Last();
                string path = System.IO.Path.Combine(DPath.ImgCachePath, fileName);

                //如果本地没有缓存,先下载图片,并写入本地缓存
                if (string.IsNullOrEmpty(DCache.GetImageCache(fileName)))
                {
                    System.Net.HttpWebResponse response = await BiliRequester.HttpGet(data.Emoticon.ImgUrl);
                    //读取字节流,并写入本地文件

                    using (Stream stream = response.GetResponseStream())
                    {
                        bool isSuccess = await FileWriter.WriteFileAsync(path, stream);
                        if (!isSuccess)
                        {
                            _isAniming = false;
                            return;
                        }
                    }
                }

                ShowDanmakuMsgEmoticon(data.UserName, path, data.Emoticon.Height);
            }
        }
        /// <summary>
        /// 显示弹幕文本
        /// </summary>
        /// <param name="name"></param>
        /// <param name="msg"></param>
        private void ShowDanmakuMsgText(string name, string msg)
        {
            //用户名文本
            Run nameRun = new Run(name + "\u00A0");
            
            nameRun.Foreground = Brushes.Blue;
            //消息文本
            Run msgRun = new Run(msg);
            msgRun.Foreground = Brushes.Black;

            //段落类
            Paragraph para = new Paragraph();
            para.LineHeight = _config.FontSize;
            para.Background = Brushes.Transparent;
            //文本添加到段落类子节点上
            para.Inlines.Add(nameRun);
            para.Inlines.Add(msgRun);

            //FlowDocument类
            FlowDocument flowDocument = new FlowDocument();
            flowDocument.FontSize = _config.FontSize;
            flowDocument.Background = Brushes.Transparent;
            //段落类 加到FlowDocument类子节点上
            flowDocument.Blocks.Add(para);

            RichTextBox box = new RichTextBox();
            //关闭边框
            box.BorderThickness = new Thickness(0);
            //FlowDocument 加到 RichTexBox子结点上
            box.Document = flowDocument;
            box.Background = Brushes.Transparent;
            box.Margin = new Thickness(0, 0, 0, _config.LinePadding);
            box.IsReadOnly = true;
            //添加到父节点上
            _mainPanel.Children.Add(box);

            //保存到数据结构中
            _model.AddRichTexBox(box);

            
            box.Loaded += (s, e) =>
            {
                //组件加载完成后播放动画
                _anim.From = box.ActualHeight + _config.LinePadding;
                _anim.To = 0;
                _anim.Duration = TimeSpan.FromMilliseconds(150);
                rootTrans.BeginAnimation(TranslateTransform.YProperty, _anim);
            };
        }
        /// <summary>
        /// 显示弹幕表情
        /// </summary>
        /// <param name="name"></param>
        /// <param name="emoticonPath"></param>
        /// <param name="height"></param>
        private void ShowDanmakuMsgEmoticon(string name, string emoticonPath, int height)
        {
            //用户名文本
            Run nameRun = new Run(name + "\u00A0");
            nameRun.Foreground = Brushes.Blue;
            //表情图片
            Image emoticonImg = new Image();
            emoticonImg.Source = new BitmapImage(new Uri(emoticonPath, UriKind.Absolute));
            emoticonImg.Height = _config.FontSize + _config.LinePadding;

            //段落类
            Paragraph para = new Paragraph();
            para.LineHeight = _config.FontSize;
            para.Background = Brushes.Transparent;
            //文本添加到段落类子节点上
            para.Inlines.Add(nameRun);
            para.Inlines.Add(emoticonImg);

            //FlowDocument类
            FlowDocument flowDocument = new FlowDocument();
            
            flowDocument.FontSize = _config.FontSize;
            flowDocument.Background = Brushes.Transparent;
            //段落类 加到FlowDocument类子节点上
            flowDocument.Blocks.Add(para);

            RichTextBox box = new RichTextBox();
            //关闭边框
            box.BorderThickness = new Thickness(0);
            //FlowDocument 加到 RichTexBox子结点上
            box.Document = flowDocument;
            box.Margin = new Thickness(0, 0, 0, _config.LinePadding);
            box.Background = Brushes.Transparent;
            box.IsReadOnly = true;
            //添加到父节点上
            _mainPanel.Children.Add(box);

            //保存到数据结构中
            _model.AddRichTexBox(box);


            box.Loaded += (s, e) =>
            {
                //组件加载完成后播放动画
                _anim.From = box.ActualHeight + _config.LinePadding;
                _anim.To = 0;
                _anim.Duration = TimeSpan.FromMilliseconds(_config.RollAnimTime);
                rootTrans.BeginAnimation(TranslateTransform.YProperty, _anim);
            };
        }

        /// <summary>
        /// 显示进入直播间信息
        /// </summary>
        /// <param name="name"></param>
        private void ShowEnterInfo(ReceiveInterAct enterInfo)
        {
            //未打开开关，不显示进入房间信息
            if (_config.IsShowEnterInfo == false)
            {
                _isAniming = false;
                return;
            }
            
            var avoidList = _config.AvoidNameKeyWordList;
            //被屏蔽的用户不显示
            foreach (var avoidName in avoidList)
            {
                if (enterInfo.UserName.Contains(avoidName))
                {
                    _isAniming = false;
                    return;
                }
            }
            TtsPlayer.Instance.AddPlayInstance($"欢迎{enterInfo.UserName}进入直播间");

            //用户名文本
            Run nameRun = new Run(enterInfo.UserName + "\u00A0");

            nameRun.Foreground = Brushes.Blue;
            //消息文本
            Run msgRun = new Run("进入直播间");
            msgRun.Foreground = Brushes.Gray;

            //段落类
            Paragraph para = new Paragraph();
            para.LineHeight = _config.FontSize;
            para.Background = Brushes.Transparent;
            //文本添加到段落类子节点上
            para.Inlines.Add(nameRun);
            para.Inlines.Add(msgRun);

            //FlowDocument类
            FlowDocument flowDocument = new FlowDocument();
            flowDocument.FontSize = _config.FontSize;
            flowDocument.Background = Brushes.Transparent;
            //段落类 加到FlowDocument类子节点上
            flowDocument.Blocks.Add(para);

            RichTextBox box = new RichTextBox();
            //关闭边框
            box.BorderThickness = new Thickness(0);
            //FlowDocument 加到 RichTexBox子结点上
            box.Document = flowDocument;
            box.Background = Brushes.Transparent;
            box.Margin = new Thickness(0, 0, 0, _config.LinePadding);
            box.IsReadOnly = true;
            //添加到父节点上
            _mainPanel.Children.Add(box);

            //保存到数据结构中
            _model.AddRichTexBox(box);


            box.Loaded += (s, e) =>
            {
                //组件加载完成后播放动画
                _anim.From = box.ActualHeight + _config.LinePadding;
                _anim.To = 0;
                _anim.Duration = TimeSpan.FromMilliseconds(_config.RollAnimTime);
                rootTrans.BeginAnimation(TranslateTransform.YProperty, _anim);
            };
        }
        /// <summary>
        /// 显示收到的礼物
        /// </summary>
        /// <param name="name"></param>
        private void ShowReceiveGiftInfo(ReceiveSendGift msg)
        {
            TtsPlayer.Instance.AddPlayInstance($"感谢{msg.UserName}送的{msg.GiftCount}个{msg.GiftName}");
            //用户名文本
            Run nameRun = new Run(msg.UserName + "\u00A0");
            nameRun.Foreground = Brushes.Blue;
            //消息文本
            Run msgRun = new Run("送了");
            msgRun.Foreground = Brushes.Black;

            //礼物数量
            Run giftNumRun = new Run(msg.GiftCount.ToString() + "个");
            giftNumRun.Foreground = Brushes.Black;

            //礼物名字
            Run giftNameRun = new Run(msg.GiftName);
            giftNameRun.Foreground = Brushes.LightGoldenrodYellow;

            //段落类
            Paragraph para = new Paragraph();
            para.LineHeight = _config.FontSize;
            para.Background = Brushes.Transparent;
            //文本添加到段落类子节点上
            para.Inlines.Add(nameRun);
            para.Inlines.Add(msgRun);
            para.Inlines.Add(giftNumRun);
            para.Inlines.Add(giftNameRun);

            //FlowDocument类
            FlowDocument flowDocument = new FlowDocument();
            flowDocument.FontSize = _config.FontSize;
            flowDocument.Background = Brushes.Transparent;
            //段落类 加到FlowDocument类子节点上
            flowDocument.Blocks.Add(para);

            RichTextBox box = new RichTextBox();
            //关闭边框
            box.BorderThickness = new Thickness(0);
            //FlowDocument 加到 RichTexBox子结点上
            box.Document = flowDocument;
            box.Background = Brushes.Transparent;
            box.Margin = new Thickness(0, 0, 0, _config.LinePadding);
            box.IsReadOnly = true;
            //添加到父节点上
            _mainPanel.Children.Add(box);

            //保存到数据结构中
            _model.AddRichTexBox(box);

            box.Loaded += (s, e) =>
            {
                //组件加载完成后播放动画
                _anim.From = box.ActualHeight + _config.LinePadding;
                _anim.To = 0;
                _anim.Duration = TimeSpan.FromMilliseconds(_config.RollAnimTime);
                rootTrans.BeginAnimation(TranslateTransform.YProperty, _anim);
            };
        }
        #endregion

        private void OnMouseLeftBtnDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
