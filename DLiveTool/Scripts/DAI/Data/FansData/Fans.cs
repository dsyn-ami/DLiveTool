using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI
{
    [Serializable]
    public class Fans
    {
        /// <summary>
        /// UID
        /// </summary>
        public string UID { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 亲密度
        /// </summary>
        public long Intimacy { get; set; }
    }
    [Serializable]
    public class FansDatas
    {
        public List<Fans> Fans = new List<Fans>();
    }
}
