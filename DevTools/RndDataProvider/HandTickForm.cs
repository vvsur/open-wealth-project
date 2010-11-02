using System;
using System.Windows.Forms;

namespace OpenWealth.RndDataSource
{
    public partial class HandTickForm : Form
    {
        public HandTickForm(IDataProvider plugin)
        {
            InitializeComponent();

            this.plugin = plugin;
            this.data = Core.GetGlobal("data") as IDataManager;
            this.MdiParent = Core.GetGlobal("MainForm") as Form;
        }

        static ILog l = Core.GetLogger(typeof(HandTickForm).FullName);
        IDataProvider plugin;
        IDataManager data;

        private void button1_Click(object sender, EventArgs e)
        {
            if (data != null)
            {
                IBar tick = null;
                try
                {
                    int number = int.Parse(txtNumber.Text);
                    double price = double.Parse(txtPrice.Text);
                    int volume = int.Parse(txtVolume.Text);

                    tick = new Simple.Tick(dateTimePicker1.Value, number, price, volume);
                }
                catch
                {
                    MessageBox.Show("Ошибка парсинга числовых значений");
                }
                if (tick != null)
                {
                    IBars bars = data.GetBars(txtMarket.Text, txtSymbol.Text, ScaleEnum.tick, 1);
                    bars.Add(plugin, tick);
                }
            }
            else
                l.Error("Не найден модуль data");
        }
    }
}
