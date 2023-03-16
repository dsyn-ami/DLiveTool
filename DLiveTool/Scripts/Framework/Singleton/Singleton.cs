using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace dsyn
{
    /// <summary>
    /// 单例，子类必须包括非公共无参构造函数，否则直接报错
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Singleton<T> where T : Singleton<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    Type type = typeof(T);
                    //获取实例非公开构造函数数组
                    ConstructorInfo[] ctors = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                    //找到其中的无参构造函数
                    ConstructorInfo ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);
                    //如果没有非公开无参构造函数，抛出异常
                    if (ctor == null)
                    {
                        throw new Exception("[SingletonError] : can not find non public contructor in " + type.Name);
                    }
                    //执行构造函数生成实例
                    _instance = ctor.Invoke(null) as T;
                }
                return _instance;
            }
        }
    }
}
