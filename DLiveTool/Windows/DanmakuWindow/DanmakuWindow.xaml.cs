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
        BiliWebSocket _biliWS;
        DanmakuWindowDataModel _model;
        public DanmakuWindow()
        {
            InitializeComponent();

            _model = new DanmakuWindowDataModel(_mainPanel);

            _anim.Completed += Anim_Completed;

            _biliWS = new BiliWebSocket();
            _biliWS.ConnectAsync("6136246");
            _biliWS.OnReceiveDanmaku += AddDanmakuMsgToQueue;
        }

        private void Anim_Completed(object sender, EventArgs e)
        {
            if (_msgQueue.Count > 0)
            {
                var data = _msgQueue.Dequeue();
                ShowDanmakuMsgText(data.UserName, data.Message);
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
        private void AddDanmakuMsgToQueue(ReceiveDanmakuMsg msgData)
        {
            _msgQueue.Enqueue(msgData);

            if (!_isAniming)
            {
                var data = _msgQueue.Dequeue();
                ShowDanmakuMsgAsync(data);
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
        Queue<ReceiveDanmakuMsg> _msgQueue = new Queue<ReceiveDanmakuMsg>();
        /// <summary>
        /// 展示一条弹幕消息
        /// </summary>
        /// <param name="msgData"></param>
        private async void ShowDanmakuMsgAsync(ReceiveDanmakuMsg msgData)
        {
            if (!_isAniming)
            {
                //动画锁开启
                _isAniming = true;
                var data = msgData;

                if (data.Type == ReceiveDanmakuMsg.DanmakuType.Text)
                {
                    ShowDanmakuMsgText(data.UserName, data.Message);
                }
                else if (data.Type == ReceiveDanmakuMsg.DanmakuType.ImgEmoticon)
                {
                    string fileName = data.Emoticon.ImgUrl.Split("/").Last();
                    string path = System.IO.Path.Combine(DPath.EmoticonCachePath, fileName);

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
        }

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
            para.LineHeight = _model.FontSize;
            para.Background = Brushes.Transparent;
            //文本添加到段落类子节点上
            para.Inlines.Add(nameRun);
            para.Inlines.Add(msgRun);

            //FlowDocument类
            FlowDocument flowDocument = new FlowDocument();
            flowDocument.FontSize = _model.FontSize;
            flowDocument.Background = Brushes.Transparent;
            //段落类 加到FlowDocument类子节点上
            flowDocument.Blocks.Add(para);

            RichTextBox box = new RichTextBox();
            //关闭边框
            box.BorderThickness = new Thickness(0);
            //FlowDocument 加到 RichTexBox子结点上
            box.Document = flowDocument;
            box.Background = Brushes.Transparent;
            box.Margin = new Thickness(0, 0, 0, _model.LinePadding);
            //添加到父节点上
            _mainPanel.Children.Add(box);

            //保存到数据结构中
            _model.AddRichTexBox(box);

            
            box.Loaded += (s, e) =>
            {
                //组件加载完成后播放动画
                _anim.From = box.ActualHeight + _model.LinePadding;
                _anim.To = 0;
                _anim.Duration = TimeSpan.FromMilliseconds(250);
                rootTrans.BeginAnimation(TranslateTransform.YProperty, _anim);
            };
        }
        private void ShowDanmakuMsgEmoticon(string name, string emoticonPath, int height)
        {
            //用户名文本
            Run nameRun = new Run(name + "\u00A0");
            nameRun.Foreground = Brushes.Blue;
            //表情图片
            Image emoticonImg = new Image();
            emoticonImg.Source = new BitmapImage(new Uri(emoticonPath, UriKind.Absolute));
            emoticonImg.Height = _model.FontSize + _model.LinePadding;

            //段落类
            Paragraph para = new Paragraph();
            para.LineHeight = _model.FontSize;
            para.Background = Brushes.Transparent;
            //文本添加到段落类子节点上
            para.Inlines.Add(nameRun);
            para.Inlines.Add(emoticonImg);

            //FlowDocument类
            FlowDocument flowDocument = new FlowDocument();
            
            flowDocument.FontSize = _model.FontSize;
            flowDocument.Background = Brushes.Transparent;
            //段落类 加到FlowDocument类子节点上
            flowDocument.Blocks.Add(para);

            RichTextBox box = new RichTextBox();
            //关闭边框
            box.BorderThickness = new Thickness(0);
            //FlowDocument 加到 RichTexBox子结点上
            box.Document = flowDocument;
            box.Margin = new Thickness(0, 0, 0, _model.LinePadding);
            box.Background = Brushes.Transparent;
            //添加到父节点上
            _mainPanel.Children.Add(box);

            //保存到数据结构中
            _model.AddRichTexBox(box);


            box.Loaded += (s, e) =>
            {
                //组件加载完成后播放动画
                _anim.From = box.ActualHeight + _model.LinePadding;
                _anim.To = 0;
                _anim.Duration = TimeSpan.FromMilliseconds(250);
                rootTrans.BeginAnimation(TranslateTransform.YProperty, _anim);
            };
        }
        #endregion
    }
}
