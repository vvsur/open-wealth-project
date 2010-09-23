using System;
using System.Windows.Forms;

namespace OpenWealth.RndDataSource
{
    public class RndDataProvider : IPlugin, IDataProvider, IDescription
    {
        static ILog l = Core.GetLogger(typeof(RndDataProvider).FullName);

        IBars AAA, BBB;
        double aaa, bbb;
        int m_TickNum = 0;
        Random rnd = new Random();
        System.Timers.Timer timer;

        Form mainForm;
        MenuStrip menuStrip;
        IData data;

        #region реализация IPlugin

        public void Init()
        {
            l.Debug("Инициирую RndDataSource");
            data = Core.GetGlobal("data") as IData;
            mainForm = Core.GetGlobal("MainForm") as Form;
            menuStrip = Core.GetGlobal("MainMenu") as MenuStrip;

            if (data != null)
            {
                AAA = data.GetBars("AAA", ScaleEnum.tick, 1);
                BBB = data.GetBars("BBB", ScaleEnum.tick, 1);
                aaa = 100;
                bbb = 200;
                timer = new System.Timers.Timer(100);
                timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);

                if ((mainForm == null) || (menuStrip == null))
                    timer.Enabled = true;
            }
            else
            {
                l.Fatal("Не найден модуль data");
                throw new Exception("Не найден модуль data");
            }

            ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem("Генератор тиков");
            ToolStripDropDown toolStripDropDown = new ToolStripDropDown();
            ToolStripItem item1 = new ToolStripButton("Включить генератор");
            ToolStripItem item2 = new ToolStripButton("Создать тик в ручную");
            ToolStripItem item3 = new ToolStripButton("Настройки генератора");
            item1.Click += new EventHandler(item1_Click);
            item2.Click += new EventHandler(item2_Click);
            item3.Click += new EventHandler(item3_Click);
            toolStripDropDown.Items.Add(item1);
            toolStripDropDown.Items.Add(item2);
            toolStripDropDown.Items.Add(item3);
            toolStripMenuItem.DropDown = toolStripDropDown;
            menuStrip.Items.Add(toolStripMenuItem);
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
            AAA.Add(this, new OpenWealth.Simple.Tick(DateTime.Now, ++m_TickNum, aaa, rnd.Next(20)));
            BBB.Add(this, new OpenWealth.Simple.Tick(DateTime.Now, ++m_TickNum, bbb, rnd.Next(20)));
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
