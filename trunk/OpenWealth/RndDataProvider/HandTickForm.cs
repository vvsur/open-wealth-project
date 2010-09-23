using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace OpenWealth.RndDataSource
{
    public partial class HandTickForm : Form
    {
        public HandTickForm(IPlugin plugin)
        {
            InitializeComponent();

            this.plugin = plugin;
            this.data = Core.GetGlobal("data") as IData;
            this.MdiParent = Core.GetGlobal("MainForm") as Form;
        }

        static ILog l = Core.GetLogger(typeof(HandTickForm).FullName);
        IPlugin plugin;
        IData data;

        private void button1_Click(object sender, EventArgs e)
        {
            if (data != null)
            {
                IBar tick = null;
                try
                {
                    int number = int.Parse(textBox2.Text);
                    double price = double.Parse(textBox3.Text);
                    int volume = int.Parse(textBox4.Text);

                    tick = new Simple.Tick(dateTimePicker1.Value, number, price, volume);
                }
                catch
                {
                    MessageBox.Show("Ошибка парсинга числовых значений");
                }
                if (tick != null)
                {
                    IBars bars = data.GetBars(textBox1.Text, ScaleEnum.tick, 1);
                    bars.Add(plugin, tick);
                }
            }
            else
                l.Error("Не найден модуль data");
        }
    }
}
