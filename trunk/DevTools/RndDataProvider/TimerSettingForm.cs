using System;
using System.Windows.Forms;

using OpenWealth;

namespace OpenWealth.RndDataSource
{
    public partial class TimerSettingForm : Form
    {
        static ILog l = Core.GetLogger(typeof(TimerSettingForm).FullName);

        System.Timers.Timer timer;
        IDataManager data;
        IDataProvider dataProvider;

        public TimerSettingForm(IDataProvider dataProvider)
        {
            InitializeComponent();

            this.dataProvider = dataProvider;

            IInterface interf = Core.GetGlobal("Interface") as IInterface;
            if (interf!=null)
            {
                if (interf.GetMainForm() != null)
                    this.MdiParent = interf.GetMainForm();
            }

            data = Core.GetGlobal("data") as IDataManager;
            if (data == null)
                l.Error("data == null");

            timer = new System.Timers.Timer(500);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
        }

        double price = 100;
        static int tickNum = 1;
        Random rnd = new Random();

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (rnd)
            {
                price += rnd.NextDouble() - 0.5;
                l.Debug("RndDataSource создаю и добавляю новые бары. m_TickNum=" + tickNum);
                data.GetBars(data.GetSymbol(textBox1.Text), data.GetScale(ScaleEnum.tick, 1)).Add(dataProvider, new OpenWealth.Simple.Tick(DateTime.Now, tickNum++, price, rnd.Next(15)+5));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer.Interval = (int)numericUpDown1.Value;
            timer.Enabled = true;
            SetEnable();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
            SetEnable();
        }

        void SetEnable()
        {
            button1.Enabled = !timer.Enabled;
            numericUpDown1.Enabled = !timer.Enabled;
            textBox1.Enabled = !timer.Enabled;
            button2.Enabled = timer.Enabled;
        }
    }
}
