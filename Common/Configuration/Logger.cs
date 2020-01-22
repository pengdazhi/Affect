using System;
using System.Diagnostics;
using System.IO;
using log4net;
using Common.Configuration;

namespace Common
{
    /// <summary>
    /// 日志记录者类,可分类记录调试日志,错误日志,404日志,信息日志
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// 调试日志记录者
        /// </summary>
        private static readonly ILog DebugLogger;
        /// <summary>
        /// 错误日志记录者
        /// </summary>
        private static readonly ILog ErrorLogger;
        /// <summary>
        /// 404日志记录者
        /// </summary>
        private static readonly ILog _404Logger;
        /// <summary>
        /// 信息日志记录者
        /// </summary>
        private static readonly ILog _infoLogger;

        static Logger()
        {

            try
            {
                log4net.Config.XmlConfigurator.Configure(new FileInfo(Utils.GetMapPath(LogConfigs.Instance().GetConfig().Log4NetConfigFile)));
            }
            catch (Exception ex)
            {
                LoggerLog("日志记录者初始化错误:" + ex.Message + ",时间:" + Utils.GetDate());
            }
            DebugLogger = LogManager.GetLogger("debugLogger");
            ErrorLogger = LogManager.GetLogger("errorLogger");
            _404Logger = LogManager.GetLogger("_404Logger");
            _infoLogger = LogManager.GetLogger("infoLogger");
        }

        /// <summary>
        /// 记录调试信息
        /// </summary>
        /// <param name="debugMessage"></param>
        public static void Debug(string debugMessage)
        {
            if (DebugLogger.IsDebugEnabled && LogConfigs.Instance().GetConfig().IsDebug)
            {
                DebugLogger.Debug(debugMessage);
            }
        }

        /// <summary>
        /// 记录错误信息
        /// </summary>
        /// <param name="errorMessage"></param>
        public static void Error(string errorMessage)
        {
            if (ErrorLogger.IsErrorEnabled && LogConfigs.Instance().GetConfig().IsError)
            {
                ErrorLogger.Error(errorMessage);
            }
        }
        /// <summary>
        /// 记录404页面信息
        /// </summary>
        /// <param name="url"></param>
        /// <param name="userIp"></param>
        public static void _404(string url, string userIp)
        {
            if (_404Logger.IsInfoEnabled && LogConfigs.Instance().GetConfig().Is404)
            {
                _404Logger.InfoFormat("{0} -- {1} -- {2}", url, userIp, DateTime.Now.ToString());
            }
        }
        /// <summary>
        /// 记录信息日志
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            if (_infoLogger.IsInfoEnabled)
            {
                _infoLogger.Info(message);
            }
        }
        /// <summary>
        /// 记录信息日志
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public static void Info(string format, params object[] args)
        {
            if (_infoLogger.IsInfoEnabled)
            {
                _infoLogger.InfoFormat(format, args);
            }
        }
        /// <summary>
        /// 记录到操作系统日志
        /// </summary>
        /// <param name="message"></param>
        private static void LoggerLog(string message)
        {
            try
            {
                Console.Error.WriteLine(message);
                Trace.WriteLine(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
