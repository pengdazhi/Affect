using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Configuration
{
    public class SysConfigs :ConfigsBase
    {
        private static SysConfigs _instance;
        public static SysConfigs Instance()
        {
            return _instance ?? (_instance = new SysConfigs());
        }

        public override string ConfigFilePath
        {
            get { return Utils.GetMapPath("/Configs/sysconfig.config"); }
        }
        protected override Type ConfigInfoType
        {
            get { return typeof(SysConfigInfo); }
        }

        public SysConfigs()
        {
            Load();
        }

        private SysConfigInfo _config;

        public SysConfigInfo GetConfig()
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
                _config = (SysConfigInfo)LoadConfig();
            }
            catch
            {
                _config = new SysConfigInfo();
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        public void Save(SysConfigInfo config)
        {
            try
            {
                SaveConfig(config);
                Load();
            }
            catch (Exception ex)
            {
                throw new Exception("保存站点默认配置文件异常：" + ex.Message);
            }
        }
    }
}
