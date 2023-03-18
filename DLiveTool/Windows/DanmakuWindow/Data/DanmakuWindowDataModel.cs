using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DLiveTool
{
    public class DanmakuWindowDataModel
    {
        public DanmakuWindowDataModel(StackPanel parentPanel)
        {
            _patentPanel = parentPanel;
        }

        StackPanel _patentPanel;
        int _maxItemCount = 20;
        List<RichTextBox> _items = new List<RichTextBox>();

        private int _fontSize = 18;
        /// <summary>
        /// 字体大小
        /// </summary>
        public int FontSize
        {
            get { return _fontSize; }
            set { _fontSize = value; }
        }

        private int _linePadding = 10;
        /// <summary>
        /// 行距
        /// </summary>
        public int LinePadding
        {
            get { return _linePadding; }
            set { _linePadding = value; }
        }

        public void AddRichTexBox(RichTextBox box)
        {
            _items.Add(box);
            if(_items.Count > _maxItemCount)
            {
                RichTextBox removeBox = _items[0];
                _patentPanel.Children.Remove(removeBox);
                _items.RemoveAt(0);
            }
        }
    }
}
