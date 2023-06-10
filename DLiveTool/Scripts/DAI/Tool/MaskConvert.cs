using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAI
{
    /// <summary>
    /// 将输入输出目标与Mask相互转换
    /// </summary>
    public static class MaskConvert
    {
        public static byte GetOutputMask(params OutputType[] args)
        {
            byte result = 0;
            for(int i = 0; i < args.Length; i++)
            {
                result += (byte)args[i];
            }
            return result;
        }
        public static byte GetInputMask(params InputType[] args)
        {
            byte result = 0;
            for(int i = 0; i < args.Length; i++)
            {
                result += (byte)args[i];
            }
            return result;
        }
        public static bool IsInMask(byte mask, OutputType type) => (mask & (byte)type) > 0;
        public static bool IsInMask(byte mask, InputType type) => (mask & (byte)type) > 0;
    }
}
