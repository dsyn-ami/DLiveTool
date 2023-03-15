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
        StackPanel _patentPanel;
        int _maxItemCount = 20;
        List<RichTextBox> _items = new List<RichTextBox>();


        public DanmakuWindowDataModel(StackPanel parentPanel)
        {
            _patentPanel = parentPanel;
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
