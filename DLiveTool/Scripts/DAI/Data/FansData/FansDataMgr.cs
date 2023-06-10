using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dsyn;

namespace DAI
{
    public class FansDataMgr : Singleton<FansDataMgr>
    {
        private string _dirRoot;
        private string _configPath;
        public FansDatas _fansDatas;
        private FansDataMgr()
        {
            Init();
        }
        #region 公开函数

        public void SaveData()
        {
            if(_fansDatas != null)
            {
                FileWriter.WriteJsonObj(_configPath, _fansDatas);
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
            _dirRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DAI/FansData/");
            _configPath = Path.Combine(_dirRoot, "FansData.json");
            if (!Directory.Exists(_dirRoot))
            {
                Directory.CreateDirectory(_dirRoot);
            }
        }
        private void LoadFansData()
        {
            if (File.Exists(_configPath))
            {
                _fansDatas = FileReader.LoadJsonObj<FansDatas>(_configPath);
            }
            else
            {
                _fansDatas = new FansDatas();
                _fansDatas.Fans = new List<Fans>();
                SaveData();
            }
        }

        #endregion
    }
}
