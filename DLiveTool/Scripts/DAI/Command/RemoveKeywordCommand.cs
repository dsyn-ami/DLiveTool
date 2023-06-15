using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI
{
    [DAICommand("#rmkw")]
    public class RemoveKeywordCommand : DAICommandBase
    {
        public override OutputMsg Excute(string uid, string userName, string[] args)
        {
            if (args == null || args.Length != 1) return null;
            string keyword = args[0];
            bool isSuccess = KeywordAnswerDataMgr.Instance.RemoveKeyword(keyword);
            OutputMsg output = new OutputMsg();
            if (isSuccess)
            {
                output.Msg = $"已删除{keyword}";
            }
            else
            {
                output.Msg = $"{keyword}不存在";
            }
            return output;
        }
    }
}
