﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLiveTool.Data
{
    /* json type
     {
    "cmd":"DANMU_MSG",
    "info":[
        [
            0,
            4,
            25,
            14893055,
            1629862164294, 时间戳(毫秒)
            124860207, 进入房间时间
            0,
            "b9dd70e0",
            0,
            0,
            5, X不是用户等级
            "#1453BAFF,#4C2263A2,#3353BAFF",用户等级颜色
            0,//弹幕类型 0 普通弹幕 1 表情图片
            "{}",//表情图片信息
            "{}"
        ],
        "虽然是我们打sc逼着她说的", 弹幕内容
        [
            19316585, 用户ID
            "悠响", 用户名
            0, 是否房管
            0, 是否月费姥爷
            0, 是否年费姥爷
            10000,
            1,
            "#00D1F1"
        ],
        [
            30, 勋章等级
            "鹤仙咕", 勋章名
            "鹤羽Official", 勋章主播名
            21359166, 勋章房间ID
            2951253,
            "",
            0,
            16771156,
            2951253,
            10329087,
            1,
            1,
            12862137
        ],
        [
            42,
            0,
            16746162,
            25771,
            0
        ],
        [
            "title-355-1",
            "title-355-1"
        ],
        0,
        3,
        null,
        {
            "ts":1629862164, 时间戳
            "ct":"D0214267"
        },
        0,
        0,
        null,
        null,
        0,
        105
    ]
    }
     */

    public class ReceiveDanmakuMsg : IBiliMsg
    {
        /// <summary>
        /// 弹幕类型
        /// </summary>
        public enum DanmakuType
        {
            //文本弹幕
            Text = 0,
            //图片表情弹幕
            ImgEmoticon = 1,
        }
        public string UserId { get; private set; }
        public string UserName { get; private set; }
        public string Message { get; private set; }
        public DanmakuType Type { get; private set; }
        /// <summary>
        /// 表情数据，如果弹幕类型不是 图片表情 类型，该项为空
        /// </summary>
        public EmoticonData Emoticon { get; private set; }

        public ReceiveDanmakuMsg(string json)
        {
            JObject jo = JObject.Parse(json);
            UserId = jo["info"][2][0].ToString();
            UserName = jo["info"][2][1].ToString();
            Message = jo["info"][1].ToString();
            Type = (DanmakuType)(int)jo["info"][0][12];

            if(Type == DanmakuType.ImgEmoticon)
            {
                string emoticonJson = jo["info"][0][13].ToString();
                Emoticon = new EmoticonData(emoticonJson);
            }
        }
    }
}
