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
        DanmakuWindowConfig _config = ConfigDataMgr.Instance.Data.DanmakuWindowConfig;

        public DanmakuWindowSettingPage()
        {
            InitializeComponent();

            Refresh();

            _backgroundColor.OnColorChanged += OnBackgroundColorChanged;
            _backgroundColor.OnPanelClosed += OnBackgroundColorConfirm;
        }

        private void Refresh()
        {
            var data = ConfigDataMgr.Instance.Data;
            _showEnterCheckBox.IsChecked = data.DanmakuWindowConfig.IsShowEnterInfo;

            foreach (string avoidKey in data.DanmakuWindowConfig.AvoidNameKeyWordList)
            {
                _avoidKeyListBox.Items.Add(avoidKey);
            }
            _alwaysTopCheckBox.IsChecked = _config.IsAlwaysTop;
            _fontSizeInput.Text = _config.FontSize.ToString();
            _linePaddingInput.Text = _config.LinePadding.ToString();
            _maxItemCountInput.Text = _config.MaxItemCount.ToString();
            _rollAnimTimeInput.Text = _config.RollAnimTime.ToString();
            _itemAliveTimeInput.Text = _config.ItemAliveTime.ToString();

            _backgroundColor.SetColor(_config.BGColorA, _config.BGColorR, _config.BGColorG, _config.BGColorB);
        }

        #region 控件事件
        DanmakuWindow _curDanmakuWindow;
        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            if(_curDanmakuWindow == null)
            {
                _curDanmakuWindow = new DanmakuWindow();
                //_curDanmakuWindow.Owner = Application.Current.MainWindow;
                _curDanmakuWindow.Topmost = _config.IsAlwaysTop;
                _curDanmakuWindow.Show();
                _openBtn.Content = "关闭弹幕窗口";
            }
            else
            {
                _curDanmakuWindow.Close();
                _curDanmakuWindow = null;
                _openBtn.Content = "打开弹幕窗口";
            }
        }
        private void OnShowEnterChecked(object sender, RoutedEventArgs e)
        {
            ConfigDataMgr.Instance.Data.DanmakuWindowConfig.IsShowEnterInfo = true;
            ConfigDataMgr.Instance.SaveData();
        }

        private void OnShowEnterUnChecked(object sender, RoutedEventArgs e)
        {
            _config.IsShowEnterInfo = false;
            ConfigDataMgr.Instance.SaveData();
        }
        private void OnAddAvoidKeyBtnClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(_avoidKeyInput.Text))
            {
                string addKey = _avoidKeyInput.Text;
                var curList = _config.AvoidNameKeyWordList;
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
            var curList = _config.AvoidNameKeyWordList;
            object selectedItem = _avoidKeyListBox.SelectedItem;
            if (selectedItem != null)
            {
                curList.Remove(selectedItem.ToString());
                _avoidKeyListBox.Items.Remove(selectedItem);
                ConfigDataMgr.Instance.SaveData();
            }
        }
        private void OnMaxItemCountChanged(object sender, RoutedEventArgs e)
        {
            string input = _maxItemCountInput.Text;
            if (int.TryParse(input, out int value))
            {
                _maxItemCountInput.Text = value.ToString();
                _config.MaxItemCount = value;
                ConfigDataMgr.Instance.SaveData();
            }
            else
            {
                _maxItemCountInput.Text = _config.MaxItemCount.ToString();
            }
        }

        private void OnRollAnimTimeChanged(object sender, RoutedEventArgs e)
        {
            string input = _rollAnimTimeInput.Text;
            if (int.TryParse(input, out int value))
            {
                _rollAnimTimeInput.Text = value.ToString();
                _config.RollAnimTime = value;
                ConfigDataMgr.Instance.SaveData();
            }
            else
            {
                _rollAnimTimeInput.Text = _config.RollAnimTime.ToString();
            }
        }

        private void OnItemAliveTimeChanged(object sender, RoutedEventArgs e)
        {
            string input = _itemAliveTimeInput.Text;
            if (int.TryParse(input, out int value))
            {
                _itemAliveTimeInput.Text = value.ToString();
                _config.ItemAliveTime = value;
                ConfigDataMgr.Instance.SaveData();
            }
            else
            {
                _itemAliveTimeInput.Text = _config.ItemAliveTime.ToString();
            }
        }

        private void OnFontSizeChanged(object sender, RoutedEventArgs e)
        {
            string input = _fontSizeInput.Text;
            if (int.TryParse(input, out int value))
            {
                _fontSizeInput.Text = value.ToString();
                _config.FontSize = value;
                ConfigDataMgr.Instance.SaveData();
            }
            else
            {
                _fontSizeInput.Text = _config.FontSize.ToString();
            }
        }

        private void OnLinePaddingChanged(object sender, RoutedEventArgs e)
        {
            string input = _linePaddingInput.Text;
            if (int.TryParse(input, out int value))
            {
                _linePaddingInput.Text = value.ToString();
                _config.LinePadding = value;
                ConfigDataMgr.Instance.SaveData();
            }
            else
            {
                _linePaddingInput.Text = _config.LinePadding.ToString();
            }
        }
        private void OnBackgroundColorConfirm(Color obj)
        {
            ConfigDataMgr.Instance.SaveData();
        }

        private void OnBackgroundColorChanged(Color obj)
        {
            _config.BGColorA = obj.A;
            _config.BGColorR = obj.R;
            _config.BGColorG = obj.G;
            _config.BGColorB = obj.B;
            if(_curDanmakuWindow != null)
            {
                _curDanmakuWindow.SetBackgroundColor(obj);
            }
        }
        #endregion

        private void OnAlwaysTopChecked(object sender, RoutedEventArgs e)
        {
            _config.IsAlwaysTop = true;
            ConfigDataMgr.Instance.SaveData();
            if (_curDanmakuWindow != null)
            {
                _curDanmakuWindow.Topmost = true;
            }
        }

        private void OnAlwaysTopUnChecked(object sender, RoutedEventArgs e)
        {
            _config.IsAlwaysTop = false;
            ConfigDataMgr.Instance.SaveData();
            if (_curDanmakuWindow != null)
            {
                _curDanmakuWindow.Topmost = false;
            }
        }
    }
}