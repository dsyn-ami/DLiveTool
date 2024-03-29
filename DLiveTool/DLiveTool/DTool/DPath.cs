﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLiveTool
{
    /// <summary>
    /// 记录常用路径
    /// </summary>
    public static class DPath
    {
        static string _rootPath = AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "/");
        /// <summary>
        /// 程序根目录地址
        /// </summary>
        public static string RootPath => _rootPath;


        static string _imgCachePath = Path.Combine(_rootPath, "Cache/ImgCache");
        /// <summary>
        /// 本地图片缓存文件夹地址
        /// </summary>
        public static string ImgCachePath => _imgCachePath;


        static string _voiceCacheRootPath = Path.Combine(_rootPath, "Cache/VoiceCache");
        /// <summary>
        /// 语音文件缓存跟目录
        /// </summary>
        public static string VoiceCacheRootPath => _voiceCacheRootPath;


        static string _configDataRootPath = Path.Combine(_rootPath, "Config");
        /// <summary>
        /// 配置数据保存位置
        /// </summary>
        public static string ConfigDataRootPath => _configDataRootPath;
    }
}
