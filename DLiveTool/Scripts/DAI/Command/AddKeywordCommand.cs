using DLiveTool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI
{
    [DAICommand("!t")]
    public class AddKeywordCommand : DAICommandBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="args">args[0] : [keyword]:[answer]</param>
        /// <param name="outputMask"></param>
        /// <returns></returns>
        public override OutputMsg Excute(string uid, string userName, string[] args)
        {
            //参数不符合条件
            if(args == null && args.Length != 1)
            {
                return null;
            }
            string[] splitStr = args[0].Split(':', '：');
            if(splitStr.Length != 2)
            {
                return null;
            }
            string keyword = splitStr[0];
            string answer = splitStr[1];

            KeywordAnswerDataMgr.Instance.AddKeywordAnswer(uid, userName, keyword, answer);
            OutputMsg output = DAIMgr.Instance.DAICore.AddKeywordChat(userName, keyword, answer);
            return output;
        }
    }
}
