using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DLiveTool.Windows
{
    public partial class MainWindow : Form
    {
        BiliWebSocket _bws = new BiliWebSocket();
        DanmakuWindow _danmakuWindow = new DanmakuWindow();
        public MainWindow()
        {
            InitializeComponent();
            //_bws.ConnectAsync("24466439");

            //设置窗体透明
            //this.BackColor = Color.Green;
            //this.TransparencyKey = Color.Green;

            _danmakuWindow.Show();
        }
    }
}
