﻿using System;
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
            _biliWS.ConnectAsync("690");
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
                ShowDanmakuMsg(data);
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
        private void ShowDanmakuMsg(ReceiveDanmakuMsg msgData)
        {
            _msgQueue.Enqueue(msgData);

            if (!_isAniming)
            {
                var data = _msgQueue.Dequeue();
                ShowDanmakuMsgText(data.UserName, data.Message);
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
            para.LineHeight = _model.FontSize + _model.LinePadding;
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
            //添加到父节点上
            _mainPanel.Children.Add(box);

            //保存到数据结构中
            _model.AddRichTexBox(box);

            //动画锁开启
            _isAniming = true;
            box.Loaded += (s, e) =>
            {
                //组件加载完成后播放动画
                _anim.From = box.ActualHeight;
                _anim.To = 0;
                _anim.Duration = TimeSpan.FromMilliseconds(350);
                rootTrans.BeginAnimation(TranslateTransform.YProperty, _anim);
            };
        }
        #endregion
    }
}
