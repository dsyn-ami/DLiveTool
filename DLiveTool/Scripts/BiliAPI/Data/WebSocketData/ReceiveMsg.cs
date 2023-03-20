using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLiveTool.Data
{
    /* json type
     {
         "cmd": ...//有些消息可能没有 cmd
         ...
     }   
     */
    public class ReceiveMsg : IBiliMsg
    {
        public string CMD { get; private set; }
        public ReceiveMsg(string json)
        {
            JObject jo = JObject.Parse(json);
            CMD = jo["cmd"]?.ToString();
        }
    }
}
