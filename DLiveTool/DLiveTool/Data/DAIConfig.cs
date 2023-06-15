using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLiveTool
{
    [Serializable]
    public class DAIConfig
    {
        public bool IsAllowDanmakuResponse = false;
        public string Cookie = "";
        /// <summary>
        /// 在cookie中找到的csrf值, Cookie更新时自动更新
        /// </summary>
        public string InCookie_csrf = "";
        /// <summary>
        /// 在cookie中找到的userId值, Cookie更新时自动更新
        /// </summary>
        public string InCookie_UserId = "";
    }
}
