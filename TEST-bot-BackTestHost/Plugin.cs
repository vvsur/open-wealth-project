using System;

using OpenWealth;

namespace TEST_bot_BackTestHost
{
    public class Plugin : IPlugin
    {
        static ILog l = Core.GetLogger(typeof(Plugin).FullName);

        IInterface interf;
        public void Init() 
        {
            l.Debug("Инициирую TEST_bot_BackTestHost.Plugin");
            interf = Core.GetGlobal("Interface") as IInterface;

            if (interf != null)
            {
                interf.AddMenuItem("Роботы", "Бэктестинг", null, menu_Click);
            }
        }

        void menu_Click(object sender, EventArgs e)
        {
            if (interf == null)
            {
                l.Error("Не найден объект, реализующий интерфейс");
                return;
            }

            BacktestForm bf = new BacktestForm();
            bf.MdiParent = interf.GetMainForm();
            bf.Show();

            Core.Data.GetMarket("test4");
        }


        public void Stop() { }
    }
}
