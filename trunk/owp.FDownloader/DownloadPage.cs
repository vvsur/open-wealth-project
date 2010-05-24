using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using System.IO;

namespace owp.FDownloader
{
    public partial class DownloadPage : Page
    {
        public DownloadPage()
        {
            InitializeComponent();
        }

        public DownloadPage(Page previous)
        {
            InitializeComponent();
            this.previous = previous;
        }

        public override void SetSetting(Settings settings) 
        {
            base.SetSetting(settings);
            if (!backgroundWorker.IsBusy)
                backgroundWorker.RunWorkerAsync();
        }
        public override Settings GetSetting() 
        {
            backgroundWorker.CancelAsync();
            while (backgroundWorker.IsBusy)
                System.Threading.Thread.Sleep(50);
            return settings;
        }

        private string VerifyFileName(string FN)
        {
            StringBuilder result = new StringBuilder(FN);
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                result.Replace(c.ToString(), string.Empty);
            }
            return result.ToString();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Random random = new Random(DateTime.Now.Millisecond);
            List<Bars> listDownloads = new List<Bars>();

            backgroundWorker.ReportProgress(0, "Формирую список эмитентов для обработки");
                foreach(EmitentInfo emitent in settings.Emitents)
                {
                    if (backgroundWorker.CancellationPending) { e.Cancel = true; return; } // пользователь отменил операцию
                    if (emitent.checed) 
                        listDownloads.Add(new Bars(emitent));
                }

                for (int i = 0; i < listDownloads.Count;++i)
                {
                    Bars bars = listDownloads[i];
                    backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "==== Работаю с " + bars.emitent.code);
                    backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "Загружаю существующие CSV файлы");
                    if (Directory.Exists(Path.Combine(settings.csvDir, FinamHelper.Period2String(settings.period))))
                        foreach (string file in Directory.GetFiles(Path.Combine(settings.csvDir, FinamHelper.Period2String(settings.period)), VerifyFileName(bars.emitent.marketName) + '-' + bars.emitent.code + "(*).csv"))
                        {
                            if (backgroundWorker.CancellationPending) { e.Cancel = true; return; } // пользователь отменил операцию
                            if (File.Exists(file))
                            {
                                StreamReader csvFile = new StreamReader(file);
                                bars.LoadCSV(csvFile);
                                csvFile.Close();
                            }
                        }

                    backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "Загружаю существующие WL файлы");
                    if (backgroundWorker.CancellationPending) { e.Cancel = true; return; } // пользователь отменил операцию
                    bars.LoadWL(
                        Path.Combine(
                            Path.Combine(
                                Path.Combine(
                                    settings.wlDir,
                                    FinamHelper.Period2String(settings.period)),
                                    VerifyFileName(bars.emitent.marketName)),
                                    bars.emitent.code + ".wl")
                                ); // как просто стало жить в Framework 4

                    if (settings.loadFromFinam)
                    {
                        backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "Загружаю CSV с финама");
                        if (backgroundWorker.CancellationPending) { e.Cancel = true; return; } // пользователь отменил операцию
                        string csv;

                        if (settings.fromCSV)
                        {
                            settings.from = bars.Last;
                        }

                        DateTime from = settings.from;
                        int days = (settings.to - settings.from).Days;
                        for (int day4Tick = 0; day4Tick <= days; ++day4Tick)
                        {
                            //тиковые данные скачиваю по дням
                            if (settings.period == 1)
                            {
                                settings.from = from.AddDays(day4Tick);
                                settings.to = from.AddDays(day4Tick);
                            }
                            else
                                day4Tick = int.MaxValue;

                            backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "Загружаю " + bars.emitent.code + " " + settings.from.ToShortDateString() + "-" + settings.to.ToShortDateString());
                            do
                            {
                                if (backgroundWorker.CancellationPending) { e.Cancel = true; return; } // пользователь отменил операцию
                                try
                                {
                                    csv = FinamHelper.Download(settings, bars.emitent);
                                }
                                catch
                                {
                                    csv = String.Empty;
                                }
                                if (csv == "Система уже обрабатывает Ваш запрос. Дождитесь окончания обработки.")
                                {
                                    backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "Финам просит подождать");
                                    System.Threading.Thread.Sleep(random.Next(10000));
                                    if (backgroundWorker.CancellationPending) { e.Cancel = true; return; } // пользователь отменил операцию
                                    backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "Пробую снова");
                                }
                            } while (csv == "Система уже обрабатывает Ваш запрос. Дождитесь окончания обработки.");

                            backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "Анализирую");
                            StringReader csvStringReader = new StringReader(csv);
                            bars.LoadCSV(csvStringReader);
                            csvStringReader.Close();

                            if ((!settings.margeCsv) && (!settings.delCSV))
                            {
                                backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "Сохраняю загруженный csv");
                                // создаю уникальное имя для csv файла
                                string fileName;
                                string dir = Path.Combine(settings.csvDir, FinamHelper.Period2String(settings.period));
                                fileName = VerifyFileName(bars.emitent.marketName) + '-' + bars.emitent.code;

                                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                                int j = 0;
                                while (File.Exists(Path.Combine(dir, fileName + ".csv")))
                                {
                                    ++j;
                                    fileName = VerifyFileName(bars.emitent.marketName) + '-' + bars.emitent.code + '(' + j + ')';
                                }
                                File.WriteAllText(Path.Combine(dir, fileName + ".csv"), csv);
                            }
                        }
                        // восстанавлию, сбитые скачиванием по дням настройки
                        settings.from = from;
                        settings.to = from.AddDays(days);
                    }

                    if (settings.convertCSV2WL)
                    {
                        backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "Сохраняю WL файлы");
                        if (backgroundWorker.CancellationPending) { e.Cancel = true; return; } // пользователь отменил операцию
                        bars.Save(
                            Path.Combine(
                                Path.Combine(
                                    Path.Combine(
                                        settings.wlDir,
                                        FinamHelper.Period2String(settings.period)),
                                        VerifyFileName(bars.emitent.marketName)),
                                        bars.emitent.code + ".wl")
                                    );
                    }

                    if ((settings.margeCsv) || (settings.delCSV))
                    {
                        backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "Удаляю CSV файлы");
                        if (Directory.Exists(Path.Combine(settings.csvDir, FinamHelper.Period2String(settings.period))))
                            foreach (string file in Directory.GetFiles(Path.Combine(settings.csvDir, FinamHelper.Period2String(settings.period)), VerifyFileName(bars.emitent.marketName) + '-' + bars.emitent.code + "(*).csv"))
                            {
                                File.Delete(file);
                            }
                    }


                    if (settings.margeCsv)
                    {
                        backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "Сохраняю объедененные CSV файлы");
                        if (backgroundWorker.CancellationPending) { e.Cancel = true; return; } // пользователь отменил операцию
                        bars.SaveCSV(
                            Path.Combine(
                                Path.Combine(settings.csvDir, FinamHelper.Period2String(settings.period)),
                                VerifyFileName(bars.emitent.marketName) + '-' + bars.emitent.code + ".csv")
                                    );
                    }
                    bars.Clear();
                }
            backgroundWorker.ReportProgress(100, "Всё!!!");
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else
                if (e.Cancelled)
                {
                    textBox.Text += "Отменено пользователем" + Environment.NewLine;
                }
                else
                    if (settings.autoFlag)
                    {
                        Application.Exit();
                    }
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty((e.UserState as string)))
                textBox.Text += (e.UserState as string) + Environment.NewLine;
            textBox.SelectionStart = textBox.TextLength;
            textBox.ScrollToCaret(); 
            progressBar.Value = e.ProgressPercentage;
        }

    }


}