using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using OpenWealth;

namespace DevTools.LogForm
{
    public partial class LogForm : Form, IPlugin, IDescription
    {
        static ILog l = Core.GetLogger(typeof(LogForm).FullName);

        public LogForm()
        {
            InitializeComponent();

            l.LogEvent += new LogEventHandler(l_LogEvent);
            l.Debug("LogForm()");
        }

        public void Init()
        {
            l.Debug("Инициирую");

            mainForm = Core.GetGlobal("MainForm") as Form;
            menuStrip = Core.GetGlobal("MainMenu") as MenuStrip;

            this.MdiParent = mainForm;

            ToolStripMenuItem toolStripMenuItem = new ToolStripMenuItem("Логи");
            ToolStripDropDown toolStripDropDown = new ToolStripDropDown();
            ToolStripItem item1 = new ToolStripButton("Отобразить лог");
            item1.Click += new EventHandler(item1_Click);
            toolStripDropDown.Items.Add(item1);
            toolStripMenuItem.DropDown = toolStripDropDown;
            menuStrip.Items.Add(toolStripMenuItem);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        void l_LogEvent(object sender, LogEventArgs e)
        {
            listBox1.Items.Insert(0, e.dt.ToString("mm:ss.ffff ")+ e.level.ToString() + " " + e.logName + " " + e.message + Environment.NewLine);
            if (listBox1.Items.Count > 400)
                listBox1.Items.RemoveAt(200);
        }


        #region реализация IPlugin

        Form mainForm;
        MenuStrip menuStrip;

        void item1_Click(object sender, EventArgs e)
        {
            Show();
        }

        #endregion реализация IPlugin

        #region IDescription
        public string Description { get { return "Генератор случайных тиков"; } }
        public string URL { get { return "www.OpenWealth.ru"; } }
        #endregion IDescription

        private void button2_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(listBox1.Items.ToString());
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
                l.LogEvent += new LogEventHandler(l_LogEvent);
            else
                l.LogEvent -= l_LogEvent;
        }
    }
}
