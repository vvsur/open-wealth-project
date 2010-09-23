using System;
using System.Windows.Forms;

namespace OpenWealth.RndDataSource
{
    public partial class TimerSettingForm : Form
    {
        System.Timers.Timer timer;

        public TimerSettingForm(System.Timers.Timer timer)
        {
            InitializeComponent();

            this.timer = timer;
            this.MdiParent = Core.GetGlobal("MainForm") as Form;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer.Interval = (int)numericUpDown1.Value;
            timer.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
        }
    }
}
