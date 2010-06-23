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
        // подключаю log4net для ведения лога
        private static readonly log4net.ILog l = log4net.LogManager.GetLogger(typeof(DownloadPage));

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
            else
                l.Error("backgroundWorker.IsBusy");

        }
        public override Settings GetSetting() 
        {
            l.Debug("Остонавливаю backgroundWorker");
            backgroundWorker.CancelAsync();
            while (backgroundWorker.IsBusy)
            {
                l.Debug("Жду освобождения backgroundWorker");
                System.Threading.Thread.Sleep(100);
                Application.DoEvents();
            }
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
            l.Debug("стартовал backgroundWorker_DoWork");

            Random random = new Random(DateTime.Now.Millisecond);
            List<Bars> listDownloads = new List<Bars>();

            backgroundWorker.ReportProgress(0, "Формирую список эмитентов для обработки");
            l.Debug("Формирую список эмитентов для обработки");
            foreach (EmitentInfo emitent in settings.Emitents)
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

                    l.Debug("==== Работаю с " + bars.emitent.code);
                    l.Debug("Загружаю существующие CSV файлы");

                    if (Directory.Exists(Path.Combine(settings.csvDir, FinamDataScale.ToString(settings.period))))
                        foreach (string file in Directory.GetFiles(Path.Combine(settings.csvDir, FinamDataScale.ToString(settings.period)), VerifyFileName(bars.emitent.marketName) + '-' + bars.emitent.code + "-*.csv"))
                        {
                            if (backgroundWorker.CancellationPending) { e.Cancel = true; return; } // пользователь отменил операцию
                            if (File.Exists(file))
                            {
                                StreamReader csvFile = new StreamReader(file);
                                bars.LoadCSV(csvFile);
                                csvFile.Close();
                            }
                        }

                    if (settings.period != 1)
                    {
                        // т.к. в WL файле не сохраняется id операции, то понять 
                        // тик из csv существует в wl или это новый тик не возможно
                        // поэтому для тиков wl вообще не загружается
                        backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "Загружаю существующие WL файлы");
                        l.Debug("Загружаю существующие WL файлы");
                        if (backgroundWorker.CancellationPending) { e.Cancel = true; return; } // пользователь отменил операцию
                        bars.LoadWL(
                            Path.Combine(
                                Path.Combine(
                                    Path.Combine(
                                        settings.wlDir,
                                        FinamDataScale.ToString(settings.period)),
                                        VerifyFileName(bars.emitent.marketName)),
                                        bars.emitent.code + ".wl")
                                    ); // как просто стало жить в Framework 4
                    }

                    if (settings.loadFromFinam)
                    {
                        backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "Загружаю CSV с финама");
                        l.Debug("Загружаю CSV с финама");
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
                                day4Tick = int.MaxValue-1;

                            backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "Загружаю " + bars.emitent.code + " " + settings.from.ToShortDateString() + "-" + settings.to.ToShortDateString());
                            l.Debug("Загружаю " + bars.emitent.code + " " + settings.from.ToShortDateString() + "-" + settings.to.ToShortDateString());
                            do
                            {
                                if (backgroundWorker.CancellationPending) { e.Cancel = true; return; } // пользователь отменил операцию
                                try
                                {
                                    csv = FinamHelper.Download(settings, bars.emitent);
                                }
                                catch
                                {
                                    l.Error("Необробатываемый Exception в FinamHelper.Download");
                                    csv = "Exception";
                                }
                                if (csv == "Система уже обрабатывает Ваш запрос. Дождитесь окончания обработки.")
                                {
                                    backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "Финам просит подождать");
                                    System.Threading.Thread.Sleep(random.Next(30000));
                                    if (backgroundWorker.CancellationPending) { e.Cancel = true; return; } // пользователь отменил операцию
                                    backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "Пробую снова");
                                }
                                if (csv == "Exception")
                                {
                                    backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "Ошибка при скачивании");
                                    System.Threading.Thread.Sleep(random.Next(30000));
                                    if (backgroundWorker.CancellationPending) { e.Cancel = true; return; } // пользователь отменил операцию
                                    backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "Пробую снова");
                                }
                            } while 
                                (
                                    (csv == "Система уже обрабатывает Ваш запрос. Дождитесь окончания обработки.")
                                ||
                                    (csv == "Exception")
                                );

                            if ((csv == String.Empty) || (csv.Length < 30))
                            {
                                backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "нет данных");
                                l.Debug("нет данных");
                                if (backgroundWorker.CancellationPending) { e.Cancel = true; return; } // пользователь отменил операцию
                            }
                            else
                            {
                                backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "Анализирую");
                                l.Debug("Анализирую");
                                StringReader csvStringReader = new StringReader(csv);
                                bars.LoadCSV(csvStringReader);
                                csvStringReader.Close();

                                if ((!settings.margeCsv) && (!settings.delCSV))
                                {
                                    backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "Сохраняю загруженный csv");
                                    l.Debug("Сохраняю загруженный csv");
                                    // создаю уникальное имя для csv файла
                                    string fileName;
                                    string dir = Path.Combine(settings.csvDir, FinamDataScale.ToString(settings.period));
                                    fileName = VerifyFileName(bars.emitent.marketName) + '-' + bars.emitent.code + '-' + settings.to.ToString("yyyyMMdd");

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
                            System.Threading.Thread.Sleep(random.Next(500)); // дадим интерфейсу отрисоваться, а финуму отдохнуть
                        }
                        // восстанавлию, сбитые скачиванием по дням настройки
                        settings.from = from;
                        settings.to = from.AddDays(days);
                    }

                    if (settings.convertCSV2WL)
                    {
                        backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "Сохраняю WL файлы");
                        l.Debug("Сохраняю WL файлы");
                        if (backgroundWorker.CancellationPending) { e.Cancel = true; return; } // пользователь отменил операцию
                        bars.Save(
                            Path.Combine(
                                Path.Combine(
                                    Path.Combine(
                                        settings.wlDir,
                                        FinamDataScale.ToString(settings.period)),
                                        VerifyFileName(bars.emitent.marketName)),
                                        bars.emitent.code + ".wl")
                                    );
                    }

                    if ((settings.margeCsv) || (settings.delCSV))
                    {
                        backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "Удаляю CSV файлы");
                        l.Debug("Удаляю CSV файлы");
                        if (Directory.Exists(Path.Combine(settings.csvDir, FinamDataScale.ToString(settings.period))))
                            foreach (string file in Directory.GetFiles(Path.Combine(settings.csvDir, FinamDataScale.ToString(settings.period)), VerifyFileName(bars.emitent.marketName) + '-' + bars.emitent.code + "(*).csv"))
                            {
                                File.Delete(file);
                            }
                    }


                    if (settings.margeCsv)
                    {
                        backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "Сохраняю объедененные CSV файлы");
                        l.Debug("Сохраняю объедененные CSV файлы");
                        if (backgroundWorker.CancellationPending) { e.Cancel = true; return; } // пользователь отменил операцию
                        bars.SaveCSV(
                            Path.Combine(
                                Path.Combine(settings.csvDir, FinamDataScale.ToString(settings.period)),
                                VerifyFileName(bars.emitent.marketName) + '-' + bars.emitent.code + ".csv")
                                    );
                    }

                    System.Threading.Thread.Sleep(random.Next(1000)); // дадим интерфейсу отрисоваться, а финуму отдохнуть

                    // пытаюсь освободить память
                    bars = null;
                    listDownloads[i] = null;
                    GC.Collect(); // а стоит ли оно того???
                }
            backgroundWorker.ReportProgress(100, "Всё!!!");
            l.Info("backgroundWorker_DoWork закончил");
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
                l.Error("Ошибка в backgroundWorker " + e);
            }
            else
                if (e.Cancelled)
                {
                    textBox.Text += "Отменено пользователем" + Environment.NewLine;
                    l.Info("Отменено пользователем");
                }
                else
                    if (settings.autoFlag)
                    {
                        Application.Exit();
                    }
        }

        private StringBuilder sb = new StringBuilder(11000);

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            if (!String.IsNullOrEmpty((e.UserState as string)))
            {
                sb.AppendLine(e.UserState as string);
                if (sb.Length > 10000)
                    sb.Remove(0, sb.Length - 10000);

                textBox.Text = sb.ToString();
                textBox.SelectionStart = textBox.TextLength;
                textBox.ScrollToCaret();
                l.Debug("backgroundWorker_ProgressChanged " + (e.UserState as string));
            }
            progressBar.Value = e.ProgressPercentage;
        }

    }


}