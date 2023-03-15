using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DLiveTool.Windows
{
    public partial class DanmakuWindow : Form
    {
        private Point m_mousePos;
        private bool m_isMouseDown;

        public DanmakuWindow()
        {
            InitializeComponent();

            //隐藏标题栏
            this.FormBorderStyle = FormBorderStyle.None;
            //窗口移动控制
            pictureBg.MouseDown += OnMouseDown;
            pictureBg.MouseUp += OnMouseUp;
            pictureBg.MouseMove += OnMouseMove;

            //背景图大小设置
            pictureBg.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBg.Parent = this;
            pictureBg.Location = new Point(0, 0);
            //背景图透明设置
            Bitmap img = (Bitmap)pictureBg.Image;
            var grapth = GetNoneTransparentRegion(img, 250);
            this.Region = new Region(grapth);
            this.BackgroundImage = pictureBg.Image;
            this.BackgroundImageLayout = ImageLayout.Zoom;
        }

        /// <summary>
        /// 鼠标按下，开启移动
        /// </summary>
        /// <param name="e"></param>
        protected void OnMouseDown(object sender, MouseEventArgs e)
        {
            m_mousePos = Cursor.Position;
            m_isMouseDown = true;
        }

        /// <summary>
        /// 鼠标抬起，关闭移动
        /// </summary>
        /// <param name="e"></param>
        protected void OnMouseUp(object sender, MouseEventArgs e)
        {
            m_isMouseDown = false;
            this.Focus();
        }

        /// <summary>
        /// 移动窗口
        /// </summary>
        /// <param name="e"></param>
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (m_isMouseDown)
            {
                Point tempPos = Cursor.Position;
                this.Location = new Point(Location.X + (tempPos.X - m_mousePos.X), Location.Y + (tempPos.Y - m_mousePos.Y));
                m_mousePos = Cursor.Position;
            }
        }

        /// 返回指定图片中的非透明区域；
        /// </summary>
        /// <param name="img">位图</param>
        /// <param name="alpha">alpha 小于等于该值的为透明</param>
        /// <returns></returns>
        public GraphicsPath GetNoneTransparentRegion(Bitmap img, byte alpha)
        {
            int height = img.Height;
            int width = img.Width;

            int xStart, xEnd;
            GraphicsPath grpPath = new GraphicsPath();
            for (int y = 0; y < height; y++)
            {
                //逐行扫描；
                for (int x = 0; x < width; x++)
                {
                    //略过连续透明的部分；
                    while (x < width && img.GetPixel(x, y).A <= alpha)
                    {
                        x++;
                    }
                    //不透明部分；
                    xStart = x;
                    while (x < width && img.GetPixel(x, y).A > alpha)
                    {
                        x++;
                    }
                    xEnd = x;
                    if (img.GetPixel(x - 1, y).A > alpha)
                    {
                        grpPath.AddRectangle(new Rectangle(xStart, y, xEnd - xStart, 1));
                    }
                }
            }
            return grpPath;
        }
    }
}
