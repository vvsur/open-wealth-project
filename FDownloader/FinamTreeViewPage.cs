using System;

namespace FDownloader
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

            lock (FinamHelper.Lock)
            {
                if (FinamHelper.Emitents == null)
                    FinamHelper.Emitents = FinamHelper.LoadEmitents(settings.EmitentsFileName);
                if (FinamHelper.Emitents.Count == 0)
                    buttonRefresh_Click(null, null);
                else
                    finamTreeView.SetEmitents(FinamHelper.Emitents);
            }
        }
        public override Settings GetSetting() 
        {
            lock (FinamHelper.Lock)
            {
                FinamHelper.Emitents = finamTreeView.GetEmitents();
            }
            return base.GetSetting();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            finamTreeView.SetEmitents(FinamHelper.DownloadEmitents(settings));
            buttonRefresh.Enabled = false;
        }
    }
}
