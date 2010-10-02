using System;

namespace OpenWealth
{
    public enum LogLevel { Debug = 0, Info = 1, Warn = 2, Error = 3, Fatal = 4 };
    /// <summary>
    /// Попытался сделать совместимым с Log4Net.ILog интерфейсом
    /// </summary>
    public interface ILog
    {
        void Debug(object message);
        //TODO        void Debug(object message, Exception exception);
        void Info(object message);
        //TODO        void Info(object message, Exception exception);
        void Warn(object message);
        //TODO        void Warn(object message, Exception exception);
        void Error(object message);
        void Error(object message, Exception exception);
        void Fatal(object message);
        //TODO        void Fatal(object message, Exception exception);
        bool IsDebugEnabled { get; }
        bool IsInfoEnabled { get; }
        bool IsWarnEnabled { get; }
        bool IsErrorEnabled { get; }
        bool IsFatalEnabled { get; }

        event LogEventHandler LogEvent;
    }

    public interface ILogManager
    {
        ILog GetLogger(String name);
        void SetLevel(LogLevel level);
    }

    public class LogEventArgs
    {
        public LogEventArgs(DateTime dt,string logName, string message, LogLevel level)
        {
            this.dt = dt;
            this.logName = logName;
            this.level = level;
            this.message = message;
        }
        public DateTime dt { get; private set; }
        public String logName { get; private set; }
        public LogLevel level { get; private set; }
        public String message { get; private set; }
    }
    public delegate void LogEventHandler(object sender, LogEventArgs e);
}