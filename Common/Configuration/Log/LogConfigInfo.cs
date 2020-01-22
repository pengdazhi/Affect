namespace Common.Configuration
{
    /// <summary>
    /// 日志配置类
    /// </summary>
    public class LogConfigInfo : IConfigInfo
    {
        private bool _isDebug = true;
        /// <summary>
        /// 是否记录调试日志
        /// </summary>
        public bool IsDebug
        {
            get { return _isDebug; }
            set { _isDebug = value; }
        }

        private bool _isError = true;
        /// <summary>
        /// 是否记录错误日志
        /// </summary>
        public bool IsError
        {
            get { return _isError; }
            set { _isError = value; }
        }

        private bool _is404 = true;
        /// <summary>
        /// 是否记录404错误
        /// </summary>
        public bool Is404
        {
            get { return _is404; }
            set { _is404 = value; }
        }
        private string _log4NetConfigFile = "/Configs/log4net.config";
        /// <summary>
        /// log4net的相对于网站根目录的配置文件路径
        /// </summary>
        public string Log4NetConfigFile
        {
            get { return _log4NetConfigFile; }
            set { _log4NetConfigFile = value; }
        }
    }
}
