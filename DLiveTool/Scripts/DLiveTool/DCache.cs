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
            _emoticonFolder = DPath.EmoticonCachePath;
            _faceFolder = DPath.FaceCachePath;

            //初始化缓存目录
            if (!Directory.Exists(_emoticonFolder))
            {
                Directory.CreateDirectory(_emoticonFolder);
            }
            if (!Directory.Exists(_faceFolder))
            {
                Directory.CreateDirectory(_faceFolder);
            }
            
            //获取所有缓存文件信息
            DirectoryInfo dirInfo = new DirectoryInfo(_faceFolder);
            FileInfo[] fileInfos = dirInfo.GetFiles();
            foreach (FileInfo fileInfo in fileInfos)
            {
                AddImageCache(fileInfo.Name, Path.Combine(_faceRelativePath, fileInfo.Name));
            }
            dirInfo = new DirectoryInfo(_emoticonFolder);
            fileInfos = dirInfo.GetFiles();
            foreach (FileInfo fileInfo in fileInfos)
            {
                AddImageCache(fileInfo.Name, Path.Combine(_emoticonRelativePath, fileInfo.Name));
            }
        }
        /// <summary>
        /// 表情本地缓存文件夹相对地址
        /// </summary>
        const string _emoticonRelativePath = "/Cache/Emoticon/";
        /// <summary>
        /// 头像本地缓存文件夹相对地址
        /// </summary>
        const string _faceRelativePath = "/Cache/Face/";
        /// <summary>
        /// 表情本地缓存文件夹地址
        /// </summary>
        static string _emoticonFolder;
        /// <summary>
        /// 头像本地缓存文件夹地址
        /// </summary>
        static string _faceFolder;

        /// <summary>
        /// 表情,头像等缓存信息的字典
        /// string : 资源的url地址中图片的名字
        /// string : 资源的本地相对根目录路径
        /// </summary>
        private static Dictionary<string, string> _imgCacheDict = new Dictionary<string, string>();

        /// <summary>
        /// 添加新的缓存信息到缓存目录
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileRelativePath"></param>
        public static void AddImageCache(string url, string fileRelativePath)
        {
            if (!_isInited) Init();
            if (!_imgCacheDict.ContainsKey(url))
            {
                _imgCacheDict.Add(url, fileRelativePath);
            }
        }
        /// <summary>
        /// 获取缓存,
        /// 如果本地有缓存,返回缓存文件相对地址,
        /// 否则,返回空
        /// </summary>
        /// <param name="url">资源的url地址</param>
        /// <returns></returns>
        public static string GetImageCache(string url)
        {
            if (!_isInited) Init();
            if (_imgCacheDict.ContainsKey(url))
            {
                return _imgCacheDict[url];
            }
            return null;
        }
    }
}
