using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI
{
    public class DAICommandBase
    {
        public virtual OutputMsg Excute(string uid, string userName, string[] args) { return null; }
    }
}
