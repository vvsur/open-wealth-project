using System;
using System.Windows.Forms;

namespace OpenWealth.Interface4EXE
{
    static class Program
    {
        private static readonly ILog l = Core.GetLogger(typeof(Program).FullName);
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);

            IInterface i = Core.GetGlobal("Interface") as IInterface;
            if (i != null)
            {
                Form f = i.GetMainForm();
                if (f != null)
                {
                    l.Debug("start Application.Run");                    
                    Application.Run(f);
                    l.Debug("stop Application.Run");
                }
                else
                    l.Error("IInterface.GetMainForm == null");
            }
            else
                l.Error("GetGlobal(\"Interface\") == null");
        }
    }
}