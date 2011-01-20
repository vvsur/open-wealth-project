using System;
using System.Windows.Forms;

namespace OpenWealth.RndDataSource
{
    public partial class HandTickForm : Form
    {
        static ILog l = Core.GetLogger(typeof(HandTickForm).FullName);
        IDataProvider plugin;

        public HandTickForm(IDataProvider plugin)
        {
            InitializeComponent();

            this.plugin = plugin;

            IInterface interf = Core.GetGlobal("Interface") as IInterface;
            if (interf != null)
                if (interf.GetMainForm() != null)
                    this.MdiParent = interf.GetMainForm();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IDataManager data = Core.GetGlobal("data") as IDataManager;
            if (data != null)
            {
                IBar tick = null;
                try
                {
                    int number = int.Parse(txtNumber.Text);
                    float price = float.Parse(txtPrice.Text);
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
