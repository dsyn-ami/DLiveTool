using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dsyn;
using DAI;
using System.Reflection;
using DLiveTool;

namespace DAI
{
    public class DAIMgr : Singleton<DAIMgr>
    {
        DAICore _daiCore = new DAICore();
        public DAICore DAICore => _daiCore;

        /// <summary>
        /// 指令字典
        /// string 指令
        /// Type 指令类型
        /// </summary>
        private Dictionary<string, Type> _commandTypeDict;

        #region 初始化
        private DAIMgr()
        {
            InitCommandType();
        }
        private void InitCommandType()
        {
            _commandTypeDict = new Dictionary<string, Type>();
            //获取所有带有 DAICommandAttribute 特性的类
            IEnumerable<Type> types = Assembly.GetExecutingAssembly().GetTypes().Where(type => type.GetCustomAttributes<DAICommandAttribute>().Any());
            //加入缓存字典
            foreach (Type type in types)
            {
                DAICommandAttribute attr = type.GetCustomAttribute<DAICommandAttribute>();
                _commandTypeDict.Add(attr.Command, type);
            }
        }

        #endregion

        #region 公开函数
        /// <summary>
        /// 处理输入, 无输出的话, 返回为空
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public OutputMsg HandleInput(InputMsg input)
        {
            //处理直播间弹幕输入
            if(MaskConvert.IsInMask(input.InputType, InputType.LiveRoom))
            {
                //判断是否是指令弹幕
                //!t [keyword]:[response]
                //#ui 100 [UID]
                string msg = input.Msg;
                //普通弹幕指令
                if(msg.StartsWith("!"))
                {
                    return HandleCommand(input);
                }
                //管理员弹幕指定
                else if (msg.StartsWith("#"))
                {
                    //判断是否有权限执行指令
                    if(!string.IsNullOrEmpty(AnchorData.UserId.Value) && AnchorData.UserId.Value.Equals(input.UID))
                    {
                        return HandleCommand(input);
                    }
                    else
                    {
                        return null;
                    }
                }
                //普通弹幕
                else
                {
                    ///获取关键词匹配的回复
                    return _daiCore.KeywordChat(input);
                }
            }
            //处理控制台输入
            if(MaskConvert.IsInMask(input.InputType, InputType.Console))
            {
                return HandleCommand(input);
            }
            return null;
        }

        #endregion

        #region 其他函数

        private OutputMsg HandleCommand(InputMsg input)
        {
            string[] splitStr = input.Msg.Split(" ");
            if(splitStr.Length <= 1)
            {
                return null;
            }
            string command = splitStr[0];
            string[] args = splitStr.Skip(1).ToArray();
            if (_commandTypeDict.ContainsKey(command))
            {
                Type commandType = _commandTypeDict[command];
                ConstructorInfo ctor = commandType.GetConstructors()[0];
                var commandIns = ctor.Invoke(null) as DAICommandBase;
                return commandIns.Excute(input.UID, input.UserName, args);
            }
            return null;
        }
        #endregion
    }
}
