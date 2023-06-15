using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI
{
    [AttributeUsage(AttributeTargets.Class)]
    public class DAICommandAttribute : Attribute
    {
        public DAICommandAttribute(string command)
        {
            Command = command;
        }

        public string Command { get; set; }
    }
}
