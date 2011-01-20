using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using OpenWealth;

namespace FDownloader
{
    public class FDownloader: IPlugin
    {
        static ILog l = Core.GetLogger(typeof(FDownloader).FullName);

        IInterface interf;
        public void Init() 
        {  
            l.Debug("Инициирую RndDataSource");
            interf = Core.GetGlobal("Interface") as IInterface;

            if (interf != null)
            {
                interf.AddMenuItem("Данные", "Загрузка с Finam.ru", null, menu_LoadFromFinam);
                interf.AddMenuItem("Данные", "Загрузка всех csv из папки", null, menu_LoadCSV);
            }
        }

        public void Stop() { }

        Form fDownloaderForm = null;
        void menu_LoadFromFinam(object sender, EventArgs e)
        {
            if (interf == null)
            {
                l.Error("Не найден объект, реализующий интерфейс");
                return;
            }

            if ((fDownloaderForm == null) || (fDownloaderForm.IsDisposed))
            {
                fDownloaderForm = new FDownloaderForm();
                fDownloaderForm.MdiParent = interf.GetMainForm();
                fDownloaderForm.Show();
            }
        }

        ISymbol ParseFileNameToSymbol(string fileName)
        {
            string onlyName = Path.GetFileNameWithoutExtension(fileName);
            
            string[] split = onlyName.Split('-');
            if (split.Length < 3)
            {
                l.Error("Не могу распарсить название бумаги " + onlyName);
                return Core.Data.GetSymbol(string.Empty, onlyName);
            }

            if (split[0] == "АДР") split[0] = "ADR";
            if (split[0] == "РТС") split[0] = "RTS";
            if (split[0] == "ФОРТС") split[0] = "SPFB";
            if (split[0] == "Индексы мировой экономики") split[0] = "Мировые Индексы";
            if (split[0] == "Отрасли экономики США") split[0] = "CBOT";

            if (split.Length > 3)
                for (int i = 2; i < split.Length - 1; ++i)  
                    split[1] = string.Concat(split[1], "-", split[i]);
            
            string[] split2 = split[1].Split('.');

            if ((split2[0] == "RTS") && (split2[1] == "ST") && (split2.Length > 2) && (split[0]=="RTS Standart"))
            {
                for (int i = 2; i < split2.Length; ++i)
                    split2[i - 2] = split2[i];
                split2[split2.Length - 2] = string.Empty;
                split2[split2.Length - 1] = string.Empty;
            }

            if ((split2[0] == "RTS") && (split2.Length > 1) && (split[0].Contains("RTS") || split[0].Contains("РТС")))
            {
                for (int i = 1; i < split2.Length; ++i)
                    split2[i - 1] = split2[i];
                split2[split2.Length - 1] = string.Empty;
            }

            if (split2.Length > 2)
                for (int i = 2; i < split2.Length; ++i)
                    if (split2[i] != string.Empty)
                        split2[1] = string.Concat(split2[1], ".", split2[i]);

            if (split2[0] == split[0])                               
                return Core.Data.GetSymbol(split2[0], split2[1]);
            else
                if ((split2.Length == 1) || (split2[1] == string.Empty))
                    return Core.Data.GetSymbol(split[0], split2[0]);
                else
                    return Core.Data.GetSymbol(split[0], split2[0] + "." + split2[1]);
        }

        void menu_LoadCSV(object sender, EventArgs e)
        {
            FolderBrowserDialog d = new FolderBrowserDialog();
            int i = 1;
            System.Text.ASCIIEncoding ascii = new System.Text.ASCIIEncoding();

            if (d.ShowDialog() == DialogResult.OK)
            {
                string[] filesArray = Directory.GetFiles(d.SelectedPath, "*.csv", SearchOption.AllDirectories);
                List<string> files = new List<string>(filesArray);

                files.Sort();

                foreach (string file in files)
                {
                    if (l.IsInfoEnabled)
                        l.Info("[" + i + "/" + files.Count + "] Загружаю файл " + file);
                    FinamHelper.LoadCSV(new StreamReader(file, ascii), ParseFileNameToSymbol(file));
                    ++i;
                }
            }
        }
    }
}
