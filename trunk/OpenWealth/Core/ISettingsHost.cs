
namespace OpenWealth
{
    public interface ISettingsHost
    {
        bool ContainsKey(string key);

        string Get(string key, string defaultValue);
        void Set(string key, string value);

        bool Get(string key, bool defaultValue);
        void Set(string key, bool value);

        int Get(string key, int defaultValue);
        void Set(string key, int value);

        void DeleteKey(string key);
    }
}
