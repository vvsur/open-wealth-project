using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace owp.FDownloader
{
    public partial class FinamTreeViewPage : Page
    {
        public FinamTreeViewPage()
        {
            InitializeComponent();
        }

        public FinamTreeViewPage(Page previous)
        {
            InitializeComponent();
            this.previous = previous;
            this.next = new DownloadPage(this);
        }

        public override void SetSetting(Settings settings) 
        {
            base.SetSetting(settings);
            if ((settings.Emitents==null) || (settings.Emitents.Count == 0))
                buttonRefresh_Click(null, null);
            else
                finamTreeView.SetEmitents(settings.Emitents);
        }
        public override Settings GetSetting() 
        {
            settings.Emitents = finamTreeView.GetEmitents();
            return base.GetSetting();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            finamTreeView.SetEmitents(FinamHelper.DownloadEmitents(settings));
            buttonRefresh.Enabled = false;
        }
    }
}
