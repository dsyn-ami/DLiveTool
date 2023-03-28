using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DLiveTool
{
    /// <summary>
    /// 保存DanmakuWindow的临时数据
    /// </summary>
    [Serializable]
    public class DanmakuWindowDataModel
    {
        StackPanel _patentPanel;
        DanmakuWindowConfig _config;
        /// <summary>
        /// 目前显示的消息列表
        /// </summary>
        List<RichTextBox> _items = new List<RichTextBox>();
        public DanmakuWindowDataModel(StackPanel parentPanel)
        {
            _patentPanel = parentPanel;
            _config = ConfigDataMgr.Instance.Data.DanmakuWindowConfig;
        }
        public void AddRichTexBox(RichTextBox box)
        {
            _items.Add(box);
            if(_items.Count > _config.MaxItemCount)
            {
                RichTextBox removeBox = _items[0];
                _patentPanel.Children.Remove(removeBox);
                _items.RemoveAt(0);
            }
        }
    }
}
