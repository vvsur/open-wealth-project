
namespace OpenWealth.WLProvider
{
    /// <summary>
    /// Данный класс ни на что в поведении не влияет, лишь позволяет засвититься IDescription среди загруженных модулей
    /// </summary>
    public class Plugin : IPlugin, IDescription
    {
        #region Реализация IPlugin

        public void Init() { }

        #endregion Реализация IPlugin

        #region Реализация IDescription

        public string Name { get { return "Главная форма"; } }
        public string Description { get { return "Главная форма приложения"; } }
        public string URL { get { return "www.OpenWealth.ru"; } }

        #endregion Реализация IDescription

    }
}
