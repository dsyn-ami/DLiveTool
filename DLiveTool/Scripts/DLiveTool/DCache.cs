using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dsyn;

namespace DLiveTool
{
    //缓存管理
    public static class DCache
    {
        static bool _isInited = false;
        private static void Init()
        {
            _isInited = true;
            //初始化缓存文件夹地址
            _imgCacheFolder = DPath.ImgCachePath;

            //初始化缓存目录
            if (!Directory.Exists(_imgCacheFolder))
            {
                Directory.CreateDirectory(_imgCacheFolder);
            }
            
            //获取所有缓存文件信息
            DirectoryInfo dirInfo = new DirectoryInfo(_imgCacheFolder);
            FileInfo[] fileInfos = dirInfo.GetFiles();
            foreach (FileInfo fileInfo in fileInfos)
            {
                AddImageCache(fileInfo.Name, Path.Combine(_imgCacheFolder, fileInfo.Name));
            }
        }
        /// <summary>
        /// 本地图片缓存路径
        /// </summary>
        static string _imgCacheFolder;
        /// <summary>
        /// 表情,头像等缓存信息的字典
        /// string : 缓存文件名
        /// string : 资源的本地全路径
        /// </summary>
        private static Dictionary<string, string> _imgCacheDict = new Dictionary<string, string>();

        /// <summary>
        /// 添加新的缓存信息到缓存目录
        /// </summary>
        /// <param name="fileName">缓存文件名</param>
        /// <param name="fileFullPath">文件全路径</param>
        public static void AddImageCache(string fileName, string fileFullPath)
        {
            if (!_isInited) Init();
            if (!_imgCacheDict.ContainsKey(fileName))
            {
                _imgCacheDict.Add(fileName, fileFullPath);
            }
        }
        /// <summary>
        /// 获取缓存,
        /// 如果本地有缓存,返回缓存文件相对地址,
        /// 否则,返回空
        /// </summary>
        /// <param name="fileName">缓存文件名</param>
        /// <returns>文件相对根目录路径</returns>
        public static string GetImageCache(string fileName)
        {
            if (!_isInited) Init();
            if (_imgCacheDict.ContainsKey(fileName))
            {
                return _imgCacheDict[fileName];
            }
            return null;
        }
    }
}
