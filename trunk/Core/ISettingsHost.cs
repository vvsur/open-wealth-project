using System;

namespace OpenWealth
{
    /// <summary>
    /// Интерфейс, описывающий способ хранение настроек в программе.
    /// Простейная реализация, сохраняющая настройки в xml описана в OpenWealth.Simple.SettingsHost
    /// но никто не запрешает в будущем завернуть это в БД или ещё куда нибудь
    /// </summary>
    public interface ISettingsHost
    {
        bool ContainsKey(string key);

        string Get(string key, string defaultValue);
        void Set(string key, string value);

        bool Get(string key, bool defaultValue);
        void Set(string key, bool value);

        int Get(string key, int defaultValue);
        void Set(string key, int value);

        double Get(string key, double defaultValue);
        void Set(string key, double value);

        DateTime Get(string key, DateTime defaultValue);
        void Set(string key, DateTime value);

        void DeleteKey(string key);
    }
}
