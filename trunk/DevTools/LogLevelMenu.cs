/*
 * Добавляет в меню  "Логи" подменю "Уровень логирования" через которое можно выбрать
 * требуемый уровень логирования
 */

using System;
using System.Windows.Forms;

using OpenWealth;

namespace DevTools
{
    public class LogLevelMenu : IPlugin
    {
        static ILog l = Core.GetLogger(typeof(LogLevelMenu).FullName);

        public LogLevelMenu()
        {
            ILogManager lm = l as ILogManager;
            if (lm != null)
                lm.SetLevel(LogLevel.Debug);
            l.Debug("LogLevelMenu()");
        }

        IInterface interf;
        public void Init()
        {
            l.Debug("Инициирую");

            interf = Core.GetGlobal("Interface") as IInterface;

            if (interf != null)
            {
                interf.AddMenuItem("Логи", "Уровень логирования", "Debug", menuClick);
                interf.AddMenuItem("Логи", "Уровень логирования", "Info", menuClick);
                interf.AddMenuItem("Логи", "Уровень логирования", "Warn", menuClick);
                interf.AddMenuItem("Логи", "Уровень логирования", "Error", menuClick);
                interf.AddMenuItem("Логи", "Уровень логирования", "Fatal", menuClick);
                interf.AddMenuItem("Логи", "Уровень логирования", "NoLog", menuClick);
            }
            else
                l.Error("interf == null");
        }

        public void Stop() { }

        void menuClick(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;

            if (menuItem == null)
            {
                l.Error("menuItem == null");
                return;
            }

            ILogManager lm = l as ILogManager;
            if (lm == null)
            {
                l.Error("lm == null");
                return;
            }

            if (menuItem.Text=="Debug")
                lm.SetLevel(LogLevel.Debug);
            else
            if (menuItem.Text=="Info")
                lm.SetLevel(LogLevel.Info);
            else
            if (menuItem.Text=="Warn")
                lm.SetLevel(LogLevel.Warn);
            else
            if (menuItem.Text=="Error")
                lm.SetLevel(LogLevel.Error);
            else
            if (menuItem.Text=="Fatal")
                lm.SetLevel(LogLevel.Fatal);
            else
            if (menuItem.Text=="NoLog")
                lm.SetLevel(LogLevel.NoLog);
            else
                l.Error("menuItem.Text=="+menuItem.Text);
        }

    }
}
