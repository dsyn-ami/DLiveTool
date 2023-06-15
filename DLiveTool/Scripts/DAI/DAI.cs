using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dsyn;

namespace DAI
{
    public class DAICore
    {
        #region 各种生成消息的类
        FreeChat _freeChat = new FreeChat();
        GetGiftChat _getGiftChat = new GetGiftChat();
        WelcomeFansChat _welcomeFansChat = new WelcomeFansChat();
        KeywordChat _keywordChat = new KeywordChat();
        AddKeywordChat _addKeywordChat = new AddKeywordChat();
        #endregion

        #region 公开方法
        /// <summary>
        /// 添加关键词成功后的回复
        /// </summary>
        /// <returns></returns>
        public OutputMsg AddKeywordChat(string userName, string keyword, string answer)
        {
            string msg = _addKeywordChat.GetRandomMsg(userName, keyword, answer);
            OutputMsg output = new OutputMsg();
            output.Msg = msg;
            output.OutPutTargetMask = MaskConvert.GetOutputMask(OutputType.LiveRoom, OutputType.WebSocket);
            return output;
        }
        /// <summary>
        /// 获取关键词匹配回复
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public OutputMsg KeywordChat(InputMsg input)
        {
            string msg = _keywordChat.GetRandomMsg(input.Msg);
            if (string.IsNullOrEmpty(msg))
            {
                return null;
            }
            else
            {
                OutputMsg output = new OutputMsg();
                output.Msg = msg;
                output.OutPutTargetMask = MaskConvert.GetOutputMask(OutputType.LiveRoom, OutputType.WebSocket);
                return output;
            }
        }

        /// <summary>
        /// 获取欢迎回复
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="initmacy">亲密度</param>
        /// <returns></returns>
        public OutputMsg WelcomeFansChat(string userName, long initmacy)
        {
            OutputMsg output = new OutputMsg();
            string msg = _welcomeFansChat.GetRandomMsg(userName, initmacy);
            if (string.IsNullOrEmpty(msg)) return null;
            output.Msg = msg;
            output.OutPutTargetMask = MaskConvert.GetOutputMask(OutputType.LiveRoom, OutputType.WebSocket);
            return output;
        }
        /// <summary>
        /// 获取收到礼物的回复
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="giftName"></param>
        /// <returns></returns>
        public OutputMsg GiftChat(string userName, string giftName)
        {
            OutputMsg output = new OutputMsg();
            string msg = _getGiftChat.GetRandomMsg(userName, giftName);
            output.Msg = msg;
            output.OutPutTargetMask = MaskConvert.GetOutputMask(OutputType.LiveRoom, OutputType.WebSocket);
            return output;
        }
        /// <summary>
        /// 获取闲聊输出
        /// </summary>
        /// <returns></returns>
        public OutputMsg FreeChat()
        {
            OutputMsg output = new OutputMsg();
            string msg = _freeChat.GetRandomMsg();
            output.Msg = msg;
            output.OutPutTargetMask = MaskConvert.GetOutputMask(OutputType.LiveRoom, OutputType.WebSocket);
            return output;
        }

        #endregion
    }
}
