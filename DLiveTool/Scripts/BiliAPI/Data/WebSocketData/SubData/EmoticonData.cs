using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLiveTool.Data
{
    /*
     {
                "bulge_display":0,
                "emoticon_unique":"official_109",
                "height":60,
                "in_player_area":1,
                "is_dynamic":1,
                "url":"http://i0.hdslb.com/bfs/live/7b7a2567ad1520f962ee226df777eaf3ca368fbc.png",
                "width":138
            }
     */
    /// <summary>
    /// 表情图片数据
    /// </summary>
    public class EmoticonData
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public string ImgUrl { get; private set; }

        public EmoticonData(string json)
        {
            JObject jo = JObject.Parse(json);
            Width = (int)jo["width"];
            Height = (int)jo["height"];
            ImgUrl = jo["url"].ToString();
        }
    }
}
