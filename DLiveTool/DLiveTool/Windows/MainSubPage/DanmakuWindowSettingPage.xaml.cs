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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DLiveTool
{
    /// <summary>
    /// DanmakuWindowSettingPage.xaml 的交互逻辑
    /// </summary>
    public partial class DanmakuWindowSettingPage : Page
    {
        public DanmakuWindowSettingPage()
        {
            InitializeComponent();

            Refresh();
        }

        private void Refresh()
        {
            var data = ConfigDataMgr.Instance.Data;
            _showEnterCheckBox.IsChecked = data.DanmakuWindowConfig.IsShowEnterInfo;

            foreach (string avoidKey in data.DanmakuWindowConfig.AvoidNameKeyWordList)
            {
                _avoidKeyListBox.Items.Add(avoidKey);
            }
        }

        #region 控件事件
        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            DanmakuWindow danmakuWindow = new DanmakuWindow();
            danmakuWindow.Owner = Application.Current.MainWindow;
            danmakuWindow.Show();
        }
        private void OnShowEnterChecked(object sender, RoutedEventArgs e)
        {
            ConfigDataMgr.Instance.Data.DanmakuWindowConfig.IsShowEnterInfo = true;
            ConfigDataMgr.Instance.SaveData();
        }

        private void OnShowEnterUnChecked(object sender, RoutedEventArgs e)
        {
            ConfigDataMgr.Instance.Data.DanmakuWindowConfig.IsShowEnterInfo = false;
            ConfigDataMgr.Instance.SaveData();
        }
        private void OnAddAvoidKeyBtnClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_avoidKeyInput.Text))
            {
                string addKey = _avoidKeyInput.Text;
                var curList = ConfigDataMgr.Instance.Data.DanmakuWindowConfig.AvoidNameKeyWordList;
                if (!curList.Contains(addKey))
                {
                    curList.Add(addKey);
                    _avoidKeyInput.Text = String.Empty;
                    _avoidKeyListBox.Items.Add(addKey);
                    ConfigDataMgr.Instance.SaveData();
                }
            }
        }

        private void OnRemoveAvoidKeyBtnClick(object sender, RoutedEventArgs e)
        {
            var curList = ConfigDataMgr.Instance.Data.DanmakuWindowConfig.AvoidNameKeyWordList;
            object selectedItem = _avoidKeyListBox.SelectedItem;
            if (selectedItem != null)
            {
                curList.Remove(selectedItem.ToString());
                _avoidKeyListBox.Items.Remove(selectedItem);
                ConfigDataMgr.Instance.SaveData();
            }
        }
        #endregion
    }
}