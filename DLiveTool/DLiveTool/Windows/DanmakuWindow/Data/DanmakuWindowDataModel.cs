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
            //延迟销毁
            DelayDestroyAsync(_config.ItemAliveTime, box);
            //超过最大值销毁
            if(_items.Count > _config.MaxItemCount)
            {
                RichTextBox removeBox = _items[0];
                _patentPanel.Children.Remove(removeBox);
                _items.RemoveAt(0);
            }
        }

        private async void DelayDestroyAsync(float delayTime, RichTextBox box)
        {
            int delayTimeM = (int)(delayTime * 1000);
            await Task.Delay(delayTimeM);
            if (_items.Contains(box))
            {
                _patentPanel.Children.Remove(box);
                _items.Remove(box);
            }
        }
    }
}
