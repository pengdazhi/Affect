using System;

namespace Common.Configuration
{
    /// <summary>
    /// 日志配置操作类
    /// </summary>
    public class LogConfigs : ConfigsBase
    {
        private static LogConfigs _instance;
        public static LogConfigs Instance()
        {
            return _instance ?? (_instance = new LogConfigs());
        }

        public override string ConfigFilePath
        {
            get { return Utils.GetMapPath("/Configs/log4net.config"); }
        }
        protected override Type ConfigInfoType
        {
            get { return typeof(LogConfigInfo); }
        }

        public LogConfigs()
        {
            Load();
        }

        private LogConfigInfo _config;

        public LogConfigInfo GetConfig()
        {
            return _config;
        }

        /// <summary>
        /// 从配置文件载入
        /// </summary>
        protected void Load()
        {
            try
            {
                _config = (LogConfigInfo)LoadConfig();
            }
            catch
            {
                _config = new LogConfigInfo();
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        public void Save(LogConfigInfo config)
        {
            try
            {
                SaveConfig(config);
                Load();
            }
            catch (Exception ex)
            {
                throw new Exception("保存日志配置文件异常：" + ex.Message);
            }
        }
    }
}
