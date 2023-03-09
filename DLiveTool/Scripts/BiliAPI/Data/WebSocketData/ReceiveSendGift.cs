using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace DLiveTool.Data
{
    /*
     {
    "cmd":"SEND_GIFT",
    "data":{
        "action":"投喂",
        "batch_combo_id":"",
        "batch_combo_send":null,
        "beatId":"",
        "biz_source":"Live",
        "blind_gift":null,
        "broadcast_id":0,
        "coin_type":"silver",
        "combo_resources_id":1,
        "combo_send":null,
        "combo_stay_time":5,
        "combo_total_coin":0,
        "crit_prob":0,
        "demarcation":1,
        "discount_price":0,
        "dmscore":36,
        "draw":0,
        "effect":0,
        "effect_block":1,
        "face":"https://i0.hdslb.com/bfs/face/d042fd27013fd68be19cac736e1cbdf47f494273.jpg",
        "face_effect_id":0,
        "face_effect_type":0,
        "float_sc_resource_id":0,
        "giftId":1,
        "giftName":"辣条",
        "giftType":5,
        "gold":0,
        "guard_level":0,
        "is_first":true,
        "is_join_receiver":false,
        "is_naming":false,
        "is_special_batch":0,
        "magnification":1,
        "medal_info":{
            "anchor_roomid":0,
            "anchor_uname":"",
            "guard_level":0,
            "icon_id":0,
            "is_lighted":1,
            "medal_color":13081892,
            "medal_color_border":13081892,
            "medal_color_end":13081892,
            "medal_color_start":13081892,
            "medal_level":18,
            "medal_name":"白河愁",
            "special":"",
            "target_id":34646754
        },
        "name_color":"",
        "num":2,
        "original_gift_name":"",
        "price":100,
        "rcost":145350458,
        "receive_user_info":{
            "uid":34646754,
            "uname":"沉默寡言白河愁"
        },
        "remain":0,
        "rnd":"2037886925",
        "send_master":null,
        "silver":0,
        "super":0,
        "super_batch_gift_num":0,
        "super_gift_num":0,
        "svga_block":0,
        "switch":true,
        "tag_image":"",
        "tid":"1678266133120000001",
        "timestamp":1678266133,
        "top_list":null,
        "total_coin":200,
        "uid":270503310,
        "uname":"_明微"
    }
}
     */
    public class ReceiveSendGift
    {
        public string UserId { get; private set; }
        public string UserName { get; private set; }
        public string UserIcon { get; private set; }
        public string GiftId { get; private set; }
        public string GiftName { get;  private set; }
        public int GiftCount { get; private set; }

        public ReceiveSendGift(string json)
        {
            JObject jo = JObject.Parse(json);
            JToken token = jo["data"];
            UserId = token["uid"].ToString();
            UserName = token["uname"].ToString();
            UserIcon = token["face"].ToString();
            GiftId = token["giftId"].ToString();
            GiftName = token["giftName"].ToString();
            GiftCount = (int)token["num"];
        }
    }
}


