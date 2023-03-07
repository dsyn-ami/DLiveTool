using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLiveTool
{
    /// <summary>
    /// 当值发生改变后，会触发事件
    /// </summary>
    /// <typeparam name="T">管理的值的类型</typeparam>
    public struct EventValue<T>
    {
        /// <summary>
        /// 当值发生改变
        /// </summary>
        public event Action<T, T> OnValueChanged;

        private T _value;
        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                OnValueChanged?.Invoke(_value, value);
                _value = value;
            }
        }
    }
}
