using System;
using log4net;

namespace AiWin_New
{
    /// <summary>
    /// log4net非公共类
    /// 用于特定配置,存入特定文件及文件夹内
    /// </summary>
    public class LogFileFolder
    {
        /// <summary>
        /// 前缀,在配置文件中对应
        /// </summary>
        private string PrdfixName { get; set; }

        private readonly ILog _debug;
        private readonly ILog _info;
        private readonly ILog _error;
        private readonly ILog _warn;
        private readonly ILog _fatal;

        public LogFileFolder(string prdfixName)
        {
            PrdfixName = prdfixName;

            _debug = LogManager.GetLogger(PrdfixName + "_DEBUG");
            _info = LogManager.GetLogger(PrdfixName + "_INFO");
            _error = LogManager.GetLogger(PrdfixName + "_ERROR");
            _warn = LogManager.GetLogger(PrdfixName + "_WARN");
            _fatal = LogManager.GetLogger(PrdfixName + "_FATAL");
        }

        public void Debug(string log, Exception ex = null)
        {
            _debug.Debug(log, ex);
        }

        public void DebugFormat(string format, params object[] args)
        {
            _debug.DebugFormat(format, args);
        }

        public void Info(string log, Exception ex = null)
        {
            _info.Info(log, ex);
        }

        public void InfoFormat(string format, params object[] args)
        {
            _info.InfoFormat(format, args);
        }

        public void Error(string log, Exception ex = null)
        {
            _error.Error(log, ex);
        }

        public void ErrorFormat(string format, params object[] args)
        {
            _error.ErrorFormat(format, args);
        }

        public void Warn(string log, Exception ex = null)
        {
            _warn.Warn(log, ex);
        }

        public void WarnFormat(string format, params object[] args)
        {
            _warn.WarnFormat(format, args);
        }

        public void Fatal(string log, Exception ex = null)
        {
            _fatal.Fatal(log, ex);
        }

        public void FatalFormat(string format, params object[] args)
        {
            _fatal.FatalFormat(format, args);
        }
    }
}
