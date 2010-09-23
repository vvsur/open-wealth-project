using System;
using System.Windows.Forms;

namespace OpenWealth.Interface
{
    public partial class MainForm4WL : Form, IPlugin
    {
        public MainForm4WL()
        {
            InitializeComponent();

            Core.SetGlobal("MainForm", this);
            Core.SetGlobal("MainMenu", this.menuStrip);

            // TODO Указать в качестве родителя главное окно WL
        }

        public void Init()
        {
            //Show();
        }

        public bool isDataSource { get { return false; } }
        public string name { get { return "Главная форма при интеграции с WL"; } }

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
