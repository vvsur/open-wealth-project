using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;

using OpenWealth;

namespace FDownloader
{
    public partial class DownloadPage : Page
    {
        // подключаю log4net для ведения лога
        private static readonly ILog l = Core.GetLogger(typeof(DownloadPage));

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
                result.Replace(c.ToString(), " ");
            }
            return result.ToString();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            l.Debug("стартовал backgroundWorker_DoWork");

            Random random = new Random(DateTime.Now.Millisecond);
            List<EmitentInfo> listDownloads = new List<EmitentInfo>();

            #region Формирование списка эмитентов
            backgroundWorker.ReportProgress(0, "Формирую список эмитентов для обработки");
            l.Debug("Формирую список эмитентов для обработки");

            lock (FinamHelper.Lock)
            {
                foreach (EmitentInfo emitent in FinamHelper.Emitents)
                {
                    if (backgroundWorker.CancellationPending) { e.Cancel = true; return; } // пользователь отменил операцию
                    if ((emitent.Id != -1) && (emitent.Checked))
                        listDownloads.Add(emitent);
                }
            }
            #endregion Формирование списка эмитентов

            IScale tickScale = Core.Data.GetScale(ScaleEnum.tick, 1);

            for (int i = 0; i < listDownloads.Count; ++i)
            {
                ISymbol symbol = Core.Data.GetSymbol(listDownloads[i].MarketName, listDownloads[i].Name);

                backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "==== Работаю с " + symbol);
                l.Debug("==== Работаю с " + symbol);

                if (backgroundWorker.CancellationPending) { e.Cancel = true; return; } // пользователь отменил операцию

                #region Определение начальной date
                DateTime date = settings.from;

                switch (settings.fromType)
                {
                    case 0:
                        date = settings.from;
                        break;
                    case 1:
                        date = DateTime.Now.Date.AddDays(-1);
                        break;
                    case 2:
                        date = DateTime2Int.DateTime(Core.Data.GetBars(symbol, tickScale).Last.DT);
                        break;
                    case 3:
                        date = DateTime2Int.DateTime(Core.Data.GetBars(symbol, tickScale).Last.DT).AddDays(-1);
                        break;
                    default:
                        l.Error("settings.fromType=" + settings.fromType);
                        break;
                }
                #endregion Определение начальной date

                for (; date <= settings.to; date = date.AddDays(1))
                {
                    string filename = Path.Combine(settings.saveCSVFolder, listDownloads[i].MarketName + "-" + listDownloads[i].Code + "-" + date.ToString("yyyyMMdd") + ".csv");
                    string csv = string.Empty;

                    if (!File.Exists(filename))
                    {
                        #region Загрузка c финама
                        if (settings.downloadChecked)
                        {
                            backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "Загружаю " + symbol + " за " + date.ToShortDateString());
                            l.Debug("Загружаю " + symbol + " за " + date.ToShortDateString());
                            do
                            {
                                if (backgroundWorker.CancellationPending) { e.Cancel = true; return; } // пользователь отменил операцию
                                try
                                {
                                    csv = FinamHelper.Download(settings, listDownloads[i], date);
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
                                backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "нет данных \r\n" + csv);
                                l.Debug("нет данных \r\n" + csv);
                                if (backgroundWorker.CancellationPending) { e.Cancel = true; return; } // пользователь отменил операцию
                            }
                        }
                        #endregion Загрузка с финама

                        #region Сохранение
                        if ((settings.saveCSVChecked) && (csv.Length >= 30))
                        {
                            backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "Сохраняю csv");
                            l.Debug("Сохраняю csv");

                            try
                            {
                                if (!Directory.Exists(settings.saveCSVFolder))
                                    Directory.CreateDirectory(settings.saveCSVFolder);

                                File.WriteAllText(filename, csv, Encoding.UTF8);
                            }
                            catch (Exception exc)
                            {
                                l.Error("Не смог сохранить csv в файл ", exc);
                            }
                        }
                        #endregion Сохранение
                    }

                    #region Анализ
                    if (settings.analyzeChecked)
                    {
                        if ((csv.Length < 30) && (File.Exists(filename)))
                            csv = File.ReadAllText(filename, Encoding.UTF8);

                        if ((settings.analyzeChecked) && (csv.Length >= 30))
                        {
                            backgroundWorker.ReportProgress((100 * i / listDownloads.Count), "Анализирую " + symbol + " за " + date.ToShortDateString());
                            l.Debug("Анализирую " + symbol + " за " + date.ToShortDateString());
                            using (StringReader csvStringReader = new StringReader(csv))
                            {
                                FinamHelper.LoadCSV(csvStringReader, symbol);
                            }
                        }
                    }
                    #endregion Анализ

                    System.Threading.Thread.Sleep(random.Next(100)); // дадим интерфейсу отрисоваться, а финуму отдохнуть
                }
            }

            if (backgroundWorker.CancellationPending) { e.Cancel = true; return; } // пользователь отменил операцию

            System.Threading.Thread.Sleep(random.Next(1000)); // дадим интерфейсу отрисоваться, а финуму отдохнуть

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
        }

        private StringBuilder sb = new StringBuilder(11000);
        private DateTime lastStatusUpdate = DateTime.Now;
        readonly TimeSpan sec = new TimeSpan(0, 0, 1);

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (!String.IsNullOrEmpty((e.UserState as string)))
            {
                sb.AppendLine(e.UserState as string);
                if (sb.Length > 10000)
                    sb.Remove(0, sb.Length - 10000);

                if ((DateTime.Now - lastStatusUpdate) >= sec)
                {
                    textBox.Text = sb.ToString();//!!!!!! очень долго
                    textBox.SelectionStart = textBox.TextLength;
                    textBox.ScrollToCaret();
                }
                l.Debug("backgroundWorker_ProgressChanged " + (e.UserState as string));
            }
            progressBar.Value = e.ProgressPercentage;
        }
    }


}