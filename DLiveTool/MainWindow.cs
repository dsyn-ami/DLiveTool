using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DLiveTool
{
    public partial class MainWindow : Form
    {
        BiliWebSocket _bws = new BiliWebSocket();
        public MainWindow()
        {
            InitializeComponent();
            _bws.ConnectAsync("213");
        }
    }
}
