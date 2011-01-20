using System;
using System.Windows.Forms;

namespace OpenWealth.RndDataSource
{
    public class RndDataProvider : IPlugin, IDataProvider, IDescription
    {
        static ILog l = Core.GetLogger(typeof(RndDataProvider).FullName);

        IInterface interf; 

        #region реализация IPlugin

        public void Init()
        {
            l.Debug("Инициирую RndDataSource");
            interf = Core.GetGlobal("Interface") as IInterface;

            if (interf != null)
            {
                interf.AddMenuItem("Данные", "Тестовые тики", "Генератор ровных баров", item1_Click);
                interf.AddMenuItem("Данные", "Тестовые тики", "Отправить тик в ручную", item2_Click);
                interf.AddMenuItem("Данные", "Тестовые тики", "Генератор случайных тиков", item3_Click);
            }
        }

        public void Stop() { }

        void item1_Click(object sender, EventArgs e)
        {
            LineBarForm f = new LineBarForm(this);
            f.Show();
        }

        void item2_Click(object sender, EventArgs e)
        {
            HandTickForm f = new HandTickForm(this);
            f.Show();
        }

        void item3_Click(object sender, EventArgs e)
        {
            TimerSettingForm f = new TimerSettingForm(this);
            f.Show();
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
        public string Name { get { return "RndDataSource"; } }
        public string Description { get { return "Генератор случайных тиков"; } }
        public string URL { get { return "www.OpenWealth.ru"; } }
        #endregion IDescription


    }
}
