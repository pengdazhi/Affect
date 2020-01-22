using System;
using System.IO;

namespace Common.Configuration
{
    /// <summary>
    /// 配置抽象基类
    /// </summary>
    public abstract class ConfigsBase
    {
        public abstract string ConfigFilePath
        {
            get;
        }
        protected abstract Type ConfigInfoType
        {
            get;
        }
        /// <summary>
        /// 锁对象
        /// </summary>
        protected object LockHelper = new object();
        /// <summary>
        /// 从文件中加载配置
        /// </summary>
        /// <returns></returns>
        protected IConfigInfo LoadConfig()
        {
            lock (LockHelper)
            {
                if (!File.Exists(ConfigFilePath))
                    throw new Exception("文件:" + ConfigFilePath + " 不存在");

                IConfigInfo iconfig = (IConfigInfo)SerializationHelper.Load(ConfigInfoType, ConfigFilePath);
                return iconfig;
            }
        }
        /// <summary>
        /// 保存配置到文件中 
        /// </summary>
        /// <param name="configinfo"></param>
        /// <returns></returns>
        protected void SaveConfig(IConfigInfo configinfo)
        {
            SerializationHelper.Save(configinfo, ConfigFilePath);
        }
    }
}
