using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using System.Diagnostics;

namespace OpenWealth.WLProvider
{
    public partial class CommissionUserControl : UserControl
    {
        public CommissionUserControl()
        {
            InitializeComponent();
        }
        public Double F
        {
            get
            {
                return (double)this.forF.Value;
            }
            set {   
                this.forF.Text = value.ToString();
            }
        }
        public Double M
        {
            get
            {
                return (double)this.forM.Value;
            }
            set
            {
                this.forM.Text = value.ToString();
            }
        }

        public Double B
        {
            get
            {
                return (double)this.forB.Value;
            }
            set
            {
                this.forB.Text = value.ToString();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://OpenWealth.ru/");
        }
    }
}
