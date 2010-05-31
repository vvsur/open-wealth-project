using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Net;

namespace owp.FDownloader
{
    /// <summary>
    /// этот класс создался только ради увелечения таймаута
    /// </summary>
    public class TimeoutWebClient : WebClient
    {
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest webRequest = base.GetWebRequest(address);
            webRequest.Timeout = 600000;
            return webRequest;
        }
    }

    class FinamHelper
    {
        /// <summary>
        /// Создается, и инициируется WebClient
        /// </summary>
        /// <param name="settings">Настройки прокси</param>
        /// <returns></returns>
        private static System.Net.WebClient InitWebClient(Settings settings)
        {
            System.Net.WebClient webClient = new TimeoutWebClient();
          
            // настраиваю прокси
            if (settings.useProxy) 
            {
                System.Net.WebProxy webProxy = new System.Net.WebProxy();
                webProxy.Address = new Uri(settings.proxy);

                if (settings.proxyWithPassword)
                {
                    webProxy.Credentials = new System.Net.NetworkCredential(settings.proxyUser, settings.proxyPassword);
                }
                webClient.Proxy = webProxy;
            }
            return webClient;
        }

        /// <summary>
        /// Скачиваю список эмитентов из финама
        /// </summary>
        /// <param name="settings">Настройки прокси</param>
        /// <returns>Список эмитентов</returns>
        public static List<EmitentInfo> DownloadEmitents(Settings settings)
        {
            System.Net.WebClient webClient = InitWebClient(settings);

            // скачиваю интерфейс
            string marketsString = webClient.DownloadString(@"http://www.finam.ru/analysis/export/default.asp");

            String sOption = @"<option\s+?.*?value=""(?<id>[0-9]+)"".*?>(?<name>.+?)</option>";
            String sSelect = @"<select(.|\n)+?id=""market""(.|\n)+?(" + sOption + ")+?(.|\n)*?</select>";

            // Поиск нужного <select id="market">
            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(marketsString, sSelect, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            if (!m.Success)
            {
                //TODO Ошибка парсинга
                return null;
            }

            marketsString = m.Value;

            // поиск всех секций
            System.Text.RegularExpressions.MatchCollection mc = System.Text.RegularExpressions.Regex.Matches(marketsString, sOption, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            List<EmitentInfo> markets = new List<EmitentInfo>();

            foreach (System.Text.RegularExpressions.Match match in mc)
            {
                markets.Add(new EmitentInfo(Convert.ToInt32(match.Groups["id"].Value), match.Groups["name"].Value, -1, String.Empty, String.Empty));
            }

            // скачиваю js
            string instruments = webClient.DownloadString(@"http://www.finam.ru/scripts/export.js");

            String pattern = @"var\saEmitentIds=new\sArray\((?<EmitentIds>.*?)\);" + "(.|\n)*?" +
                             @"var\saEmitentNames=new\sArray\((?<EmitentNames>.*?)\);" + "(.|\n)*?" +
                             @"var\saEmitentCodes=new\sArray\((?<EmitentCodes>.*?)\);" + "(.|\n)*?" +
                             @"var\saEmitentMarkets=new\sArray\((?<EmitentMarkets>.*?)\);" + "(.|\n)*?" +
                             @"var\saDataFormatStrs=new Array\(.*?\);";

            System.Text.RegularExpressions.Match emitentMarkets
                = System.Text.RegularExpressions.Regex.Match(instruments, pattern);

            System.Text.RegularExpressions.MatchCollection sEmitentIds
                = System.Text.RegularExpressions.Regex.Matches(emitentMarkets.Groups["EmitentIds"].Value, @"[0-9]+");
            System.Text.RegularExpressions.MatchCollection sEmitentNames
                = System.Text.RegularExpressions.Regex.Matches(emitentMarkets.Groups["EmitentNames"].Value, @"'.+?'");
            System.Text.RegularExpressions.MatchCollection sEmitentCodes
                = System.Text.RegularExpressions.Regex.Matches(emitentMarkets.Groups["EmitentCodes"].Value, @"'.+?'");
            System.Text.RegularExpressions.MatchCollection sEmitentMarkets
                = System.Text.RegularExpressions.Regex.Matches(emitentMarkets.Groups["EmitentMarkets"].Value, @"[0-9]+");

            if ((sEmitentIds.Count != 0) && (sEmitentIds.Count == sEmitentNames.Count) && (sEmitentIds.Count == sEmitentCodes.Count) && (sEmitentNames.Count == sEmitentMarkets.Count))
            {
                List<EmitentInfo> emitents = new List<EmitentInfo>();
                for (int i = 0; i < sEmitentMarkets.Count; ++i)
                {
                    foreach (EmitentInfo market in markets)
                    {
                        if (Convert.ToInt32(sEmitentMarkets[i].Value) == market.marketId)
                        {
                            String chars4trim = "' ";
                            String instrumentName = sEmitentNames[i].Value.Trim(chars4trim.ToCharArray());

                            emitents.Add(new EmitentInfo(
                                                            market.marketId,
                                                            market.marketName,
                                                            Convert.ToInt32(sEmitentIds[i].Value),
                                                            instrumentName,
                                                            sEmitentCodes[i].Value.Trim(chars4trim.ToCharArray())
                                        ));
                            break;
                        }
                    }
                }
                return emitents;
            }
            else
            {
                // TODO ошибка парсинга
                return null;
            }
        }

        /// <summary>
        /// преобразует числовое значение периода, используемое на форме Финама, в строковое значение
        /// </summary>
        /// <param name="period">числовое значение периода, используемое на форме Финама</param>
        /// <returns>строковое значение, пригодное для названий папок</returns>
        public static string Period2String(int period)
        {
            switch (period)
            {
                case 1: return "tick";
                case 2: return "1min";
                case 3: return "5min";
                case 4: return "10min";
                case 5: return "15min";
                case 6: return "30min";
                case 7: return "1h";
                case 8: return "1d";
                case 9: return "1w";
                case 10: return "1m";
                case 11: return "1hFrom1030";
                default: return "error";
            }
        }

        public static string Download(Settings settings, EmitentInfo emitent)
        {
            int format;
            if (settings.period == 1) format = 11;
            else format = 5;

            string link = String.Format("http://195.128.78.52/{0}.{1}?d=d&market={2}&em={3}&p={4}&df={5}&mf={6}&yf={7}&dt={8}&mt={9}&yt={10}&f={11}&e=.{12}&datf={13}&cn={14}&dtf=1&tmf=1&MSOR=0&sep=3&sep2=1&at=1",
                emitent.code,
                "csv",
                emitent.marketId,
                emitent.id,
                settings.period,
                settings.from.Day,
                settings.from.Month - 1,
                settings.from.Year,
                settings.to.Day,
                settings.to.Month - 1,
                settings.to.Year,
                emitent.code,
                "csv",
                format,
                emitent.code
                    );

            System.Net.WebClient webClient = InitWebClient(settings);
            webClient.Headers.Add("Referer", @"http://www.finam.ru/analysis/export/default.asp");

            return webClient.DownloadString(link);
        }
    }

    /// <summary>
    /// Информация о эмитенте, а если id = -1, то о секции (рынке)
    /// </summary>
    public class EmitentInfo
    {
        public int marketId = -1;
        public int id = -1;
        public String marketName = String.Empty;
        public String name = String.Empty;
        public String code = String.Empty;
        public bool checed = false;

        public EmitentInfo() { } // требуется для сериализации

        public EmitentInfo(int marketId, String marketName, int emitentId, String emitentName, String emitentCode)
        {
            this.marketId = marketId;
            this.id = emitentId;
            this.marketName = marketName;
            this.name = emitentName;
            this.code = emitentCode;
        }
    }

    /// <summary>
    /// Информация о баре
    /// </summary>
    class Bar
    {
        public DateTime dt;
        public Double open = 0;
        public Double high = 0;
        public Double low = 0;
        public Double close = 0;
        public Double volume = 0;
        public Int64 id = 0;
    }

    /// <summary>
    /// Данные по одному инструменту/периоду
    /// </summary>
    class Bars
    {
        private List<Bar> list = new List<Bar>();
        public EmitentInfo emitent { get; set; }
        //public DateTime from { get; set; }
        //public DateTime to { get; set; }

        public Bar this[int i] { get { return list[i]; } }
        public int Count { get { return list.Count ; } }
        public DateTime Last { get { if (Count > 0) return list[Count - 1].dt; else return DateTime.Today.AddDays(-1); } }

        public Bars(EmitentInfo emitent)
        {
            this.emitent = emitent;
        }

        /// <summary>
        /// Сохраняю информацию о сделках в WL файл
        /// </summary>
        /// <param name="fileName">Имя WL файла</param>
        public void Save(string fileName)
        {
            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));

            BinaryWriter wlFile = new BinaryWriter(File.Open(fileName,FileMode.Create,FileAccess.Write));

            wlFile.Write(list.Count);
            for (int bar = 0; bar < list.Count; bar++)
            {
                {
                    wlFile.Write((double)list[bar].dt.ToOADate());
                    wlFile.Write((float)list[bar].open);
                    wlFile.Write((float)list[bar].high);
                    wlFile.Write((float)list[bar].low);
                    wlFile.Write((float)list[bar].close);
                    wlFile.Write((float)list[bar].volume);
                }
            }
            wlFile.Close();
        }

        public void SaveCSV(string fileName)
        {
            if (list.Count == 0)
                return;

            if (!Directory.Exists(Path.GetDirectoryName(fileName)))
                Directory.CreateDirectory(Path.GetDirectoryName(fileName));

            StreamWriter csvFile = new StreamWriter(File.Open(fileName,FileMode.Create, FileAccess.Write) );

            int period;
            if ((list[0].id == 0) && (list[0].low == list[0].high))
            {
                period = 1;
                csvFile.WriteLine("<DATE>,<TIME>,<LAST>,<VOL>,<ID>");
            }
            else
            {
                period = 2;
                csvFile.WriteLine("<DATE>;<TIME>;<OPEN>;<HIGH>;<LOW>;<CLOSE>;<VOL>");
            }

            foreach (Bar bar in list)
            {
                if (period == 1)
                {
                    csvFile.Write(bar.dt.ToString("yyyyMMdd;HHmmss;"));
                    csvFile.Write(bar.close.ToString());
                    csvFile.Write(';');
                    csvFile.Write(bar.volume.ToString());
                    csvFile.Write(';');
                    csvFile.WriteLine(bar.id.ToString());
                }
                else
                {
                    csvFile.Write(bar.dt.ToString("yyyyMMdd;HHmmss;"));
                    csvFile.Write(bar.open.ToString());
                    csvFile.Write(';');
                    csvFile.Write(bar.high.ToString());
                    csvFile.Write(';');
                    csvFile.Write(bar.low.ToString());
                    csvFile.Write(';');
                    csvFile.Write(bar.close.ToString());
                    csvFile.Write(';');
                    csvFile.WriteLine(bar.volume.ToString());
                }
            }
            csvFile.Close();
        }


        /// <summary>
        /// указывает, на последний вставленный элемент в bars
        /// используется в Add для ускарения поиска нужного места вставки в list
        /// </summary>
        private int last_bar = 0; 
        /// <summary>
        /// Добавляет бар, проверяя его уникальность и поддерживая сортировку по времени
        /// </summary>
        /// <param name="bar">Вставляемый бар</param>
        private void Add(Bar bar)
        {
            if (list.Count == 0)
            {
                list.Add(bar);
                return;
            }
            // устанавливаю last_bar на нужную позицию
            if ((last_bar < 0) || (last_bar >= list.Count))
                last_bar = 0;
            while ((last_bar > 0) && (list[last_bar].dt > bar.dt))
                --last_bar;
            while ((last_bar < list.Count) && (list[last_bar].dt < bar.dt))
                ++last_bar;
            // и если такого бара ещё нет, то вставляю
            if ((last_bar == list.Count) || ((list[last_bar].dt != bar.dt) && (bar.id == 0)) || (list[last_bar].id != bar.id))
                list.Insert(last_bar, bar);
        }

        public void LoadWL(string fileName)
        {
            if (!File.Exists(fileName)) return;
            
            BinaryReader wlFile = new BinaryReader(File.Open(fileName, FileMode.Open, FileAccess.Read));
            try
            {
                for (int count = wlFile.ReadInt32(); count > 0 ; --count)
                {
                    Bar bar = new Bar();
                    bar.dt = DateTime.FromOADate(wlFile.ReadDouble());
                    bar.open = wlFile.ReadSingle();
                    bar.high = wlFile.ReadSingle();
                    bar.low = wlFile.ReadSingle();
                    bar.close = wlFile.ReadSingle();
                    bar.volume = wlFile.ReadSingle();
                    Add(bar);
                }
            }
            finally
            {
                wlFile.Close();
            }
        }

        public void LoadCSV(TextReader csvFile)
        {
            int period = 0;
            string line = csvFile.ReadLine();
            if (line == "<DATE>,<TIME>,<LAST>,<VOL>,<ID>")
                 period = 1;
            if (line == "<DATE>,<TIME>,<OPEN>,<HIGH>,<LOW>,<CLOSE>,<VOL>")
                 period = 2;
            if (period != 0)
            {
                // Читаю все строки CSV
                while ((line = csvFile.ReadLine()) != null)
                {
                    // Распарсить строку
                    string[] attr = line.Split(';');
                    if (attr[0] != emitent.code)
                    // TODO добавить проверку PER
                    {
                        //TODO неверный фармат
                        break;
                    }
                    Bar bar = new Bar();
                    bar.dt = DateTime.ParseExact(attr[0] + attr[1], "yyyyMMddHHmmss", null);

                    if (period == 1)
                    {
                        bar.close = Convert.ToDouble(attr[2].Replace('.', ','));
                        bar.volume = Convert.ToDouble(attr[3].Replace('.', ','));
                        bar.id  = Convert.ToInt64(attr[4].Replace('.', ','));
                        bar.open = bar.close;
                        bar.high = bar.close;
                        bar.low = bar.close;
                    }
                    else
                    {
                        bar.open = Convert.ToDouble(attr[2].Replace('.', ','));
                        bar.high = Convert.ToDouble(attr[3].Replace('.', ','));
                        bar.low = Convert.ToDouble(attr[4].Replace('.', ','));
                        bar.close = Convert.ToDouble(attr[5].Replace('.', ','));
                        bar.volume = Convert.ToDouble(attr[6].Replace('.', ','));
                    }

                    Add(bar);
                }
            }
            else
            {
                //TODO Неверный фармат 
            }

        }

        // освобождаю память
        public void Clear()
        {
            list.Clear();
        }
    }
}