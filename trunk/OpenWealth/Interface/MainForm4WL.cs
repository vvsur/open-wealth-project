using System;
using System.Windows.Forms;

namespace OpenWealth.Interface
{
    public partial class MainForm4WL : Form, IPlugin, IDescription
    {
        public MainForm4WL()
        {
            InitializeComponent();

            Core.SetGlobal("MainForm", this);
            Core.SetGlobal("MainMenu", this.menuStrip);

            // TODO Указать в качестве родителя главное окно WL
            Name = "Главная форма";
        }

        public void Init()
        {
            //Show();
        }

        #region Реализация IDescription

        //public string Name { get { return "Главная форма"; } }
        public string Description { get { return "Главная форма приложения"; } }
        public string URL { get { return "www.OpenWealth.ru"; } }

        #endregion Реализация IDescription

        private void MainForm4WL_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
                Hide();
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void MainForm4WL_FormClosing(object sender, FormClosingEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }
    }
}
