using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using OpenWealth;
using OpenWealth.Simple;

namespace FDownloader
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

    public static class FinamHelper
    {
        // подключаю log4net для ведения лога
        private static readonly ILog l = Core.GetLogger(typeof(FinamHelper));

        public static Object Lock = new Object();

        /// <summary>
        /// Создается, и инициируется WebClient
        /// </summary>
        /// <param name="settings">Настройки прокси</param>
        /// <returns></returns>
        private static System.Net.WebClient InitWebClient(Settings settings)
        {
            l.Debug("Создаю WebClient");
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

        public static List<EmitentInfo> Emitents;

        /// <summary>
        /// Скачиваю список эмитентов из финама
        /// </summary>
        /// <param name="settings">Настройки прокси</param>
        /// <returns>Список эмитентов</returns>
        public static List<EmitentInfo> DownloadEmitents(Settings settings)
        {
            l.Debug("Скачиваю список эмитентов из финама");
            System.Net.WebClient webClient = InitWebClient(settings);

            string marketsString = string.Empty;

            try
            {
                // скачиваю интерфейс
                marketsString = webClient.DownloadString(@"http://www.finam.ru/analysis/export/default.asp");
            }
            catch (Exception e)
            {
                l.Error("Не смог скачать интерфейс с финама " + e);
                return null;
            }

            String sOption = @"<option\s+?.*?value=""(?<id>[0-9]+)"".*?>(?<name>.+?)</option>";
            String sSelect = @"<select(.|\n)+?id=""market""(.|\n)+?(" + sOption + ")+?(.|\n)*?</select>";

            // Поиск нужного <select id="market">
            System.Text.RegularExpressions.Match m = System.Text.RegularExpressions.Regex.Match(marketsString, sSelect, System.Text.RegularExpressions.RegexOptions.IgnoreCase);

            if (!m.Success)
            {
                l.Error("Ошибка парсинга <select id=market>");
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

            instruments = instruments.Replace("\\'", "-"); // убрать кавычки из списка эмитентов, в противном случае строка не парситься

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
                        if (Convert.ToInt32(sEmitentMarkets[i].Value) == market.MarketId)
                        {
                            String chars4trim = "' ";
                            String instrumentName = sEmitentNames[i].Value.Trim(chars4trim.ToCharArray());

                            emitents.Add(new EmitentInfo(
                                                            market.MarketId,
                                                            market.MarketName,
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
                l.Error("Ошибка парсинга export.js");
                return null;
            }
        }

        public static List<EmitentInfo> LoadEmitents(String FileName)
        {
            List<EmitentInfo> emitents;
            if (System.IO.File.Exists(FileName))
            {
                System.Xml.Serialization.XmlSerializer nser = new System.Xml.Serialization.XmlSerializer(typeof(List<EmitentInfo>));
                System.IO.TextReader reader = new System.IO.StreamReader(FileName);
                emitents = (List<EmitentInfo>)nser.Deserialize(reader);
                reader.Close();
            }
            else
            {
                emitents = new List<EmitentInfo>();
            }
            return emitents;
        }

        public static void SaveEmitents(String FileName, List<EmitentInfo> emitents)
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(List<EmitentInfo>));
            System.IO.TextWriter writer = new System.IO.StreamWriter(FileName);
            ser.Serialize(writer, emitents);
            writer.Close();
        }

        public static string Download(Settings settings, EmitentInfo emitent, DateTime date)
        {
            string link = String.Format("http://195.128.78.52/{0}.{1}?d=d&market={2}&em={3}&p={4}&df={5}&mf={6}&yf={7}&dt={8}&mt={9}&yt={10}&f={11}&e=.{12}&datf={13}&cn={14}&dtf=1&tmf=1&MSOR=0&sep=3&sep2=1&at=1",
                emitent.Code,
                "csv",
                emitent.MarketId,
                emitent.Id,
                1,//settings.period,
                date.Day,
                date.Month - 1,
                date.Year,
                date.Day,
                date.Month - 1,
                date.Year,
                emitent.Code,
                "csv",
                11,//format, if (settings.period == 1) format = 11; else format = 5;
                emitent.Code
                    );

            l.Debug("Скачиваю " + link);

            System.Net.WebClient webClient = InitWebClient(settings);
            webClient.Headers.Add("Referer", @"http://www.finam.ru/analysis/export/default.asp");

            string result = string.Empty;
            try
            {
                result = webClient.DownloadString(link);
            }
            catch (Exception e)
            {
                result = "Exception";
                l.Info("Ошибка при скачивании " + e);
            }

            return result;
        }

        public static void LoadCSV(TextReader csvFile, ISymbol symbol)
        {
            l.Debug("Читаю bars из csv");

            string line = csvFile.ReadLine();
            if (line != "<DATE>;<TIME>;<LAST>;<VOL>;<ID>")
            {
                l.Error("Неверный формат csv файла " + line);
                return;
            }

            List<Tick> csvBars = new List<Tick>(1000);

            System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo("en-US", false).NumberFormat;
            nfi.NumberDecimalSeparator = ",";
            string[] separator = { ";" };

            Int64 lastId = -1;
            bool idErrorFlag = false;

            // Читаю все строки CSV
            while ((line = csvFile.ReadLine()) != null)
            {
                // Распарсить строку
                string[] attr = line.Split(separator, StringSplitOptions.None);

                DateTime dt = ParseDateTimeWithCache(attr[0] + attr[1]);

                try
                {
                    Double price = Convert.ToDouble(attr[2].Replace('.', ','), nfi);
                    Double volume = Convert.ToDouble(attr[3].Replace('.', ','), nfi);
                    Int64 id = Convert.ToInt64(attr[4].Replace('.', ','), nfi);

                    if (id > int.MaxValue)
                    {
                        l.Error("Случилось страшное!!!!! id сделки для " + symbol + " в csv превысил размер int'а и стал равен " + id);
                        id = 0;
                    }

                    // проверяю, что полученные id корректны
                    if ((!idErrorFlag) && (lastId >= id))
                    {
                        idErrorFlag = true;
                        l.Error("Ошибка id для " + symbol + " " + attr[0]);
                    }
                    lastId = id;

                    csvBars.Add(new Tick(dt, (int)id, (float)price, (int)volume));
                }

                catch (Exception e)
                {
                    l.Error("Ошибка при парсинге строки " + symbol + " " + line + " " + e);
                }
            }

            if (csvBars.Count > 0)
            {
                // Пушу в чистовую
                IBars bars = Core.Data.GetBars(symbol, Core.Data.GetScale(ScaleEnum.tick, 1));
                IBar forId = bars.Get(csvBars[0].DT);
                int genID = 0;
                if (forId != null)
                    genID = (int)forId.Number;

                foreach (Tick t in csvBars)
                    if (idErrorFlag)
                        bars.Add(null, new Tick(t.DT, (int)++genID, (float)t.Close, (int)t.Volume));
                    else
                        bars.Add(null, t);
            }
        }

        // Данные переменные необходимы для работы ParseDateTimeWithCache
        static string dateTimeCacheString = string.Empty;
        static DateTime dateTimeCacheDateTime = new DateTime();
        //        static readonly string[] formats = { "dd.MM.yyyy HH:mm:ss", "MM/dd/yyyy hh:mm:sstt", "MM/dd/yyyy HH:mm:ss", "G" };
        static readonly string[] formats = { "yyyyMMddHHmmss" };
        static readonly CultureInfo cultInfo = CultureInfo.CurrentCulture;
        /// <summary>
        /// Если данная строка толькочто преобразовывалась в DateTime то будет использовано предыдущий результат
        /// Потребовалось ввести при профилировании GetDeals
        /// </summary>
        static DateTime ParseDateTimeWithCache(string s)
        {
            lock (dateTimeCacheString)
            {
                if (dateTimeCacheString == s)
                    return dateTimeCacheDateTime;
                try
                {
                    dateTimeCacheDateTime = DateTime.ParseExact(s, formats, cultInfo, DateTimeStyles.None);
                }
                catch (Exception e)
                {
                    l.Error("Не смог преобразовать строку к дате " + s, e);
                }
                dateTimeCacheString = s;
                return dateTimeCacheDateTime;
            }
        }

    }

    /// <summary>
    /// Информация о эмитенте, а если id = -1, то о секции (рынке)
    /// </summary>
    public class EmitentInfo
    {
        public int MarketId = -1;
        public int Id = -1;
        private string _marketName = String.Empty;
        public string MarketName 
        {
            get
            {
                if ((_marketName == "Фьючерсы ФОРТС") || (_marketName == "ФОРТС Архив"))
                    return "ФОРТС";
                return _marketName;
            }
            set
            {
                _marketName = value;
            }
        }
        public string MarketName4gui
        {
            get
            {
                return _marketName;
            }
        }
        public string Name = String.Empty;
        public string Code = String.Empty;
        public bool Checked = false;

        public EmitentInfo() { } // требуется для сериализации

        public EmitentInfo(int marketId, String marketName, int emitentId, String emitentName, String emitentCode)
        {
            this.MarketId = marketId;
            this.Id = emitentId;
            this.MarketName = marketName;
            this.Name = emitentName;
            this.Code = emitentCode;
        }
    }
}