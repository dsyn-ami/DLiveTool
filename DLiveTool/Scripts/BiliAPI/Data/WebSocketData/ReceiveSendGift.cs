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
        "bag_gift":null,
        "batch_combo_id":"batch:gift:combo_id:3493268838942937:226638530:31036:1686813397.7487",
        "batch_combo_send":{
            "action":"投喂",
            "batch_combo_id":"batch:gift:combo_id:3493268838942937:226638530:31036:1686813397.7487",
            "batch_combo_num":1,
            "blind_gift":null,
            "gift_id":31036,
            "gift_name":"小花花",
            "gift_num":3,
            "send_master":null,
            "uid":3493268838942937,
            "uname":"一念大人的狗"
        },
        "beatId":"0",
        "biz_source":"Live",
        "blind_gift":null,
        "broadcast_id":0,
        "coin_type":"gold",
        "combo_resources_id":1,
        "combo_send":{
            "action":"投喂",
            "combo_id":"gift:combo_id:3493268838942937:226638530:31036:1686813397.7476",
            "combo_num":3,
            "gift_id":31036,
            "gift_name":"小花花",
            "gift_num":3,
            "send_master":null,
            "uid":3493268838942937,
            "uname":"一念大人的狗"
        },
        "combo_stay_time":5,
        "combo_total_coin":300,
        "crit_prob":0,
        "demarcation":1,
        "discount_price":100,
        "dmscore":112,
        "draw":0,
        "effect":0,
        "effect_block":0,
        "face":"https://i1.hdslb.com/bfs/face/6fb05f895d854e68419d45eef2c2e272b04ad25f.jpg",
        "face_effect_id":0,
        "face_effect_type":0,
        "float_sc_resource_id":0,
        "giftId":31036,
        "giftName":"小花花",
        "giftType":0,
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
            "is_lighted":0,
            "medal_color":0,
            "medal_color_border":0,
            "medal_color_end":0,
            "medal_color_start":0,
            "medal_level":0,
            "medal_name":"",
            "special":"",
            "target_id":0
        },
        "name_color":"",
        "num":3,
        "original_gift_name":"",
        "price":100,
        "rcost":106689,
        "receive_user_info":{
            "uid":226638530,
            "uname":"断殇一念"
        },
        "remain":0,
        "rnd":"2193346195269637632",
        "send_master":null,
        "silver":0,
        "super":0,
        "super_batch_gift_num":1,
        "super_gift_num":3,
        "svga_block":0,
        "switch":true,
        "tag_image":"",
        "tid":"2193346195269637632",
        "timestamp":1686813397,
        "top_list":null,
        "total_coin":300,
        "uid":3493268838942937,
        "uname":"一念大人的狗"
    }
}
     */
    public class ReceiveSendGift : IBiliMsg
    {
        public string UserId { get; private set; }
        public string UserName { get; private set; }
        public string UserIcon { get; private set; }
        public string GiftId { get; private set; }
        public string GiftName { get;  private set; }
        public int GiftCount { get; private set; }
        /// <summary>
        /// 单个礼物价值 1元 == 10电池 == 1000
        /// 非付费礼物价值不为 0  比如辣条为100
        /// </summary>
        public int Price { get; private set; }

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
            Price = (int)token["price"];
        }
    }
}


