using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace DLiveTool
{
    /// <summary>
    /// DAISettingPage.xaml 的交互逻辑
    /// </summary>
    public partial class DAISettingPage : Page
    {
        DAIConfig _config = ConfigDataMgr.Instance.Data.DAIConfig;
        public DAISettingPage()
        {
            InitializeComponent();
            Refresh();
        }

        void Refresh()
        {
            _isAllowDanmakuResponse.IsChecked = _config.IsAllowDanmakuResponse;
            _cookieInput.Password = _config.Cookie;
        }

        #region 控件事件
        private void OnCookieChanged(object sender, RoutedEventArgs e)
        {
            _config.Cookie = _cookieInput.Password;
            _config.InCookie_csrf = FindCSRFInCookie(_config.Cookie);
            _config.InCookie_UserId = FindUserIdInCookie(_config.Cookie);
            ConfigDataMgr.Instance.SaveData();
        }
        private void OnPageLoaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
        private void OnSendBtnClick(object sender, RoutedEventArgs e)
        {
            string msg = _danmakuInput.Text;
            if (!string.IsNullOrEmpty(msg))
            {
                _danmakuInput.Text = string.Empty;
                BiliRequester.SendDanmakuAsync(msg);
            }
        }

        private void OnIsAllowDanmakuResponseChecked(object sender, RoutedEventArgs e)
        {
            _config.IsAllowDanmakuResponse = true;
            ConfigDataMgr.Instance.SaveData();
        }

        private void OnIsAllowDanmakuResponseUnChecked(object sender, RoutedEventArgs e)
        {
            _config.IsAllowDanmakuResponse = false;
            ConfigDataMgr.Instance.SaveData();
        }
        #endregion

        #region 其他函数

        /// <summary>
        /// 在cookie中查找csrf值
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        private string FindCSRFInCookie(string cookie)
        {
            return new Regex("(?<=(bili_jct=))[.\\s\\S]*?(?=(;))", RegexOptions.Multiline | RegexOptions.Singleline).Match(cookie).Value;
        }
        /// <summary>
        /// 在cookie中查找csrf值
        /// </summary>
        /// <param name="cookie"></param>
        /// <returns></returns>
        private string FindUserIdInCookie(string cookie)
        {
            return new Regex("(?<=(DedeUserID=))[.\\s\\S]*?(?=(;))", RegexOptions.Multiline | RegexOptions.Singleline).Match(cookie).Value;
        }
        #endregion
    }
}
