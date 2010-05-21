using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace owp.FDownloader
{
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            next = new FinamTreeViewPage(this);
        }

        public override void SetSetting(Settings settings) 
        {
            base.SetSetting(settings);

            if (settings == null)
                return;

            checkBoxProxy.Checked = settings.useProxy;
            textBoxProxy.Text = settings.proxy;
            checkBoxProxyPassword.Checked = settings.proxyWithPassword;
            textBoxLogin.Text = settings.proxyUser;
            textBoxPassword.Text = settings.proxyPassword;

            if ((settings.from > dateTimePickerFrom.MinDate) && (settings.from < dateTimePickerFrom.MaxDate))
                dateTimePickerFrom.Value = settings.from;
            else
                dateTimePickerFrom.Value = DateTime.Today;
            if ((settings.to > dateTimePickerTo.MinDate) && (settings.to < dateTimePickerTo.MaxDate))
                dateTimePickerTo.Value = settings.to;
            else
                dateTimePickerTo.Value = DateTime.Today;
            if (settings.period>0)
                comboBoxPeriod.SelectedIndex = settings.period - 1;
            else
                comboBoxPeriod.SelectedIndex = 6;

            checkBoxLoadFromFinam.Checked = settings.loadFromFinam;
            checkBoxFromCSV.Checked = settings.fromCSV;
            checkBoxYesterday.Checked = settings.fromYesterday;
            checkBoxToday.Checked = settings.toToday;

            if (settings.fromYesterday) dateTimePickerFrom.Value = DateTime.Today.AddDays(-1);
            if (settings.toToday) dateTimePickerTo.Value = DateTime.Today;

            textBoxCSVDir.Text = settings.csvDir;
            checkBoxMargeCSV.Checked = settings.margeCsv;
            checkBoxConvertCSV2WL.Checked = settings.convertCSV2WL;
            checkBoxDelCSV.Checked = settings.delCSV;
            textBoxWLDir.Text = settings.wlDir;
        }
        public override Settings GetSetting() 
        {            
            settings.useProxy = checkBoxProxy.Checked;
            settings.proxy = textBoxProxy.Text;
            settings.proxyWithPassword = checkBoxProxyPassword.Checked;
            settings.proxyUser = textBoxLogin.Text;
            settings.proxyPassword = textBoxPassword.Text;

            settings.loadFromFinam = checkBoxLoadFromFinam.Checked;
            settings.fromYesterday = checkBoxYesterday.Checked;
            settings.fromCSV = checkBoxFromCSV.Checked;
            settings.toToday = checkBoxToday.Checked;
            settings.from = dateTimePickerFrom.Value;
            settings.to = dateTimePickerTo.Value;
            settings.period = comboBoxPeriod.SelectedIndex + 1;

            settings.csvDir = textBoxCSVDir.Text;
            settings.margeCsv = checkBoxMargeCSV.Checked;
            settings.convertCSV2WL = checkBoxConvertCSV2WL.Checked;
            settings.delCSV = checkBoxDelCSV.Checked;
            settings.wlDir = textBoxWLDir.Text;

            return base.GetSetting();
        }

        private void checkBoxProxy_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxProxy.Enabled = checkBoxProxy.Checked;
        }
        private void checkBoxProxyPassword_CheckedChanged(object sender, EventArgs e)
        {
            label2.Enabled = checkBoxProxyPassword.Checked && checkBoxProxyPassword.Enabled;
            label3.Enabled = checkBoxProxyPassword.Checked && checkBoxProxyPassword.Enabled;
            textBoxLogin.Enabled = checkBoxProxyPassword.Checked && checkBoxProxyPassword.Enabled;
            textBoxPassword.Enabled = checkBoxProxyPassword.Checked && checkBoxProxyPassword.Enabled;
        }

        private void checkBoxYesterday_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxYesterday.Checked)
            {
                checkBoxFromCSV.Checked = false;
                dateTimePickerFrom.Enabled = false;
                dateTimePickerFrom.Value = DateTime.Today.AddDays(-1);
            }
            else
                dateTimePickerFrom.Enabled = !checkBoxFromCSV.Checked;
        }

        private void checkBoxFromCSV_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxFromCSV.Checked)
            {
                checkBoxYesterday.Checked = false;
                dateTimePickerFrom.Enabled = false;
                dateTimePickerFrom.Value = dateTimePickerFrom.MinDate;
            }
            else
                dateTimePickerFrom.Enabled = !checkBoxYesterday.Checked;
            if (dateTimePickerFrom.Enabled) 
                dateTimePickerFrom.Value = DateTime.Today.AddDays(-1);
        }

        private void checkBoxToday_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePickerTo.Enabled = !checkBoxToday.Checked;
            dateTimePickerTo.Value = DateTime.Today;

        }

        private void checkBoxOnlyConvert_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxLoadSetting.Enabled = checkBoxLoadFromFinam.Checked;
        }

        private void checkBoxConvertCSV2WL_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxDelCSV.Enabled =
            checkBoxDelCSV.Checked =
            textBoxWLDir.Enabled =
            labelWLDir.Enabled =
            buttonWLDir.Enabled = checkBoxConvertCSV2WL.Checked;
        }

        private void checkBoxDelCSV_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxMargeCSV.Enabled = ! checkBoxDelCSV.Checked;
        }

        private void buttonLoadSetting_Click(object sender, EventArgs e)
        {
            openFileDialogLoadSetting.InitialDirectory = Environment.CurrentDirectory;
            if (openFileDialogLoadSetting.ShowDialog() == DialogResult.OK)
            {
                SetSetting(Settings.Load(openFileDialogLoadSetting.FileName));
            }
        }

        private void buttonCSVDir_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = Environment.CurrentDirectory;
            if (DialogResult.OK == folderBrowserDialog1.ShowDialog())
                textBoxCSVDir.Text = folderBrowserDialog1.SelectedPath;
        }

        private void buttonWLDir_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = Environment.CurrentDirectory;
            if (DialogResult.OK == folderBrowserDialog1.ShowDialog())
                textBoxWLDir.Text = folderBrowserDialog1.SelectedPath;
        }
    }
}
