using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace OpenWealth.DataManager
{
    public partial class ExportForm : Form
    {
        public ExportForm()
        {
            InitializeComponent();

            numericUpDown1.Value = Core.SettingsHost.Get("Export.TimeFrame", 3600);
            dateTimePicker1.Value = Core.SettingsHost.Get("Export.From", DateTime.Now);
            dateTimePicker2.Value = Core.SettingsHost.Get("Export.To", DateTime.Now);
            folderTextBox.Text = Core.SettingsHost.Get("Export.Dir", @"c:\export_from_openwealth");
            comboBox1.Text = Core.SettingsHost.Get("Export.FileType", "csv");
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if(backgroundWorker1.IsBusy)
                backgroundWorker1.CancelAsync();
            else
                Close();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            DoWorkParam doWorkParam = e.Argument as DoWorkParam;
            if (doWorkParam != null)
            {
                if (!Directory.Exists(doWorkParam.Dir ))
                    Directory.CreateDirectory(doWorkParam.Dir);

                IScale scale;
                if (doWorkParam.TimeFrame>0)
                    scale = Core.Data.GetScale(ScaleEnum.sec, doWorkParam.TimeFrame);
                else
                    scale = Core.Data.GetScale(ScaleEnum.tick, 1);

                for (int i4Symbol = 0; i4Symbol < doWorkParam.SymbolList.Count; ++i4Symbol)
                {
                    if (backgroundWorker1.CancellationPending) { e.Cancel = true; return; }
                    IBars bars = Core.Data.GetBars(doWorkParam.SymbolList[i4Symbol], scale);
                    if (backgroundWorker1.CancellationPending) { e.Cancel = true; return; }
                    IBar bar = bars.Get(doWorkParam.From);
                    if (bar == null)
                    {
                        bar = bars.First;
                    }
                    if ((bar != null) && (bar.DT <= doWorkParam.To))
                    {
                        double progressDelta = 1 / doWorkParam.SymbolList.Count;
                        int count = 0;
//                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\Users\Public\TestFolder\WriteLines2.txt"))
                        using (BinaryWriter file = new BinaryWriter(File.Open(Path.Combine(doWorkParam.Dir,string.Concat(doWorkParam.SymbolList[i4Symbol],".",doWorkParam.FileType)), FileMode.Create, FileAccess.Write)))
                        {
                            //заголовок файла
                            if (doWorkParam.FileType == "wl")
                                file.Write((int)0);
                            else
                                if (doWorkParam.TimeFrame == 0)
                                    file.Write(Encoding.ASCII.GetBytes("<DATE>;<TIME>;<LAST>;<VOL>;<ID>" + Environment.NewLine));
                                else
                                    file.Write(Encoding.ASCII.GetBytes("<DATE>;<TIME>;<OPEN>;<HIGH>;<LOW>;<CLOSE>;<VOL>" + Environment.NewLine));

                            // данные файла
                            while ((bar != null) && (bar.DT <= doWorkParam.To))
                            {
                                if (backgroundWorker1.CancellationPending) { e.Cancel = true; return; }

                                if (doWorkParam.FileType == "wl")
                                {
                                    file.Write((double)DateTime2Int.DateTime(bar.DT).ToOADate());
                                    file.Write((float)bar.Open);
                                    file.Write((float)bar.High);
                                    file.Write((float)bar.Low);
                                    file.Write((float)bar.Close);
                                    file.Write((float)bar.Volume);
                                }
                                else
                                {
                                    string s = string.Empty;
                                    if (doWorkParam.TimeFrame == 0)
                                        s = string.Concat(
                                                DateTime2Int.DateTime(bar.DT).ToString("yyyyMMdd;"),
                                                DateTime2Int.DateTime(bar.DT).ToString("hhmmss;"),
                                                bar.Close.ToString(),
                                                ";",
                                                bar.Volume.ToString(),
                                                ";",
                                                bar.Number.ToString(),
                                                Environment.NewLine
                                                );
                                    else
                                        s = string.Concat(
                                                DateTime2Int.DateTime(bar.DT).ToString("yyyyMMdd;"),
                                                DateTime2Int.DateTime(bar.DT).ToString("hhmmss;"),
                                                bar.Open.ToString(),
                                                ";",
                                                bar.High.ToString(),
                                                ";",
                                                bar.Low.ToString(),
                                                ";",
                                                bar.Close.ToString(),
                                                ";",
                                                bar.Volume.ToString(),
                                                Environment.NewLine
                                                );
                                    byte[] bytes = Encoding.ASCII.GetBytes(s);
                                    file.Write(bytes);
                                }

                                ++count;
                                backgroundWorker1.ReportProgress((int)(100 * i4Symbol / doWorkParam.SymbolList.Count + progressDelta));
                                bar = bars.GetNext(bar);
                            }
                            if (doWorkParam.FileType == "wl")
                            {
                                file.Seek(0, SeekOrigin.Begin);
                                file.Write(count);
                            }
                            file.Close();
                        }
                    }
                }
            }
            if (backgroundWorker1.CancellationPending) { e.Cancel = true; return; }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void exportButton_Click(object sender, EventArgs e)
        {
            EnableAll(false);
            backgroundWorker1.RunWorkerAsync(new DoWorkParam(
                selectSymbol1.GetSelectedSymbols(),
                (int)numericUpDown1.Value,
                DateTime2Int.Int(dateTimePicker1.Value),
                DateTime2Int.Int(dateTimePicker2.Value),
                folderTextBox.Text,
                comboBox1.Text
                ));
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            EnableAll(true);
            if (e.Cancelled)
                MessageBox.Show("Экспорт отменён");
            else
                MessageBox.Show("Экспорт завершён");
        }

        void EnableAll(bool enable)
        {
            exportButton.Enabled = enable;
            selectSymbol1.Enabled = enable;
            numericUpDown1.Enabled = enable;
            dateTimePicker1.Enabled = enable;
            dateTimePicker2.Enabled = enable;
            folderTextBox.Enabled = enable;
            comboBox1.Enabled = enable;
            button1.Enabled = enable;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog d = new FolderBrowserDialog();
            if (d.ShowDialog() == DialogResult.OK)
                folderTextBox.Text = d.SelectedPath;
        }

        internal class DoWorkParam
        {
            internal DoWorkParam(IList<ISymbol> SymbolList, int TimeFrame, int From, int To, string Dir, string FileType)
            {
                this.SymbolList = SymbolList;
                this.TimeFrame = TimeFrame;
                this.From = From;
                this.To = To;
                this.Dir = Dir;
                this.FileType = FileType;
            }
            internal readonly IList<ISymbol> SymbolList;
            internal readonly int TimeFrame;
            internal readonly int From, To;
            internal readonly string Dir, FileType;
        }

        private void ExportForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Core.SettingsHost.Set("Export.TimeFrame", (int)numericUpDown1.Value);
            Core.SettingsHost.Set("Export.From", dateTimePicker1.Value);
            Core.SettingsHost.Set("Export.To", dateTimePicker2.Value);
            Core.SettingsHost.Set("Export.Dir", folderTextBox.Text);
            Core.SettingsHost.Set("Export.FileType", comboBox1.Text);

        }
    }
}