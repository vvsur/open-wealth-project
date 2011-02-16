using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;

namespace OpenWealth
{
    /// <summary>
    /// Основной класс ядра
    /// Содержит только статические методы и свойства
    /// логику см. в _readme.txt
    /// </summary>
    public static class Core
    {        
        #region Init Stop
        // ***************************
        // Данный регион служит для инициализации ядра
        // в нем загружаются плагины
        // все методы приватные, вызываются из статического конструктора
        // ***************************

        /// <summary>
        /// Статический конструктор, инициирующий Core
        /// </summary>
        static Core()
        {   
            l.Debug("Статический конструктор Core.");
            try
            {
                string appFileName = System.Reflection.Assembly.GetEntryAssembly().Location;
                string coreFileName = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string appFilePath = System.IO.Path.GetDirectoryName(appFileName);
                string pluginsPath1 = System.IO.Path.Combine(appFilePath, "OpenWealth");
                string pluginsPath2 = System.IO.Path.Combine(appFilePath, "Plugins");

                SetGlobal("AppPath", appFilePath);
                if (appFileName != coreFileName)
                    LoadPlugin(coreFileName);
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
                    else
                    {
                        // нет ни одной папки с плагинами. от безысходности загружаю все DLL из папки программы
                        SetGlobal("PluginsPath", appFilePath);
                        foreach (string file in Directory.GetFiles(appFilePath, "*.dll", SearchOption.AllDirectories))
                            if (file != appFileName)
                                LoadPlugin(file);
                    }

                InitPlugins();
            }
            catch(Exception e)
            {
                l.Error("Ошибка в static Core(). Жаль что узнать о ней никто не сможет т.к. логгер наверняка не загрузился", e);
            }
        }

        /// <summary>
        /// Вызываю метод Init у всех загруженных плагинов
        /// </summary>
        static void InitPlugins()
        {
            l.Debug("Получаю список ранее загруженных плагинов");
            List<IPlugin> plugins = GetGlobal("plugins") as List<IPlugin>;
            if (plugins!=null)
                lock (plugins)
                {
                    l.Debug("Вызываю Init у каждого метода");
                    foreach (IPlugin p in plugins)
                        p.Init();
                }
        }

        /// <summary>
        /// Загрузка (создание объекта) плагина определенного типа
        /// </summary>
        /// <param name="type">тип загружаемого плагина</param>
        static void LoadPlugin(Type type)
        {
            l.Debug("Получаю список ранее загруженных плагинов");
            List<IPlugin> plugins = GetGlobal("plugins") as List<IPlugin>;
            if (plugins == null)
            {
                plugins = new List<IPlugin>();
                SetGlobal("plugins", plugins);
            }
            try
            {
                lock (plugins)
                {
                    l.Debug("Пытаюсь найти данный тип среди ранее созданных объектов");
                    bool find = false;
                    foreach (IPlugin p in plugins)
                        if (p.GetType() == type)
                        {
                            l.Info("Попытка повторного создания плагина " + type);
                            find = true;
                            break;
                        }
                    if (!find)
                    {
                        IPlugin newPlugin = Activator.CreateInstance(type) as IPlugin;
                        plugins.Add(newPlugin);
                        l.Debug("Загрузил плагин " + type);
                    }
                }
            }
            catch (Exception ex)
            {
                l.Error("Исключение при создании плагина " + type, ex);
            }
        }

        /// <summary>
        /// Загрузка всех плагинов из файла
        /// </summary>
        /// <param name="file">имя файла, из которого требуется загрузить плагины</param>
        static void LoadPlugin(string file)
        {
            l.Debug("загружаю список плагинов из файла " + file);
            try
            {
                Assembly assembly = Assembly.LoadFile(file);

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

        /// <summary>
        /// Остановка всех плагинов, вызывается при завершении программы
        /// </summary>
        public static void Stop()
        {
            l.Debug("Останавливаю плагины");
            try
            {
                List<IPlugin> plugins = GetGlobal("plugins") as List<IPlugin>;
                if (plugins == null)
                    return;
                lock (plugins)
                {
                    foreach (IPlugin p in plugins)
                        p.Stop();
                }
            }
            catch (Exception ex)
            {
                l.Error("Исключение при остановке плагинов", ex);
            }
        }

        #endregion Init

        #region SetGlobal GetGlobal
        // **********************
        // Список глобальных переменных, которые используются сейчас
        // SettingsHost - реализация ISettingsHost
        // Data - реализация IDataManager
        // AppPath - строка, содержащая путь к каталогу с запущенной программой
        // PluginsPath - строка, содержащая путь к каталогу с плагинами
        // **********************

        static ReaderWriterLock Lock = new System.Threading.ReaderWriterLock();
        static Dictionary<string, object> global = new Dictionary<string, object>();

        /// <summary>
        /// Сохранить объект в глобальный кэш
        /// </summary>
        /// <param name="key">название объекта в кэше</param>
        /// <param name="obj">сохраняемый объект</param>
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

        /// <summary>
        /// Получение объекта из глобального кэша
        /// </summary>
        /// <param name="key">имя объекта в кэше</param>
        /// <returns>объект из кэша или null, если объект не найден</returns>
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
                result = null;
            }
            return result;
        }

        static IDataManager _data = null;
        public static IDataManager Data
        {
            get
            {
                Lock.AcquireReaderLock(1000);
                try
                {
                    if (_data != null)
                        return _data;
                    else
                    {
                        LockCookie lc = Lock.UpgradeToWriterLock(1000);
                        try
                        {
                            _data = Core.GetGlobal("data") as IDataManager;
                        }
                        finally
                        {
                            Lock.DowngradeFromWriterLock(ref lc);
                        }
                        l.Info("Попытка получить Data, в то время как он null");
                        return _data;
                    }
                }
                finally
                {
                    Lock.ReleaseLock();
                }
            }
            set
            {
                Lock.AcquireWriterLock(1000);
                try
                {
                    _data = value;
                    Core.SetGlobal("data", value);
                }
                finally
                {
                    Lock.ReleaseWriterLock();
                }
            }
        }

        static ISettingsHost _settingsHost = null;
        public static ISettingsHost SettingsHost
        {
            get
            {
                Lock.AcquireReaderLock(1000);
                try
                {
                    if (_settingsHost != null)
                        return _settingsHost;
                    else
                    {
                        LockCookie lc = Lock.UpgradeToWriterLock(1000);
                        try
                        {
                            _settingsHost = Core.GetGlobal("SettingsHost") as ISettingsHost;
                        }
                        finally
                        {
                            Lock.DowngradeFromWriterLock(ref lc);
                        }
                        l.Info("Попытка получить SettingsHost, в то время как он null");
                        return _settingsHost;
                    }
                }
                finally
                {
                    Lock.ReleaseLock();
                }
            }
            set
            {
                Lock.AcquireWriterLock(1000);
                try
                {
                    _settingsHost = value;
                    Core.SetGlobal("SettingsHost", value);
                }
                finally
                {
                    Lock.ReleaseWriterLock();
                }
            }
        }

        #endregion

        #region Log
        // В лог надо писать ещё до загрузки плагинов, поэтому в качестве logManager использую Simple.Log
        // в Simple.Log есть возможность подписаться на сообщения для логирования

        static ILogManager logManager = new Simple.Log(null);
        static ILog l = logManager.GetLogger("Core");

        /// <summary>
        /// Получить логгер реализующий ILog
        /// </summary>
        /// <param name="name">Имя того, что предстоит логгировать. Например, название типа. Выводится в лог.</param>
        /// <returns>Логгер реализующий ILog</returns>
        static public ILog GetLogger(String name)
        {
            return logManager.GetLogger(name);
        }
        static public ILog GetLogger(Type type)
        {
            return logManager.GetLogger(type.FullName);
        }

        #endregion Log
    }
}
