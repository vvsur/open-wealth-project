using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

using System.IO;

namespace OpenWealth.DataManager
{
    public class Data : IDataManager, IPlugin, IDescription
    {
        static ILog l = Core.GetLogger(typeof(Data).FullName);

        #region Реализация интерфейса IPlugin и конструкторы

        public Data()
        {
            if (Core.GetGlobal("data") != null)
                l.Error("переменная data уже установленна");
            else
            {
                Core.SetGlobal("data", this);
                TicksFiles.Init();
            }
        }

        public void Init()
        {
            l.Debug("Инициирую RndDataSource");
            IInterface interf = Core.GetGlobal("Interface") as IInterface;

            if (interf != null)
            {
                interf.AddMenuItem("Данные", "Объединение мелких файлов (долго!)", null, menu_Concat);
                interf.AddMenuItem("Данные", "Экспорт данных", null, menu_Export);
            }
        }

        void menu_Export(object sender, EventArgs e)
        {
            ExportForm f = new ExportForm();

            IInterface interf = Core.GetGlobal("Interface") as IInterface;
            if (interf != null)
                f.MdiParent = interf.GetMainForm();

            f.Show();
        }

        void menu_Concat(object sender, EventArgs e)
        {
            IScale tickScale = GetScale(ScaleEnum.tick, 1);
            foreach (IMarket m in markets)
                foreach (ISymbol s in m.GetSymbols())
                {
                    Ticks ticks = GetBars(s, tickScale) as Ticks;
                    if (ticks != null)
                        ticks.ticksFileList.Concat();
                }
        }

        public void Stop()
        {
            TicksFiles.Save();
            TicksFileSaver.Stop();
        }

        #endregion Реализация интерфейса IPlugin

        //public IBars Get(ISymbol symbol, IScale scale, DateTime from, DateTime to, int maxCount);
        //public IBars Get(ISymbol symbol, IScale scale, int count);

        List<IMarket> markets = new List<IMarket>();
        public IMarket GetMarket(string name)
        {
            lock (markets)
            {
                foreach (IMarket m in markets)
                    if (m.Name == name)
                        return m;

                IMarket market = new Market(name);
                markets.Add(market);

                // вызывать событие
                EventHandler<EventArgs> changeMarkets = ChangeMarkets;
                if (changeMarkets != null)
                    changeMarkets(this, null);

                return market;
            }
        }

        public IEnumerable<IMarket> GetMarkets() { return markets; }

        public event EventHandler<EventArgs> ChangeMarkets;

        Dictionary<string, IBars> barss = new Dictionary<string, IBars>();
        public IBars GetBars(ISymbol symbol, IScale scale)
        {
            if (l.IsDebugEnabled)
                l.Debug("GetBars " + symbol + " " + scale);
            lock (barss)
            {
                string key = symbol + "." + scale;
                if (barss.ContainsKey(key))
                    return barss[key];

                IBars newBars;
                if ((scale.scaleType == ScaleEnum.tick) && (scale.interval == 1))
                {
                    newBars = new Ticks(symbol);
                }
                else
                    newBars = new AggregateBars(this, symbol, scale);

                barss.Add(key, newBars);

                return newBars;
            }
        }

        public IBars GetBars(string marketName, string symbol, ScaleEnum scale, int interval)
        {
            return GetBars(GetSymbol(marketName, symbol), GetScale(scale, interval));
        }

        #region Symbol

        public ISymbol GetSymbol(string marketName, string name)
        {
            if ((marketName == null) || (name == null))
            {
                l.Error("GetSymbol ((marketName == null) || (name == null))");
                throw new ArgumentNullException("GetSymbol ((marketName == null) || (name == null))");
            }

            if (marketName.IndexOf('.') != -1)
            {
                l.Warn("marketName не может содержать точку " + marketName);
                return GetSymbol(string.Empty, string.Concat(marketName, ".", name));
            }

            return GetMarket(marketName).GetSymbol(name);
        }

        public ISymbol GetSymbol(string marketNameDotSymbolName)
        {
            string[] split = marketNameDotSymbolName.Split('.');
            
            if (split.Length < 2)
                return GetSymbol(string.Empty, marketNameDotSymbolName);

            for (int i = 2; i < split.Length; ++i)
                split[1] = string.Concat(split[1], ".", split[i]);

            return GetSymbol(split[0], split[1]);
        }

        public void DelSymbol(ISymbol symbol)
        {
            throw new NotImplementedException("DelSymbol");
            // Удаляю все Bars для данного символа
            // Удаляю сам Symbol
            // Если маркет больше не содержит Symbol, он должен быть удален?
        }

        #endregion Symbol

        #region Scale

        List<IScale> scales =new List<IScale>();
        public IScale GetScale(ScaleEnum scale, int interval, int beginning)
        {
            foreach (IScale s in scales)
                if ((s.beginning == beginning) && (s.scaleType == scale) && (s.interval == interval))
                    return s;

            IScale newS = new Scale(scale, interval, beginning);
            scales.Add(newS);
            return newS;
        }
        public IScale GetScale(ScaleEnum scale, int interval)
        {
            return GetScale(scale, interval, 0);
        }

        #endregion Scale

        #region Реализация интерфейса IDescription
        public string Name { get { return "Модуль Данные"; } }
        public string Description { get { return "Получает данные от провайдеров и отдает желающим. Не сохраняет данные."; } }
        public string URL { get { return "www.OpenWealth.ru"; } }
        #endregion Реализация интерфейса IDescription

    }
}
