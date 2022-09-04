﻿using System;
using System.IO;
using wms.Client.LogicCore.Helpers.Files;

namespace wms.Client.LogicCore.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class SerivceFiguration
    {
        /// <summary>
        /// 配置文件
        /// </summary>
        public const string INI_CFG = "\\user.ini";

        /// <summary>
        /// 获取本地样式参数
        /// </summary>
        /// <returns></returns>
        public static string GetSkin()
        {
            string cfgINI = AppDomain.CurrentDomain.BaseDirectory + INI_CFG;
            if (File.Exists(cfgINI))
            {
                IniFile ini = new IniFile(cfgINI);
                string SkinName = ini.IniReadValue("Skin", "Skin");
                return SkinName;
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// 设置样式
        /// </summary>
        /// <param name="SkinName"></param>
        public static void SetKin(string SkinName)
        {
            string cfgINI = AppDomain.CurrentDomain.BaseDirectory + INI_CFG;
            IniFile ini = new IniFile(cfgINI);
            ini.IniWriteValue("Skin", "Skin", SkinName);
        }
    }
}
