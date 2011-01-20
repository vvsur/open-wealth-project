using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

using OpenWealth;

namespace TEST_bot_BackTestHost
{
    public partial class BacktestForm : Form
    {
        BackgroundWorker test = new BackgroundWorker();

        public BacktestForm()
        {
            InitializeComponent();

            test.DoWork += new DoWorkEventHandler(test_DoWork);
            test.RunWorkerCompleted += new RunWorkerCompletedEventHandler(test_RunWorkerCompleted);
            test.ProgressChanged += new ProgressChangedEventHandler(test_ProgressChanged);

//            Core.Data.ChangeMarkets += new EventHandler<EventArgs>(Data_ChangeMarkets);
//            Data_ChangeMarkets(this, null);
        }


        void test_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!test.CancellationPending)
            {
                throw new NotImplementedException();
                //test.ReportProgress((DT - startDT) / (endDT - startDT));
            }
            e.Cancel = true;
        }

        void test_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar.Value = e.ProgressPercentage;
        }

        void test_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
