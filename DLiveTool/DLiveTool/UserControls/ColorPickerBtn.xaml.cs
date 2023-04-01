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
        public RoutedEventHandler OnEvent;
        public Action<Color> OnColorChanged;
        public Action<Color> OnPanelClosed;
        /// <summary>
        /// 当前选择的颜色
        /// </summary>
        private Color _curColor;
        /// <summary>
        /// 当前颜色的不透明度
        /// </summary>
        byte _curA = 255;
        /// <summary>
        /// 当前颜色R通道值
        /// </summary>
        byte _curR = 255;
        /// <summary>
        /// 当前颜色G通道值
        /// </summary>
        byte _curG = 255;
        /// <summary>
        /// 当前颜色B通道值
        /// </summary>
        byte _curB = 255;
        /// <summary>
        /// 当前选择颜色的色相(0-1)
        /// </summary>
        float _curH;
        /// <summary>
        /// 当前选择的颜色的饱和度(0-1)
        /// </summary>
        float _curS;
        /// <summary>
        /// 当前选择的颜色的亮度(0-1)
        /// </summary>
        float _curV;
        /// <summary>
        /// 组件状态
        /// </summary>
        private ControlState _state = ControlState.Normal;

        public ColorControlBtn()
        {
            InitializeComponent();

            _colorPickerPanel.Visibility = Visibility.Collapsed;
        }

        #region 颜色设置相关
        /// <summary>
        /// 通过rgb设置颜色,会更新选色滑块的位置
        /// rgb 转 hsv 时, 有些hsv值永远不会出现, 如:
        /// hsv(0.5, 0, 0) => rgb(0, 0, 0) => hsv(0, 0, 0)
        /// 导致色相丢失, 所以仅在直接修改RGB值, 或初始化使用,
        /// </summary>
        /// <param name="a"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        public void SetColor(byte a, byte r, byte g, byte b)
        {
            //更新数据
            Color newColor = Color.FromArgb(a, r, g, b);
            _curColor = newColor;
            _curA = a;
            _curR = r;
            _curG = g;
            _curB = b;
            Vector3 hsv = RgbToHsv(r, g, b);
            _curH = hsv.X;
            _curS = hsv.Y;
            _curV = hsv.Z;
            //更新UI
            double pickX = _curS * 110;
            double pickY = (1 - _curV) * 110;
            _pickCircle.SetValue(Canvas.LeftProperty, pickX);
            _pickCircle.SetValue(Canvas.TopProperty, pickY);

            double ringY = _curH * 110;
            _colorRingSelectBar.SetValue(Canvas.TopProperty, ringY);

            double alphaX = _curA * 0.43137; // _curA / 255 * 110;
            _colorAlphaSelectBar.SetValue(Canvas.LeftProperty, alphaX);

            Color mainColor = GetMainColor(_curH * 6);
            _mainColorRect.Fill = new SolidColorBrush(mainColor);
            _overviewColor.Fill = new SolidColorBrush(_curColor);

            _textA.Text = _curA.ToString();
            _textR.Text = _curR.ToString();
            _textG.Text = _curG.ToString();
            _textB.Text = _curB.ToString();
            _textColor.Text = _curColor.ToString();

            var brush = Resources["ColorToTransParentBrush"] as LinearGradientBrush;
            brush.GradientStops[1].Color = Color.FromRgb(_curR, _curG, _curB);

            _changColorBtn.Background = new SolidColorBrush(_curColor);
            //触发回调
            OnColorChanged?.Invoke(_curColor);
        }
        /// <summary>
        /// 通过hsv设置颜色,不会更新 选色滑块的位置,
        /// 通过选色滑块选择颜色时,调用这个来更新颜色
        /// </summary>
        /// <param name="a"></param>
        /// <param name="h"></param>
        /// <param name="s"></param>
        /// <param name="v"></param>
        private void SetColor(byte a, float h, float s, float v)
        {
            byte[] rgb = HsvToRgb(h, s, v);
            _curA = a;
            _curR = rgb[0];
            _curG = rgb[1];
            _curB = rgb[2];
            _curColor = Color.FromArgb(_curA, _curR, _curG, _curB);
            //更新UI
            _overviewColor.Fill = new SolidColorBrush(_curColor);

            _textA.Text = _curA.ToString();
            _textR.Text = _curR.ToString();
            _textG.Text = _curG.ToString();
            _textB.Text = _curB.ToString();
            _textColor.Text = _curColor.ToString();

            var brush = Resources["ColorToTransParentBrush"] as LinearGradientBrush;
            brush.GradientStops[1].Color = Color.FromRgb(_curR, _curG, _curB);

            _changColorBtn.Background = new SolidColorBrush(_curColor);
            //触发回调
            OnColorChanged?.Invoke(_curColor);
        }
        #endregion


        public void OpenColorPickerPanel(Thickness pos)
        {
            if (_state == ControlState.Normal)
            {
                _colorPickerPanel.Visibility = Visibility.Visible;
                _colorPickerPanel.Margin = pos;
                _state = ControlState.PickColor;
            }
        }
        public void CloseColorPickerPanel()
        {
            if(_state == ControlState.PickColor)
            {
                _colorPickerPanel.Visibility = Visibility.Collapsed;
                _state = ControlState.Normal;
                OnPanelClosed?.Invoke(_curColor);
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

        #region 色环滑块操作事件
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
                var colorRingValue = value * 0.054545; // value / 110 * 6;

                _colorRingSelectBar.SetValue(Canvas.TopProperty, value);
                _mainColorRect.Fill = new SolidColorBrush(GetMainColor(colorRingValue));

                _curH = (float)(value / 110);

                SetColor(_curA, _curH, _curS, _curV);
            }
        }
        #endregion

        #region 颜色选择操作事件
        bool _isPickingColor = false;
        private void OnPickAreaMouseDown(object sender, MouseButtonEventArgs e)
        {
            _isPickingColor = true;
            (sender as IInputElement).CaptureMouse();
        }

        private void OnPickAreaMouseUp(object sender, MouseButtonEventArgs e)
        {
            _isPickingColor = false;
            (sender as IInputElement).ReleaseMouseCapture();
        }

        private void OnPickAreaMouseMove(object sender, MouseEventArgs e)
        {
            if (_isPickingColor)
            {
                Point point = Mouse.GetPosition(sender as IInputElement);
                double x = Math.Clamp(point.X, 0, 110);
                double y = Math.Clamp(point.Y, 0, 110);

                _pickCircle.SetValue(Canvas.LeftProperty, x);
                _pickCircle.SetValue(Canvas.TopProperty, y);

                _curS = (float)(x / 110);
                _curV = 1 - (float)(y / 110);
                SetColor(_curA, _curH, _curS, _curV);
            }
        }
        #endregion

        #region 透明度滑块操作事件
        bool _isChangeAlphaArea;
        private void OnAlphaAreaMouseDown(object sender, MouseButtonEventArgs e)
        {
            _isChangeAlphaArea = true;
            (sender as UIElement).CaptureMouse();

        }

        private void OnAlphaAreaMouseUp(object sender, MouseButtonEventArgs e)
        {
            _isChangeAlphaArea = false;
            (sender as UIElement).ReleaseMouseCapture();
        }

        private void OnAlphaAreaMouseMove(object sender, MouseEventArgs e)
        {
            if (_isChangeAlphaArea)
            {
                Point point = Mouse.GetPosition(sender as IInputElement);
                var value = Math.Clamp(point.X, 0, 110);
                
                _colorAlphaSelectBar.SetValue(Canvas.LeftProperty, value);
                _curA = (byte)(value * 2.32); //value / 110 * 255;
                SetColor(_curA, _curH, _curS, _curV);
            }
        }
        #endregion

        #region 颜色文本框编辑事件
        private void CheckIfEnterDown(object sender, KeyEventArgs e)
        {
            //如果按下Enter键
            if (e.Key == Key.Enter)
            {
                if (true)
                {
                    //焦点切换到其他元素上,使自己失去焦点,从而触发失去焦点事件
                    _focusLabel.Focus();
                }
                else
                {
                    //也可以 直接触发失去焦点事件
                    RoutedEventArgs newEventArgs = new RoutedEventArgs(TextBox.LostFocusEvent);
                    (sender as TextBox).RaiseEvent(newEventArgs);
                }
            }
        }
        private void OnREndEdit(object sender, RoutedEventArgs e)
        {
            string input = _textR.Text;
            if (byte.TryParse(input, out byte value))
            {
                _curR = value;
                //刷新选择的颜色
                SetColor(_curA, _curR, _curG, _curB);
            }
            else
            {
                //重置文本框
                SetColor(_curA, _curH, _curS, _curV);
            }
        }
        private void OnGEndEdit(object sender, RoutedEventArgs e)
        {
            string input = _textG.Text;
            if (byte.TryParse(input, out byte value))
            {
                _curG = value;
                //刷新选择的颜色
                SetColor(_curA, _curR, _curG, _curB);
            }
            else
            {
                //重置文本框
                SetColor(_curA, _curH, _curS, _curV);
            }
        }
        private void OnBEndEdit(object sender, RoutedEventArgs e)
        {
            string input = _textB.Text;
            if (byte.TryParse(input, out byte value))
            {
                _curB = value;
                //刷新选择的颜色
                SetColor(_curA, _curR, _curG, _curB);
            }
            else
            {
                //重置文本框
                SetColor(_curA, _curH, _curS, _curV);
            }
        }
        private void OnAEndEdit(object sender, RoutedEventArgs e)
        {
            string input = _textA.Text;
            if (byte.TryParse(input, out byte value))
            {
                _curA = value;
                //刷新选择的颜色
                SetColor(_curA, _curR, _curG, _curB);
            }
            else
            {
                //重置文本框
                SetColor(_curA, _curH, _curS, _curV);
            }
        }
        private void OnColorEndEdit(object sender, RoutedEventArgs e)
        {
            string input = _textColor.Text;
            Object obj = ColorConverter.ConvertFromString(input);
            if (obj != null && obj is Color)
            {
                Color color = (Color)obj;
                //刷新选择的颜色
                SetColor(color.A, color.R, color.G, color.B);
            }
            else
            {
                //重置文本框
                SetColor(_curA, _curH, _curS, _curV);
            }
        }
        #endregion

        #endregion

        private Color GetMainColor(double colorRingValue)
        {
            int colorIndex = (int)(colorRingValue);
            double lerpValue = colorRingValue - colorIndex;

            if (colorIndex == 5)
            {
                return Color.FromRgb(255, 0, Lerp(255, 0, lerpValue));
            }
            else if(colorIndex == 4)
            {
                return Color.FromRgb(Lerp(0, 255, lerpValue), 0, 255);
            }
            else if(colorIndex == 3)
            {
                return Color.FromRgb(0, Lerp(255, 0, lerpValue), 255);
            }
            else if(colorIndex == 2)
            {
                return Color.FromRgb(0, 255, Lerp(0, 255, lerpValue));
            }
            else if(colorIndex == 1)
            {
                return Color.FromRgb(Lerp(255, 0, lerpValue), 255, 0);
            }
            else if(colorIndex == 0)
            {
                return Color.FromRgb(255, Lerp(0, 255, lerpValue), 0);
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
        double GetMax(double x, double y, double z)
        {
            return Math.Max(Math.Max(x, y), z);
        }
        double GetMin(double x, double y, double z)
        {
            return Math.Min(Math.Min(x, y), z);
        }
        Vector3 RgbToHsv(byte r, byte g, byte b)
        {
            Vector3 hsv = new Vector3();
            //RGB转换到 0-1 之间
            double rNormal = r * 0.00392; // r / 255.0;
            double gNormal = g * 0.00392; // g / 255.0;
            double bNormal = b * 0.00392; // b / 255.0;

            //找到最大值和最小值
            double max = GetMax(rNormal, gNormal, bNormal);
            double min = GetMin(rNormal, gNormal, bNormal);

            //计算最大最小的差值
            double delta = max - min;

            //计算H
            if(delta == 0)
            {
                hsv.X = 0;
            }
            else if(max == rNormal)
            {
                hsv.X = (float)(0.16667 * ((gNormal - bNormal) / delta + 0));
            }
            else if (max == gNormal)
            {
                hsv.X = (float)(0.16667 * ((bNormal - rNormal) / delta + 2));
            }
            else if (max == bNormal)
            {
                hsv.X = (float)(0.16667 * ((rNormal - gNormal) / delta + 4));
            }
            else
            {
                MessageBox.Show("failed");
            }
            if(hsv.X < 0)
            {
                hsv.X += 1;
            }
            //计算S
            if(max == 0)
            {
                hsv.Y = 0;
            }
            else
            {
                hsv.Y = (float)(delta / max);
            }
            //计算V
            hsv.Z = (float)max;

            return hsv;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hsv">hsv取值都是 0-1 </param>
        /// <returns>0-255</returns>
        byte[] HsvToRgb(float h, float s, float v)
        {
            Vector3 rgb = new Vector3();

            float colorRingValue = h * 6;
            int colorIndex = (int)colorRingValue;
            float lerpValue = colorRingValue - colorIndex;

            float p = v * (1 - s);
            float q = v * (1 - (lerpValue * s));
            float t = v * (1 - (1 - lerpValue) * s);

            if(colorIndex == 0)
            {
                rgb.X = v;
                rgb.Y = t;
                rgb.Z = p;
            }
            else if(colorIndex == 1)
            {
                rgb.X = q;
                rgb.Y = v;
                rgb.Z = p;
            }
            else if (colorIndex == 2)
            {
                rgb.X = p;
                rgb.Y = v;
                rgb.Z = t;
            }
            else if (colorIndex == 3)
            {
                rgb.X = p;
                rgb.Y = q;
                rgb.Z = v;
            }
            else if (colorIndex == 4)
            {
                rgb.X = t;
                rgb.Y = p;
                rgb.Z = v;
            }
            else if (colorIndex == 5)
            {
                rgb.X = v;
                rgb.Y = p;
                rgb.Z = q;
            }
            else if (colorIndex == 6)
            {
                rgb.X = v;
                rgb.Y = t;
                rgb.Z = p;
            }
            byte[] result = new byte[3];
            result[0] = (byte)(rgb.X * 255);
            result[1] = (byte)(rgb.Y * 255);
            result[2] = (byte)(rgb.Z * 255);
            return result;
        }

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
    }
}
