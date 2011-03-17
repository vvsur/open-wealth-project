using System;
using System.Windows.Forms;

using OpenWealth;

namespace FDownloader
{
    public partial class FDownloaderForm : Form
    {
        // подключаю log4net для ведения лога
        private static readonly ILog l = Core.GetLogger(typeof(FDownloaderForm).FullName);

        public FDownloaderForm()
        {
            InitializeComponent();
            l.Debug("форма FDownloaderForm создана");
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
                l.Debug("Удаляю страницу " + сorrentPage.GetType().ToString() );
                this.Controls.Remove(сorrentPage);
                settings = сorrentPage.GetSetting();
            }
            сorrentPage = newCorrentPage;
            if (сorrentPage != null)
            {
                l.Debug("Добавляю страницу " + сorrentPage.GetType().ToString());
                this.Controls.Add(сorrentPage);
                try
                {
                    сorrentPage.SetSetting(settings);
                }
                catch (Exception e)
                {
                    l.Error("Необробатываемое исключение в " + сorrentPage.GetType() + " " + e);
                }
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
            if (onlyOne.WaitOne(0,false))
            {
                timerOnlyOne.Enabled = false;
                labelOnlyOne.Visible = false;
                labelStarting.Visible = true;

                settings = new Settings();                
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
            if ((onlyOne!=null)&&(!timerOnlyOne.Enabled)) 
            {
                onlyOne.ReleaseMutex();
                onlyOne =null;
            }
        }
        #endregion
    }
}
