﻿using System;
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
        DanmakuWindowSettingPage _danmakuSettingPage;
        DAISettingPage _daiSettingPage;

        DAISystem _daiSystem;

        #region 初始化
        public MainWindow()
        {
            InitializeComponent();
            //初始化缓存
            DCache.Init();
            //刷新窗口
            Refresh();
            //DAI初始化
            _daiSystem = new DAISystem();
            _daiSystem.Init();
        }

        private void Refresh()
        {
            var data = ConfigDataMgr.Instance.Data;
            _roomIdInput.Text = data.RoomId;
            //初始化子页面
            _danmakuSettingPage = new DanmakuWindowSettingPage();
            //设置子Page
            _mainContent.Content = _danmakuSettingPage;
            //刷新侧边栏toggle
            SetSelectedToggleColor(_toggleBoderDanmaku);
        }
        #endregion

        #region 加载图片

        /// <summary>
        /// 替换指定Image的展示图片
        /// </summary>
        /// <param name="image">指定图片源</param>
        /// <param name="url">图片url</param>
        private async void UpdateImageAsync(Image image, string url)
        {
            image.Source = await LoadImageAsync(url);
        }
        /// <summary>
        /// 替换指定ImageBrush的展示图片
        /// </summary>
        /// <param name="imgBrush">指定图片源</param>
        /// <param name="url">图片url</param>
        private async void UpdateImageAsync(ImageBrush imgBrush, string url)
        {
            imgBrush.ImageSource = await LoadImageAsync(url);
        }
        /// <summary>
        /// 加载图片, 有缓存的话会从缓存加载,否则从url下载
        /// </summary>
        /// <param name="url">图片url</param>
        /// <returns></returns>
        private async Task<BitmapImage> LoadImageAsync(string url)
        {
            if (string.IsNullOrEmpty(url)) return null;
            string fileName = url.Split('/').Last();
            string path = Path.Combine(DPath.ImgCachePath, fileName);
            //检查本地文件是否有缓存
            //本地没有缓存，下载，并保存在本地
            if (string.IsNullOrEmpty(DCache.GetImageCache(fileName)))
            {
                HttpWebResponse response = await BiliRequester.HttpGetAsync(url);
                Stream stream = response.GetResponseStream();

                bool isSuccess = await FileWriter.WriteFileAsync(path, stream);
                DCache.AddImageCache(fileName, path);
            }
            return new BitmapImage(new Uri(path));
        }

        #endregion

        #region 控件事件
        private void OnConnectBtnClick(object sender, RoutedEventArgs e)
        {
            string roomId = _roomIdInput.Text;
            if (string.IsNullOrEmpty(roomId)) return;


            if (_connectBtn.Content.Equals("断开"))
            {
                _roomIdInput.IsEnabled = true;
                _connectBtn.Content = "连接";
                _connectBtn.IsEnabled = true;

                _topPic.Source = null;
                _headPicBrush.ImageSource = null;
                UpdateText(_userName, "用户名");

                DConnection.Disconnect();
            }
            else if (_connectBtn.Content.Equals("连接"))
            {
                _roomIdInput.IsEnabled = false;
                _connectBtn.Content = "连接中";
                _connectBtn.IsEnabled = false;
                try
                {
                    DConnection.Connect(roomId, (code, msg) =>
                    {
                        if (code != 0)
                        {
                            MessageBox.Show(msg);
                            _connectBtn.Content = "连接";
                            _connectBtn.IsEnabled = true;
                            _roomIdInput.IsEnabled = true;
                            return;
                        }

                        _connectBtn.Content = "断开";
                        _connectBtn.IsEnabled = true;
                        UpdateImageAsync(_topPic, AnchorData.TopPhoto.Value);
                        UpdateImageAsync(_headPicBrush, AnchorData.UserFace.Value);
                        UpdateText(_userName, AnchorData.UserName.Value);

                        //更新房间号
                        ConfigDataMgr.Instance.Data.RoomId = roomId;
                        ConfigDataMgr.Instance.SaveData();
                    });
                }
                catch (Exception ex)
                {
                    _connectBtn.Content = "连接";
                    _roomIdInput.IsEnabled = true;
                    _connectBtn.IsEnabled = true;
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void OnMainWindowClosed(object sender, EventArgs e)
        {
            DConnection.BiliWS.DisConnect();
            Environment.Exit(0);
        }

        private void OnDanmakuToggleBtnClick(object sender, RoutedEventArgs e)
        {
            if(_mainContent.Content != _danmakuSettingPage)
            {
                //刷新 Toggle
                SetSelectedToggleColor(_toggleBoderDanmaku);
                //刷新子页面
                if(_danmakuSettingPage == null) _danmakuSettingPage = new DanmakuWindowSettingPage();
                _mainContent.Content = _danmakuSettingPage;
            }
        }

        private void OnDAIToggleBtnClick(object sender, RoutedEventArgs e)
        {
            if(_mainContent.Content != _daiSettingPage)
            {
                //刷新 Toggle
                SetSelectedToggleColor(_toggleBoderDAI);
                //刷新子页面
                if(_daiSettingPage == null) _daiSettingPage = new DAISettingPage();
                _mainContent.Content = _daiSettingPage;
            }
        }
        #endregion

        #region 其他函数
        private void SetSelectedToggleColor(Border selectedBorder)
        {
            SolidColorBrush selectBrush = Resources["_toggleSelectColor"] as SolidColorBrush;
            SolidColorBrush normalBrush = Resources["_toggleNormalColor"] as SolidColorBrush;

            _toggleBoderDanmaku.Background = normalBrush;
            _toggleBoderDAI.Background = normalBrush;
            _toggleBorderConsole.Background = normalBrush;
            selectedBorder.Background = selectBrush;
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
        private void OnConsoleToggleBtnClick(object sender, RoutedEventArgs e)
        {

        }
        #endregion


    }
}
