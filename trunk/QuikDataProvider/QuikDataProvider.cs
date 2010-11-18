using System;
using OpenWealth;

namespace OpenWealth.QuikDataProvider
{
    public class QuikDataProvider : IPlugin, IDataProvider, IDescription
    {
        static ILog l = Core.GetLogger(typeof(QuikDataProvider).FullName);

        #region реализация IPlugin

        DDE.DDEServer ddeServer;

        public void Init()
        {
            l.Debug("Init();");
            if (ddeServer == null)
            {
                ddeServer = new DDE.DDEServer("OpenWealth");
                ddeServer.Register();
                l.Debug("DDE сервер зарегистрирован");
            }
            else
                l.Error("Повторная инициализация плагина");
        }

        #endregion реализация IPlugin

        #region IDataProvider

        public bool isHistoryProvider { get { return false; } }
        public bool isRealTimeProvider { get { return true; } }
        public bool GetData(ISymbol symbol, IScale scale, DateTime startDate, DateTime endDate, int maxBars, bool includePartialBar)
        {
            l.Error("Для провайдера не реализуещего isHistoryProvider данный метод вызываться не должен");
            return false;
        }
        #endregion IDataProvider

        #region IDescription
        public string Name { get { return "QuikDataProvider"; } }
        public string Description { get { return "Получение торговых данных из Quik"; } }
        public string URL { get { return "www.OpenWealth.ru"; } }
        #endregion IDescription
    }
}
