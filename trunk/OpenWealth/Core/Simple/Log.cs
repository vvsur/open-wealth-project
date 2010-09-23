using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenWealth.Simple
{
    public class Log : ILog, ILogManager
    {
        private string name;
        private int level;

        public void Debug(object message)
        {
            if (IsDebugEnabled)
            {
                WriteEntry(message.ToString(), "Debug");
            }
        }
        public void Info(object message)
        {
            if (IsInfoEnabled)
            {
                WriteEntry(message.ToString(), "Info");
            }
        }
        public void Warn(object message)
        {
            if (IsWarnEnabled)
            {
                WriteEntry(message.ToString(), "Warn");
            }
        }
        public void Error(object message)
        {
            if (IsErrorEnabled)
            {
                WriteEntry(message.ToString(), "Error");
            }
        }
        public void Error(object message, Exception exception)
        {
            if (IsErrorEnabled)
            {
                WriteEntry(message.ToString() + " Exception: " + exception.ToString(), "Error");
            }
        }
        public void Fatal(object message)
        {
            if (IsFatalEnabled)
            {
                WriteEntry(message.ToString(), "Fatal");
            }
        }

        public bool IsDebugEnabled { get { return (level == 0); } }
        public bool IsInfoEnabled  { get { return (level <= 1); } }
        public bool IsWarnEnabled  { get { return (level <= 2); } }
        public bool IsErrorEnabled { get { return (level <= 3); } }
        public bool IsFatalEnabled { get { return (level <= 4); } }

        public void SetLevel(LogLevel level)
        {
            System.Threading.Interlocked.Exchange(ref this.level, (int)level);
        }

        public ILog GetLogger(string name)
        {
            //TODO проверку, что такого логгера ещё нет
            return new Log(name);
        }

        internal Log(string name)
        {
            this.name = name;
        }

        private void WriteEntry(string message, string level)
        {
            //TODO Вызывающий метод
            System.Diagnostics.Trace.WriteLine(
                    string.Format("{0},{1},{2},{3},{4}",
                                  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                  System.Threading.Thread.CurrentThread.ManagedThreadId.ToString() , //TODO номер потока в шеснадцатиричном виде
                                  level,
                                  name,
                                  message));
            // TODO Описать и вызывать событие
        }
    }
}


