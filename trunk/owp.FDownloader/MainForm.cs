using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace owp.FDownloader
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        Settings settings;
        String settingsFileName = string.Empty;

        #region Работа кнопок Назад Далее 

        Page сorrentPage;

        private void SetCorrentPage(Page newCorrentPage)
        {
            if (сorrentPage != null)
            {
                this.Controls.Remove(сorrentPage);
                settings = сorrentPage.GetSetting();
            }
            сorrentPage = newCorrentPage;
            if (сorrentPage != null)
            {
                this.Controls.Add(сorrentPage);
                сorrentPage.SetSetting(settings);

                buttonNext.Enabled = сorrentPage.NextExists();
                buttonPrevious.Enabled = сorrentPage.PreviousExists();
            }
            else
            {
                buttonNext.Enabled = false;
                buttonPrevious.Enabled = false;
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            buttonNext.Enabled = false;
            SetCorrentPage(сorrentPage.NextControl());
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            buttonPrevious.Enabled = false;
            SetCorrentPage(сorrentPage.PreviousControl());
        }
        #endregion

        #region Запуск (Проверка, что запущена только одна копия программы)

        System.Threading.Mutex onlyOne = new System.Threading.Mutex(false, "FDownloader 57DCD6DC-0CB3-4162-B8FF-C7A95ABF00E9");

        private void timerOnlyOne_Tick(object sender, EventArgs e)
        {
            if (onlyOne.WaitOne(0))
            {
                timerOnlyOne.Enabled = false;
                labelOnlyOne.Visible = false;
                labelStarting.Visible = true;

                bool autoFlag = false; 
                bool settingFlag = false;

                // проверяю командную строку
                String[] arg = System.Environment.GetCommandLineArgs();

                // если имени файла с настройками в ключах командах найдено не будет
                settingsFileName = System.IO.Path.ChangeExtension(arg[0],".config.xml");

                for (int i = 1; i < arg.Length; ++i)
                {
                    if (settingFlag) 
                        settingsFileName = arg[i];
                    autoFlag = autoFlag || (arg[i].ToUpper() == "/R");
                    settingFlag = (arg[i].ToUpper() == "/S");
                }
                // загружаю файл настроек
                settings = Settings.Load(settingsFileName);
                settings.autoFlag = autoFlag;
                
                if (autoFlag) 
                    SetCorrentPage(new DownloadPage());
                else 
                    SetCorrentPage(new SettingsPage());
                labelStarting.Visible = false;
            }
            else
            {
                labelOnlyOne.Visible = true;
                labelStarting.Visible = false;
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            SetCorrentPage(null);
            settings.Save(settingsFileName);
            if (!timerOnlyOne.Enabled) onlyOne.ReleaseMutex();
        }
        #endregion

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://code.google.com/p/open-wealth-project/wiki/FDownloader");
        }
    }
}
