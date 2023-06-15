using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLiveTool.Data
{
    /*
    "code":0, 0表示发送成功
    "data":{
        "mode_info":{
            "mode":0,
            "show_player_type":0,
            "extra":"{\"send_from_me\":true,\"mode\":0,\"color\":16777215,\"dm_type\":0,\"font_size\":25,\"player_mode\":1,\"show_player_type\":0,\"content\":\"233\",\"user_hash\":\"3252098211\",\"emoticon_unique\":\"\",\"bulge_display\":0,\"recommend_score\":8,\"main_state_dm_color\":\"\",\"objective_state_dm_color\":\"\",\"direction\":0,\"pk_direction\":0,\"quartet_direction\":0,\"anniversary_crowd\":0,\"yeah_space_type\":\"\",\"yeah_space_url\":\"\",\"jump_to_url\":\"\",\"space_type\":\"\",\"space_url\":\"\",\"animation\":{},\"emots\":null,\"is_audited\":false,\"id_str\":\"429f1f161a366db3d4474dab8dc2ace674\"}"
        },
        "dm_v2":"CiI0MjlmMWYxNjFhMzY2ZGIzZDQ0NzRkYWI4ZGMyYWNlNjc0EAEYGSD///8HKghjMWQ3MTRhMzIDMjMzOIv+wsmLMUjP66ekBmIAigEAkAEBmgEQCgg5MjJBRTE0QxDPiqakBqIBiQEI2ZGAwsCjmgYSEuS4gOW/teWkp+S6uueahOeLlyJKaHR0cHM6Ly9pMS5oZHNsYi5jb20vYmZzL2ZhY2UvNmZiMDVmODk1ZDg1NGU2ODQxOWQ0NWVlZjJjMmUyNzJiMDRhZDI1Zi5qcGc4kE5AAVoCCAFiDxCWrdoEGgY+NTAwMDAgAmoAcgB6AKoBBRiasJkE"
    },
    "message":"",
    "msg":""
}
     */

    /// <summary>
    /// 发送弹幕的响应
    /// </summary>
    public class SendDanmakuResponse
    {
        /// <summary>
        /// 0 : 发送成功
        /// </summary>
        public int Code;
        public string ErrorMsg;

        public SendDanmakuResponse(string json)
        {
            JObject jo = JObject.Parse(json);
            Code = (int)jo["code"];
            ErrorMsg = jo["message"].ToString();
        }
    }
}
