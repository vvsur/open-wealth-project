using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;

namespace OpenWealth
{
    public sealed class Core
    {
        // публичный конструктор запрещаем. Ядро имеет только статические методы
        Core() { }

        static System.Threading.ReaderWriterLockSlim Lock = new System.Threading.ReaderWriterLockSlim();

        #region Init

        static bool m_SingleInit = true;
        static public bool SingleInit()
        {
            Lock.EnterWriteLock();
            try
            {
                l.Debug("Core.SingleInit");
                bool result = m_SingleInit;
                m_SingleInit = false;
                return result;
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }

        static public void Init(string AppPath)
        {
            l.Debug("инициирую Core");
            SetGlobal("AppPath", AppPath);
        }

        static public void InitPlugin()
        {
            l.Debug("Получаю список ранее загруженных плагинов");
            List<IPlugin> plugins = GetGlobal("plugins") as List<IPlugin>;
            foreach (IPlugin p in plugins)
                p.Init();
        }

        static public void LoadPlugin(Type type)
        {
            l.Debug("Получаю список ранее загруженных плагинов");
            List<IPlugin> plugins = GetGlobal("plugins") as List<IPlugin>;
            if (plugins == null)
            {
                plugins = new List<IPlugin>();
                SetGlobal("plugins", plugins);
            }
            l.Debug("Инициирую плагин");
            try
            {
                // пытаюсь найти данный тип среди ранее созданных объектов
                bool find = false;
                foreach (IPlugin p in plugins)
                    if (p.GetType() == type)
                    {
                        find = true;
                        break;
                    }
                if (!find)
                {
                    IPlugin newPlugin = Activator.CreateInstance(type) as IPlugin;
                    plugins.Add(newPlugin);
                    l.Debug("Cоздал инстанс плагина " + type);
                }
                else
                {
                    l.Info("Попытка повторного создания плагина " + type);
                }
            }
            catch (Exception ex)
            {
                l.Error("Исключение при создании плагина " + type, ex);
            }
        }

        static public void LoadPlugin(string file)
        {
            // загружаю список плагинов из файла
            try
            {
                Assembly assembly;
                assembly = Assembly.LoadFile(file);

                foreach (Type type in assembly.GetTypes())
                {
                    if (!type.IsClass || type.IsNotPublic) continue;
                    Type[] interfaces = type.GetInterfaces();
                    if (((IList<Type>)interfaces).Contains(typeof(IPlugin)))
                        LoadPlugin(type);
                }
            }
            catch (Exception ex)
            {
                l.Error("Исключение при загрузке плагинов из файла " + file, ex);
            }
        }

        static public void LoadPlugin()
        {            
            string folder = GetGlobal("AppPath") as string;
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            if (folder != null)
                foreach (string file in Directory.GetFiles(folder, "*.dll", SearchOption.AllDirectories))
                    LoadPlugin(file);
        }

        #endregion Init

        #region SetGlobal GetGlobal

        static Dictionary<string, object> global = new Dictionary<string, object>();
        public static void SetGlobal(string key, object obj)
        {
            l.Debug("Сохранил в global " + key);
            Lock.EnterWriteLock();
            try
            {
                global[key.ToLower()] = obj;
            }
            finally
            {
                Lock.ExitWriteLock();
            }
        }
        public static object GetGlobal(string key)
        {
            object result;
            bool getResult;
            Lock.EnterReadLock();
            try
            {
                getResult = global.TryGetValue(key.ToLower(), out result);
            }
            finally
            {
                Lock.ExitReadLock();
            }
            if (getResult)
            {
                l.Debug("Получил из global " + key);
            }
            else
            {
                l.Info("Не смог получить из global " + key);
            }
            return result;
        }

        #endregion

        #region Log

        static ILogManager logManager = new Simple.Log(null);
        static ILog l = logManager.GetLogger("Core");

        static public ILog GetLogger(String name)
        {
            return logManager.GetLogger(name);
        }
        #endregion Log
    }
}
