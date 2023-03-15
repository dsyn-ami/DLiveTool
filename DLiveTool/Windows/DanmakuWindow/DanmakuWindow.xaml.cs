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

            _biliWS = new BiliWebSocket();
            _biliWS.ConnectAsync("21263282");
            _biliWS.OnReceiveDanmaku += ShowDanmakuMsg;
        }

        private void ShowDanmakuMsg(ReceiveDanmakuMsg msgData)
        {
            string name = msgData.UserName;
            string msg = msgData.Message;
            ShowDanmakuMsgText(name, msg);
        }

        private void ShowDanmakuMsgText(string name, string msg)
        {
            //用户名文本
            Run nameRun = new Run(name + " ");
            nameRun.Foreground = new SolidColorBrush(Colors.Red);
            //消息文本
            Run msgRun = new Run(msg);
            msgRun.Foreground = new SolidColorBrush(Colors.Black);

            //段落类
            Paragraph para = new Paragraph();
            para.LineHeight = 30;
            para.Background = new SolidColorBrush(Color.FromArgb(0, 1, 1, 1));
            para.Foreground = new SolidColorBrush(Color.FromRgb(1, 0, 0));
            //文本添加到段落类子节点上
            para.Inlines.Add(nameRun);
            para.Inlines.Add(msgRun);

            //FlowDocument类
            FlowDocument flowDocument = new FlowDocument();
            flowDocument.FontSize = 25;
            flowDocument.Background = new SolidColorBrush(Color.FromArgb(0, 1, 1, 1));
            //段落类 加到FlowDocument类子节点上
            flowDocument.Blocks.Add(para);
            
            RichTextBox box = new RichTextBox();
            //FlowDocument 加到 RichTexBox子结点上
            box.Document = flowDocument;
            box.Background = new SolidColorBrush(Color.FromArgb(0, 1, 1, 1));
            //添加到父节点上
            _mainPanel.Children.Add(box);

            //保存到数据结构中
            _model.AddRichTexBox(box);
        }
    }
}
