using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dsyn
{
    /// <summary>
    /// 和时间相关的工具
    /// </summary>
    public static class Time
    {
        /// <summary>
        /// 获取当前时间戳
        /// </summary>
        /// <param name="isMillisecond">是否精确到毫秒</param>
        /// <returns></returns>
        public static long GetTimeStamp(bool isMillisecond = false)
        {
            TimeSpan ts = DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(isMillisecond ? ts.TotalMilliseconds : ts.TotalSeconds);//精确到毫秒
        }
    }
}
