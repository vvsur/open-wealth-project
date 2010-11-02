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

            addListItem = new AddListItem(AddListItemMethod);
            l.LogEvent += new LogEventHandler(l_LogEvent);
            l.Debug("LogForm()");
        }

        IInterface interf;
        public void Init()
        {
            l.Debug("Инициирую");

            interf = Core.GetGlobal("Interface") as IInterface;

            if (interf != null)
            {
                interf.AddMenuItem("Логи", "Отобразить логи", null, item1_Click);
            }       
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
        }

        public void AddListItemMethod(String msg)
        {
            listBox1.Items.Insert(0, msg);
            if (listBox1.Items.Count > 400)
                listBox1.Items.RemoveAt(200);
        }
        
        public delegate void AddListItem(String msg);
        public AddListItem addListItem;

        void l_LogEvent(object sender, LogEventArgs e)
        {
            string msg = e.dt.ToString("mm:ss.ffff ") + e.level.ToString() + " " + e.logName + " " + e.message + Environment.NewLine;
            if (this.InvokeRequired)
                this.Invoke(addListItem, new Object[] { msg });
            else
                AddListItemMethod(msg);
        }

        void item1_Click(object sender, EventArgs e)
        {
            if (interf==null)
                return;
            this.MdiParent = interf.GetMainForm();
            Show();
        }

        #region IDescription
        public string Description { get { return "Генератор случайных тиков"; } }
        public string URL { get { return "www.OpenWealth.ru"; } }
        #endregion IDescription

        private void button2_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string s in listBox1.Items)
                sb.AppendLine(s);
            Clipboard.SetText(sb.ToString());
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
