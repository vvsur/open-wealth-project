using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;

namespace OpenWealth
{
    public sealed class Core
    {
        static System.Threading.ReaderWriterLock Lock = new System.Threading.ReaderWriterLock();

        #region Init
        // публичный конструктор запрещаем. Ядро имеет только статические методы
        Core() { }

        /// <summary>
        /// Статический конструктор, инициирующий Core
        /// </summary>
        static Core()
        {   
            l.Debug("Статический конструктор Core.");
            string appFileName = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string appFilePath = System.IO.Path.GetDirectoryName(appFileName);
            string pluginsPath1 = System.IO.Path.Combine(appFilePath,"OpenWealth");
            string pluginsPath2 = System.IO.Path.Combine(appFilePath,"Plugins");

            SetGlobal("AppPath", appFilePath);

            LoadPlugin(appFileName);

            if (Directory.Exists(pluginsPath1))
            {
                SetGlobal("PluginsPath", pluginsPath1);
                foreach (string file in Directory.GetFiles(pluginsPath1, "*.dll", SearchOption.AllDirectories))
                    LoadPlugin(file);
            }
            else 
                if (Directory.Exists(pluginsPath2))
                {
                    SetGlobal("PluginsPath", pluginsPath2);
                    foreach (string file in Directory.GetFiles(pluginsPath2, "*.dll", SearchOption.AllDirectories))
                        LoadPlugin(file);
                }

            InitPlugins();
        }

        static void InitPlugins()
        {
            l.Debug("Получаю список ранее загруженных плагинов");
            List<IPlugin> plugins = GetGlobal("plugins") as List<IPlugin>;
            foreach (IPlugin p in plugins)
                p.Init();
        }

        static void LoadPlugin(Type type)
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

        static void LoadPlugin(string file)
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

        #endregion Init

        #region SetGlobal GetGlobal

        static Dictionary<string, object> global = new Dictionary<string, object>();
        public static void SetGlobal(string key, object obj)
        {
            l.Debug("Сохранил в global " + key);
            Lock.AcquireWriterLock(1000);
            try
            {
                global[key.ToLower()] = obj;
            }
            finally
            {
                Lock.ReleaseWriterLock();
            }
        }
        public static object GetGlobal(string key)
        {
            object result;
            bool getResult;
            Lock.AcquireReaderLock(1000);
            try
            {
                getResult = global.TryGetValue(key.ToLower(), out result);
            }
            finally
            {
                Lock.ReleaseReaderLock();
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
