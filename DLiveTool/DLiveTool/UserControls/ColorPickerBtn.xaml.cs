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
using System.Numerics;

namespace DLiveTool
{
    /// <summary>
    /// ColorControlBtn.xaml 的交互逻辑
    /// </summary>
    public partial class ColorControlBtn : UserControl
    {
        public Action<Color> OnColorChanged;
        /// <summary>
        /// 当前选择的颜色
        /// </summary>
        private Color _curColor;
        /// <summary>
        /// 组件状态
        /// </summary>
        private ControlState _state = ControlState.Normal;

        public ColorControlBtn()
        {
            InitializeComponent();

            _colorPickerPanel.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// 设置当前选择的颜色
        /// </summary>
        /// <param name="color"></param>
        public void SetColor(Color color)
        {
            _curColor = color;
            OnColorChanged?.Invoke(_curColor);
        }
        public void OpenColorPickerPanel(Thickness pos)
        {
            if (_state == ControlState.Normal)
            {
                //_changColorBtn.IsEnabled = false;
                _colorPickerPanel.Visibility = Visibility.Visible;
                _colorPickerPanel.Margin = pos;
                _state = ControlState.PickColor;
                //CaptureMouse();
            }
        }
        public void CloseColorPickerPanel()
        {
            if(_state == ControlState.PickColor)
            {
                //_changColorBtn.IsEnabled = true;
                _colorPickerPanel.Visibility = Visibility.Collapsed;
                _state = ControlState.Normal;
                //ReleaseMouseCapture();
            }
        }
        #region 事件函数
        private void OnColorBtnClicked(object sender, RoutedEventArgs e)
        {
            if(_state == ControlState.Normal)
            {
                Thickness pos = new Thickness(_changColorBtn.ActualWidth, 0, 0, 0);
                OpenColorPickerPanel(pos);
            }
            else if(_state == ControlState.PickColor)
            {
                CloseColorPickerPanel();
            }
            
        }
        #endregion

        public enum ControlState
        {
            /// <summary>
            /// 默认状态
            /// </summary>
            Normal,
            /// <summary>
            /// 正在选择颜色
            /// </summary>
            PickColor,
        }

        bool _isChangeColorRing = false;
        private void OnColorRingMouseDown(object sender, MouseButtonEventArgs e)
        {
            _isChangeColorRing = true;
            (sender as UIElement).CaptureMouse();
        }

        private void OnColorRingMouseUp(object sender, MouseButtonEventArgs e)
        {
            _isChangeColorRing = false;
            (sender as UIElement).ReleaseMouseCapture();
        }

        private void OnColorRingMouseMove(object sender, MouseEventArgs e)
        {
            if (_isChangeColorRing)
            {
                Point point = Mouse.GetPosition(sender as IInputElement);
                var value = Math.Clamp(point.Y, 0, 110);
                _colorRingSelectBar.SetValue(Canvas.TopProperty, value);

                _testPosY.Text = value.ToString();

                double colorRingValue = value * 0.054545;//18.33 == (110 / 6); 0.54545 == 1 / 18.33

                Color mainColor = GetColor(colorRingValue);
                _mainColorRect.Fill = new SolidColorBrush(mainColor);
                _textR.Text = mainColor.R.ToString();
                _textG.Text = mainColor.G.ToString();
                _textB.Text = mainColor.B.ToString();
                _textA.Text = mainColor.A.ToString();
            }
        }

        private Color GetColor(double colorRingValue)
        {
            int colorIndex = (int)(colorRingValue);
            double lerpValue = colorRingValue - colorIndex;

            _testValue.Text = colorRingValue.ToString();
            _testIndex.Text = colorIndex.ToString();
            _testLerpValue.Text = lerpValue.ToString();

            if (colorIndex == 0)
            {
                return Color.FromRgb(255, 0, Lerp(0, 255, lerpValue));
            }
            else if(colorIndex == 1)
            {
                return Color.FromRgb(Lerp(255, 0, lerpValue), 0, 255);
            }
            else if(colorIndex == 2)
            {
                return Color.FromRgb(0, Lerp(0, 255, lerpValue), 255);
            }
            else if(colorIndex == 3)
            {
                return Color.FromRgb(0, 255, Lerp(255, 0, lerpValue));
            }
            else if(colorIndex == 4)
            {
                return Color.FromRgb(Lerp(0, 255, lerpValue), 255, 0);
            }
            else if(colorIndex == 5)
            {
                return Color.FromRgb(255, Lerp(255, 0, lerpValue), 0);
            }
            else
            {
                return Color.FromRgb(255, 0, 0);
            }
        }
        byte Lerp(byte start, byte end, double lerpValue)
        {
            return (byte)(start + lerpValue * (end - start));
        }
        Vector3 HsvToRgb(Vector3 hsv)
        {
            Vector3 rgb = new Vector3();
            return rgb;
        }
    }
}
