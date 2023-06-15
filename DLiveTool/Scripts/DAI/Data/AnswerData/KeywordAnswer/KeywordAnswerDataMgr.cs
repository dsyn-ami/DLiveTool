using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dsyn;

namespace DAI
{
    public class KeywordAnswerDataMgr : Singleton<KeywordAnswerDataMgr>
    {
        private string _dirRoot;
        private string _configPath;
        public KeywordAnswerDatas _data;
        private KeywordAnswerDataMgr()
        {
            Init();
        }
        #region 公开函数
        /// <summary>
        /// 查找所有关键词匹配弹幕消息的 KeywordAnswer
        /// </summary>
        /// <param name="chatMsg"></param>
        /// <returns></returns>
        public KeywordAnswer[] FindKeywordAnswer(string chatMsg)
        {
            return _data.KeywordAnswer.FindAll(x => chatMsg.Contains(x.Keyword)).ToArray();
        }
        /// <summary>
        /// 增加关键词回答
        /// </summary>
        /// <param name="createUID"></param>
        /// <param name="createName"></param>
        /// <param name="keyword"></param>
        /// <param name="answer"></param>
        public void AddKeywordAnswer(string createUID, string createName, string keyword, string answer)
        {
            KeywordAnswer ka = _data.KeywordAnswer.Find(x => x.Keyword.Equals(keyword));
            if(ka == null)
            {
                ka = new KeywordAnswer();
                ka.CreaterUID = createUID;
                ka.CreaterName = createName;
                ka.CreateTimeStamp = Time.GetTimeStamp();
                ka.Keyword = keyword;
                ka.Answer = new List<string>();
                ka.Answer.Add(answer);
                _data.KeywordAnswer.Add(ka);
            }
            else
            {
                ka.Answer.Add(answer);
            }
            SaveData();
        }
        public bool RemoveKeyword(string keyword)
        {
            KeywordAnswer ka = _data.KeywordAnswer.Find(x => x.Keyword.Equals(keyword));
            if(ka != null)
            {
                _data.KeywordAnswer.Remove(ka);
                SaveData();
                return true;
            }
            else
            {
                return false;
            }
        }
        public void SaveData()
        {
            if (_data != null)
            {
                FileWriter.WriteJsonObj(_configPath, _data);
            }
        }

        #endregion

        #region 其他函数
        private void Init()
        {
            InitDirectory();
            LoadFansData();
        }
        private void InitDirectory()
        {
            _dirRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DAI/KeywordAnswerData/");
            _configPath = Path.Combine(_dirRoot, "KeywordAnswerData.json");
            if (!Directory.Exists(_dirRoot))
            {
                Directory.CreateDirectory(_dirRoot);
            }
        }
        private void LoadFansData()
        {
            if (File.Exists(_configPath))
            {
                _data = FileReader.LoadJsonObj<KeywordAnswerDatas>(_configPath);
            }
            else
            {
                _data = new KeywordAnswerDatas();
                SaveData();
            }
        }

        #endregion
    }
}
