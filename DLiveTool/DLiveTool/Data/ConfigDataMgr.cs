using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using dsyn;

namespace DLiveTool
{
    public class ConfigDataMgr : Singleton<ConfigDataMgr>
    {
        DLiveConfigData _data;
        string _configDataRootPath;
        string _configDataPath;

        private ConfigDataMgr()
        {
            //初始化配置文件路径
            _configDataRootPath = DPath.ConfigDataRootPath;
            _configDataPath = Path.Combine(_configDataRootPath, "Config.json");

            //初始化配置文件夹
            if (!Directory.Exists(_configDataRootPath))
            {
                Directory.CreateDirectory(_configDataRootPath);
            }
            //加载配置文件
            if (!File.Exists(_configDataPath))
            {
                _data = new DLiveConfigData();
                FileWriter.WriteJsonObj<DLiveConfigData>(_configDataPath, _data);
            }
            else
            {
                _data = FileReader.LoadJsonObj<DLiveConfigData>(_configDataPath);
            }
        }

        public DLiveConfigData Data => _data;
        
        public void SaveData()
        {
            FileWriter.WriteJsonObj<DLiveConfigData>(_configDataPath, _data);
        }
    }
}
