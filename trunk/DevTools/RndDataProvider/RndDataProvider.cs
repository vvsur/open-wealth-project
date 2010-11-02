using System;
using System.Windows.Forms;

namespace OpenWealth.RndDataSource
{
    public class RndDataProvider : IPlugin, IDataProvider, IDescription
    {
        static ILog l = Core.GetLogger(typeof(RndDataProvider).FullName);

        double aaa, bbb;
        int m_TickNum = 0;
        Random rnd = new Random();
        System.Timers.Timer timer;
        IInterface interf; 

        IDataManager data;

        #region реализация IPlugin

        public void Init()
        {
            l.Debug("Инициирую RndDataSource");
            data = Core.GetGlobal("data") as IDataManager;
            interf = Core.GetGlobal("Interface") as IInterface;

            if (interf == null)
            {
                timer = new System.Timers.Timer(100);
                timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            }
            else
            {
                interf.AddMenuItem("Тестовые тики", "Включить генератор", null, item1_Click);
                interf.AddMenuItem("Тестовые тики", "Отправить тик в ручную", null, item2_Click);
                interf.AddMenuItem("Тестовые тики", "Настройки генератора тиков", null, item3_Click);
            }
        }

        void item2_Click(object sender, EventArgs e)
        {
            HandTickForm f = new HandTickForm(this);
            f.Show();
        }

        void item3_Click(object sender, EventArgs e)
        {
            TimerSettingForm f = new TimerSettingForm(timer);
            f.Show();
        }

        void item1_Click(object sender, EventArgs e)
        {
            if (timer.Enabled)
            {
                (sender as ToolStripItem).Text = "Включить генератор";
                timer.Enabled = false;
            }
            else
            {
                (sender as ToolStripItem).Text = "Выключить генератор";
                timer.Enabled = true;
            }
        }

        #endregion реализация IPlugin

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            aaa += rnd.NextDouble() - 0.5;
            bbb += 2*rnd.NextDouble() - 1;

            l.Debug("RndDataSource создаю и добавляю новые бары. m_TickNum=" + m_TickNum);

            data.GetBars("TestMarket", "AAA", ScaleEnum.tick, 1).Add(this, new OpenWealth.Simple.Tick(DateTime.Now, ++m_TickNum, aaa, rnd.Next(20)));
            data.GetBars("TestMarket", "BBB", ScaleEnum.tick, 1).Add(this, new OpenWealth.Simple.Tick(DateTime.Now, ++m_TickNum, bbb, rnd.Next(20)));
        }

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
