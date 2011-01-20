using System;
using System.IO;
using OpenWealth;

namespace FDownloader
{
    public class Settings 
    {
        ISettingsHost sh = Core.GetGlobal("SettingsHost") as ISettingsHost;

        public bool useProxy
        {
            get
            {
                return sh.Get("FDownloader.UseProxy", false);
            }
            set
            {
                sh.Set("FDownloader.UseProxy", value);
            }
        }
        public String proxy
        {
            get
            {
                return sh.Get("FDownloader.Proxy", string.Empty);
            }
            set
            {
                sh.Set("FDownloader.Proxy", value);
            }
        }
        public bool proxyWithPassword
        {
            get
            {
                return sh.Get("FDownloader.ProxyWithPassword", false);
            }
            set
            {
                sh.Set("FDownloader.ProxyWithPassword", value);
            }
        }
        public String proxyUser
        {
            get
            {
                return sh.Get("FDownloader.ProxyUser", string.Empty);
            }
            set
            {
                sh.Set("FDownloader.ProxyUser", value);
            }
        }
        public String proxyPassword
        {
            get
            {
                return sh.Get("FDownloader.ProxyPassword", string.Empty);
            }
            set
            {
                sh.Set("FDownloader.ProxyPassword", value);
            }
        }

        public DateTime from
        {
            get
            {
                return sh.Get("FDownloader.From", DateTime.Now.Date.AddDays(-1));
            }
            set
            {
                sh.Set("FDownloader.From", value);
            }
        }
        /// <summary>
        /// 0 - брать дату из from
        /// 1 - вчера
        /// 2 - последний день из IBar
        /// 3 - последний день из IBar + 1
        /// </summary>
        public int fromType
        {
            get
            {
                return sh.Get("FDownloader.FromType", 0);
            }
            set
            {
                sh.Set("FDownloader.FromType", value);
            }
        }

        public DateTime to
        {
            get
            {
                if (toToday)
                    return DateTime.Now.Date;
                else 
                    return sh.Get("FDownloader.To", DateTime.Now.Date);
            }
            set
            {
                sh.Set("FDownloader.To", value);
            }
        }
        public bool toToday
        {
            get
            {
                return sh.Get("FDownloader.ToToday", false);
            }
            set
            {
                sh.Set("FDownloader.ToToday", value);
            }
        }

        public bool saveCSVChecked
        {
            get
            {
                return sh.Get("FDownloader.saveCSVChecked", false);
            }
            set
            {
                sh.Set("FDownloader.saveCSVChecked", value);
            }
        }

        public string saveCSVFolder
        {
            get
            {
                return sh.Get("FDownloader.saveCSVFolder", string.Empty);
            }
            set
            {
                sh.Set("FDownloader.saveCSVFolder", value);
            }
        }

        public String EmitentsFileName
        {
            get
            {
                string result = sh.Get("FDownloader.EmitentsFileName", string.Empty);
                if (result != string.Empty)
                    return result;

                string dataDir = sh.Get("DataDir", string.Empty);
                if (dataDir == string.Empty)
                {
                    dataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "OpenWealth");
                    sh.Set("DataDir", dataDir);
                }
                return Path.Combine(dataDir, "FDownloaderEmitents.xml");
            }
            set
            {
                sh.Set("FDownloader.EmitentsFileName", value);
            }
        }

        public bool analyzeChecked
        {
            get
            {
                return sh.Get("FDownloader.analyzeChecked", true);
            }
            set
            {
                sh.Set("FDownloader.analyzeChecked", value);
            }
        }

        public bool downloadChecked
        {
            get
            {
                return sh.Get("FDownloader.downloadCheckBox", true);
            }
            set
            {
                sh.Set("FDownloader.downloadCheckBox", value);
            }
        }
    }
}
