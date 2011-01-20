using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using OpenWealth;

namespace OpenWealth.RndDataSource
{
    public partial class LineBarForm : Form
    {
        static ILog l = Core.GetLogger(typeof(LineBarForm).FullName);

        System.Timers.Timer timer;
        IDataManager data;
        IDataProvider dataProvider;

        public LineBarForm(IDataProvider dataProvider)
        {
            InitializeComponent();

            this.dataProvider = dataProvider;

            IInterface interf = Core.GetGlobal("Interface") as IInterface;
            if (interf != null)
            {
                if (interf.GetMainForm() != null)
                    this.MdiParent = interf.GetMainForm();
            }

            data = Core.GetGlobal("data") as IDataManager;
            if (data == null)
                l.Error("data == null");

            timer = new System.Timers.Timer(250);
            timer.Elapsed +=new System.Timers.ElapsedEventHandler(timer_Elapsed);
        }

        Object Locker = new Object();
        DateTime startDT;

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            lock (Locker)
            {
                IBars bars = data.GetBars(data.GetSymbol(textBox1.Text), data.GetScale(ScaleEnum.tick, 1));
                int tickNum = bars.Count;
                int secNum = tickNum / 4 + 1;
                TimeFor timeFor = (TimeFor)(tickNum % 4);
                float price = 0;

                switch (timeFor)
                {
                    case TimeFor.Open:
                        price = secNum + 2;
                        break;
                    case TimeFor.Low:
                        price = secNum + 1;
                        break;
                    case TimeFor.High:
                        price = secNum + 4;
                        break;
                    case TimeFor.Close:
                        price = secNum + 3;
                        break;
                }

//                DateTime dt = new DateTime(2010,11,12,15,0,0) + new TimeSpan(secNum * 10000000);
                DateTime dt = startDT + new TimeSpan(secNum * 10000000);
                OpenWealth.Simple.Tick t = new OpenWealth.Simple.Tick(dt, tickNum, price, 1);
                bars.Add(dataProvider,  t);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer.Enabled = true;
            startDT = DateTime.Now;
            setEnable();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer.Enabled = false;
            setEnable();
        }

        void setEnable()
        {
            button1.Enabled = !timer.Enabled;
            button2.Enabled = timer.Enabled;
            textBox1.Enabled = !timer.Enabled;
        }

    }

    enum TimeFor { Open, Low, High, Close };
}
