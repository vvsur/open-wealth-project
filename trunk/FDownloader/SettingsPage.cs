using System;
using OpenWealth;

namespace FDownloader
{
    public partial class SettingsPage : Page
    {
        private static readonly ILog l = Core.GetLogger(typeof(SettingsPage));

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

            checkBoxToday.Checked = settings.toToday;

            if (settings.toToday) dateTimePickerTo.Value = DateTime.Today;

            switch (settings.fromType)
            {
                case 0:
                    checkBoxFromType.Checked = false;
                    break;
                case 1:
                    radioButton1.Checked = true;
                    break;
                case 2:
                    radioButton2.Checked = true;
                    break;
                case 3:
                    radioButton3.Checked = true;
                    break;
                default:
                    l.Error("settings.fromType=" + settings.fromType);
                    break;
            }

            saveCSVCheckBox.Checked = settings.saveCSVChecked ;
            saveCSVFolderTextBox.Text = settings.saveCSVFolder;

            analyzeCheckBox.Checked = settings.analyzeChecked;
            downloadCheckBox.Checked = settings.downloadChecked;
        }
        public override Settings GetSetting() 
        {            
            settings.useProxy = checkBoxProxy.Checked;
            settings.proxy = textBoxProxy.Text;
            settings.proxyWithPassword = checkBoxProxyPassword.Checked;
            settings.proxyUser = textBoxLogin.Text;
            settings.proxyPassword = textBoxPassword.Text;

            settings.from = dateTimePickerFrom.Value;
            if (!checkBoxFromType.Checked) settings.fromType = 0;
            else
                if (radioButton1.Checked) settings.fromType = 1;
                else
                    if (radioButton2.Checked) settings.fromType = 2;
                    else
                        if (radioButton3.Checked) settings.fromType = 3;

            settings.toToday = checkBoxToday.Checked;
            settings.to = dateTimePickerTo.Value;

            settings.saveCSVChecked = saveCSVCheckBox.Checked;
            settings.saveCSVFolder = saveCSVFolderTextBox.Text;

            settings.analyzeChecked = analyzeCheckBox.Checked;
            settings.downloadChecked = downloadCheckBox.Checked;

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

        private void checkBoxFromType_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePickerFrom.Enabled = !checkBoxFromType.Checked;
            radioButton1.Enabled = checkBoxFromType.Checked;
            radioButton2.Enabled = checkBoxFromType.Checked;
            radioButton3.Enabled = checkBoxFromType.Checked;

            radioButton1.Checked = true;
        }

        private void checkBoxToday_CheckedChanged(object sender, EventArgs e)
        {
            dateTimePickerTo.Enabled = !checkBoxToday.Checked;
            dateTimePickerTo.Value = DateTime.Today;
        }

        private void saveCSVFolderButton_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();
            if(dialog.ShowDialog()==System.Windows.Forms.DialogResult.OK)
                saveCSVFolderTextBox.Text = dialog.SelectedPath;
        }

        private void saveCSVCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // TODO реализовать проверку aCheckBox
        }
    }
}
