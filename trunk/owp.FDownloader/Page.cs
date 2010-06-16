using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

namespace owp.FDownloader
{
    public class Page : UserControl
    {


        protected Settings settings;
        protected Page previous = null;
        protected Page next = null;

        public Page() {}

        public Page(Page previous) {this.previous = previous;}

        public virtual void SetSetting(Settings settings) { this.settings = settings;  }
        public virtual Settings GetSetting() { return settings; }

        public virtual Page PreviousControl() { return previous; }
        public virtual bool PreviousExists() { return (previous != null); }

        public virtual Page NextControl() { return next; }
        public virtual bool NextExists() { return (next != null); }
    }
}
