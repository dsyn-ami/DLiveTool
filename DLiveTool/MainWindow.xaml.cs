using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DLiveTool.Data;
using dsyn;
using Path = System.IO.Path;

namespace DLiveTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BiliWebSocket _biliWS = new BiliWebSocket();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            DanmakuWindow danmakuWindow = new DanmakuWindow();
            danmakuWindow.Owner = this;
            danmakuWindow.Show();
        }

        private void OnConnectBtnClick(object sender, RoutedEventArgs e)
        {
            string roomId = _roomIdInput.Text;
            if (string.IsNullOrEmpty(roomId)) return;
            _connectBtn.Content = "连接中";
            try
            {
                _biliWS.ConnectAsync(roomId, () =>
                {
                    _connectBtn.Content = "断开";
                    UpdateImage(_topPic, AnchorData.TopPhoto.Value);
                    UpdateImage(_headPic, AnchorData.UserFace.Value);
                    UpdateText(_userName, AnchorData.UserName.Value);
                });
            }
            catch(Exception ex)
            {
                _connectBtn.Content = "连接";
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 替换指定Image的展示图片
        /// </summary>
        /// <param name="image">指定图片组件</param>
        /// <param name="url">图片url</param>
        private async void UpdateImage(Image image, string url)
        {
            string fileName = url.Split('/').Last();
            string path = Path.Combine(DPath.ImgCachePath, fileName);
            //检查本地文件是否有缓存
            //本地没有缓存，下载，并保存在本地
            if (string.IsNullOrEmpty(DCache.GetImageCache(fileName)))
            {
                HttpWebResponse response = await BiliRequester.HttpGet(url);
                Stream stream = response.GetResponseStream();

                bool isSuccess = await FileWriter.WriteFileAsync(path, stream);
                DCache.AddImageCache(fileName, path);
                MessageBox.Show("下载图片");
            }
            //替换图片
            image.Source = new BitmapImage(new Uri(path));
        }
        /// <summary>
        /// 更新指定文本组件的文本
        /// </summary>
        /// <param name="contentItem"></param>
        /// <param name="value"></param>
        private void UpdateText(ContentControl contentItem, string value)
        {
            contentItem.Content = value;
        }
    }
}
