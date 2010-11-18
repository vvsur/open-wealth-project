using System;
using System.Collections.Generic;
using System.Net;

using WealthLab;

namespace OpenWealth.WLProvider
{
    /// <summary>
    /// WLProvider
    /// </summary>
    public class StaticProvider : StaticDataProvider
    {
        static ILog l = Core.GetLogger(typeof(StaticProvider).FullName);

        IDataManager data;

        /// Wizard pages
        private WizardPage Page;
                
        public override void Initialize(IDataHost dataHost)
        {
            l.Debug("Initialize " + dataHost.GetType().FullName);

            base.Initialize(dataHost);

            this.data = Core.GetGlobal("data") as IDataManager;
            if (this.data == null)
                throw new Exception("Не найден модуль data");
        }

        // Checking connection with server (not implemented here)
        public override void CheckConnectionWithServer()
        {
            l.Debug("CheckConnectionWithServer");
            base.CheckConnectionWithServer();
        }

        #region Descriptive

        public override string FriendlyName
        {
            get { return "OpenWealth"; }
        }

        public override string Description
        {
            get { return "Provides historical stock data from OpenWelth"; }
        }

        public override System.Drawing.Bitmap Glyph
        {
            get { return Properties.Resources.Image1; }
        }

        public override string URL
        {
            get { return @"http://www.OpenWealth.ru/"; }
        }

        #endregion

        #region Provider capabilities

        // Indicates that provider supports modifying dataset composition on-the-fly
        // Base возвращает false
        // Если ставлю true, то PartialBar, формируемый стрим провайдером не отображается
        // и при этом, несмотря на изменение WealthLab.Bars отображаемого на экране, перерисовка не происходит
        // перересовка происходит только если вызвать событие перерисовки вручную (двойное нажатие алт-таб)
        public override bool CanModifySymbols
        {
            get
            {
                bool result = base.CanModifySymbols;
                l.Debug("CanModifySymbols " + result);
                return result;
            }
        }

        // Indicates that provider supports deleting symbols
        public override bool CanDeleteSymbolDataFile
        {
            get
            {
                bool result = base.CanDeleteSymbolDataFile;
                l.Debug("CanDeleteSymbolDataFile " + result);
                return result;
            }
        }

        // Strategy Monitor support not implemented
        public override bool CanRequestUpdates
        {
            get
            {
                bool result = base.CanRequestUpdates;
                l.Debug("CanRequestUpdates " + result);
                return result;
            }
        }

        // Должен вернуть True, если данный scale поддерживается
        // Вызывается, когда меняешь таймфрэйм через интерфейс у существующего символа
        // Если вернуть False, то WL не даст переключиться на данный таймфрейм
        public override bool SupportsDynamicUpdate(BarScale scale)
        {
            l.Debug("SupportsDynamicUpdate");
            return true;
        }

        // Indicates that dataset updates are supported by provider
        public override bool SupportsDataSourceUpdate
        {
            get
            {
                bool result = base.SupportsDataSourceUpdate;
                l.Debug("SupportsDataSourceUpdate " + result);
                return result;
            }
        }

        // Indicates that provider updates are supported ("Update all data for..." in the Data Manager)
        public override bool SupportsProviderUpdate
        {
            get
            {
                bool result = base.SupportsProviderUpdate;
                l.Debug("SupportsProviderUpdate " + result);
                return result;
            }
        }

        #endregion

        #region Wizard pages

        public override System.Windows.Forms.UserControl WizardFirstPage()
        {
            if (Page == null)
                Page = new WizardPage();

            Page.Initialize();
            return Page;
        }

        public override System.Windows.Forms.UserControl WizardNextPage(System.Windows.Forms.UserControl currentPage)
        {
            return null;
        }

        public override System.Windows.Forms.UserControl WizardPreviousPage(System.Windows.Forms.UserControl currentPage)
        {
            return null;
        }

        #endregion Wizard pages

        #region Implementing StaticDataProvider

        public override DataSource CreateDataSource()
        {
            l.Debug("CreateDataSource");

            if (this.Page == null)
                return null;

            DataSource ds = new DataSource(this);

            ds.DSString = Page.Symbols();

            ds.Scale = BarScale.Minute;
            ds.BarInterval = 1;

            return ds;
        }

        public override string SuggestedDataSourceName
        {
            get { return "OpenWealth"; }
        }

        public override string ModifySymbols(DataSource ds, List<string> symbols)
        {
            l.Debug("ModifySymbols from " + ds.DSString + " !!!TO!!! " + symbols.ToString());
            ds.DSString = string.Empty;
            foreach(string s in symbols)
            {
                ds.DSString = ds.DSString + " " + s;
            }
            ds.DSString = ds.DSString.TrimStart(' ');
            return ds.DSString;
        }

        public override void DeleteSymbolDataFile(DataSource ds, string symbol)
        {
            l.Debug("DeleteSymbolDataFile " + symbol +" from " +ds.DSString);
            //this._dataStore.RemoveFile(symbol, ds.Scale, ds.BarInterval);
        }

        public override void PopulateSymbols(DataSource ds, List<string> symbols)
        {
            l.Debug("PopulateSymbols " + ds.DSString);

            List<string> symbolList = new List<string>();
            string[] strArray = ds.DSString.Split(new char[] { ' ', ',', '\n', '\r' });
            foreach (string str in strArray)
            {
                symbolList.Add(str.Trim(new char[] { ' ', '\n', '\r' }));
            }
            symbols.AddRange(symbolList);
        }

        static Dictionary<IBars, WeakReference> barsCache = new Dictionary<IBars, WeakReference>();

        public override Bars RequestData(DataSource ds, string symbol, DateTime startDate, DateTime endDate, int maxBars, bool includePartialBar)
        {
            l.Debug("RequestData " + symbol + " from " + startDate.ToString("yy.MM.dd hh:mm:ss") + " to " + endDate.ToString("yy.MM.dd hh:mm:ss") + " maxBars=" + maxBars.ToString() + " includePartialBar=" + includePartialBar.ToString());            

            // Пытаюсь получить Bars из кэша
            WealthLab.Bars bars = null;

            OpenWealth.ISymbol owSymbol = data.GetSymbol(symbol);
            OpenWealth.IScale owScale = WLScale2Scale(ds.Scale, ds.BarInterval);
            OpenWealth.IBars owBars = data.GetBars(owSymbol, owScale);

            lock (barsCache)
            {
                if (barsCache.ContainsKey(owBars))
                {
                    l.Debug("bars содержится в Cache");
                    bars = barsCache[owBars].Target as Bars;
                    if (bars == null)
                    {
                        l.Debug("ссылка в Cache истекла");
                        bars = IBars2WLBars(symbol, ds.Scale, ds.BarInterval, owBars, includePartialBar);
                        barsCache[owBars].Target = bars;
                    }
                }
                else
                {
                    l.Debug("bars НЕ содержится в Cache");
                    bars = IBars2WLBars(symbol, ds.Scale, ds.BarInterval, owBars, includePartialBar);
                //    barsCache.Add(owBars, new WeakReference(bars));
                }                
            }

            return bars;
        }

        #endregion Implementing StaticDataProvider

        #region OnlyDebug
        
        public override void UpdateDataSource(DataSource ds, IDataUpdateMessage dataUpdateMsg)
        {
            base.UpdateDataSource(ds, dataUpdateMsg);
            l.Debug("UpdateDataSource");
        }
        public override void UpdateProvider(IDataUpdateMessage dataUpdateMsg, List<DataSource> dataSources, bool updateNonDSSymbols, bool deleteNonDSSymbols)
        {
            base.UpdateProvider(dataUpdateMsg, dataSources, updateNonDSSymbols, deleteNonDSSymbols);
            l.Debug("UpdateProvider dataSources.Count=" + dataSources.Count.ToString());
        }
        public override void CancelUpdate()
        {
            base.CancelUpdate();
            l.Debug("CancelUpdate");
        }

        #endregion OnlyDebug

        #region Преобразование типов OpenWealth к WealthLab

        // TODO Вынести в настройки  Интервал, после которого свеча закрывается не тиком из новой свечи, а по времени
        static TimeSpan sec3 = new TimeSpan(0, 0, 3);
        Bars IBars2WLBars(string symbol, WealthLab.BarScale scale, int barInterval, IBars bars, bool includePartialBar)
        {
            l.Debug("IBars2WLBars");
            WealthLab.Bars result = new WealthLab.Bars(symbol, scale, barInterval);

            IBar b = bars.First;

            while (b != bars.Last)
            {
                result.Add(b.dt, b.open, b.high, b.low, b.close, b.volume);
                b = bars.GetNext(b);
            }

//            bars.NewBarEvent += bars_NewBarEvent;
            
            if (b != null) // а значит указывает на Last
            {
                if ((!includePartialBar)&&((result.Date[result.Count - 1] + bars.scale.ToTimeSpan()) < (DateTime.Now - sec3)))
                    result.Add(b.dt, b.open, b.high, b.low, b.close, b.volume);
                if (includePartialBar)
                {
                    result.Open.PartialValue = b.open;;
                    result.Close.PartialValue = b.close;
                    result.High.PartialValue = b.high;
                    result.Low.PartialValue = b.low;
                    result.Volume.PartialValue = b.volume;
                }
            }
            
            l.Debug("IBars2WLBars result.Count " + result.Count);
            return result;
        }

        /* void bars_NewBarEvent(object sender, BarsEventArgs e)
        {
            // Получаю IBars, в который был добавлен бар
            OpenWealth.IBars owBars = sender as OpenWealth.IBars;
            if (owBars == null)
            {
                l.Error("bars_NewBarEvent owBars == null");
                return;
            }

            l.Debug("bars_NewBarEvent " + owBars);

            // Достаю из кэша WealthLab.Bars, в который эти изменения должны быть внесены
            WealthLab.Bars wlBars = null;
            lock (barsCache)
            {
                if (barsCache.ContainsKey(owBars))
                    wlBars = barsCache[owBars].Target as WealthLab.Bars;
            }

            if (wlBars == null)
            {
                l.Debug("wlBars не найден в barsCache");
                //owBars.ChangeBarEvent -= bars_ChangeBarEvent;
                owBars.NewBarEvent -= bars_NewBarEvent;
                return;
            }           

            IBar bar = owBars.GetPrevious(e.bar);
            if (bar != null)
            {
                int idx = wlBars.Date.IndexOf(bar.dt);
                if (idx < 0)
                {
                    if (wlBars.Count>0)
                        l.Debug("bars_NewBarEvent бар не найден " + e.bar.dt + " " + wlBars.Date[wlBars.Count-1]);
                    else
                        l.Debug("bars_NewBarEvent бар не найден wlBars.Count==0");
                    wlBars.Add(bar.dt, bar.open, bar.high, bar.low, bar.close, bar.volume);
                }
                else
                {
                    l.Debug("предыдущий бар уже содержется в wlBars");
                    wlBars.Open[idx] = bar.open;
                    wlBars.High[idx] = bar.high;
                    wlBars.Low[idx] = bar.low;
                    wlBars.Close[idx] = bar.close;
                    wlBars.Volume[idx] = bar.volume;
                }
            }
            else
                l.Debug("не найден предыдущий бар");
             
        }
        */

        IScale WLScale2Scale(BarScale barScale, int barInterval)
        {
            l.Debug("WLScale2Scale");

            int multiplier = barInterval;
            if (multiplier ==0)
                multiplier = 1;
            switch (barScale)
            {
                case BarScale.Daily:
                    return data.GetScale(ScaleEnum.sec, multiplier*60*60*24);
                case BarScale.Minute:
                    return data.GetScale(ScaleEnum.sec, multiplier*60);
                case BarScale.Monthly:
                    return data.GetScale(ScaleEnum.month, multiplier);
                case BarScale.Quarterly:
                    return data.GetScale(ScaleEnum.undefined, multiplier);//TODO определить таймфрейм
                case BarScale.Second:
                    return data.GetScale(ScaleEnum.sec, multiplier);
                case BarScale.Tick:
                    return data.GetScale(ScaleEnum.tick, multiplier);
                case BarScale.Weekly:
                    return data.GetScale(ScaleEnum.undefined, multiplier);//TODO определить таймфрейм
                case BarScale.Yearly:
                    return data.GetScale(ScaleEnum.undefined, multiplier);//TODO определить таймфрейм
                default: 
                    throw new Exception("Не известный BarScale ");
            }
            throw new Exception("не может того быть!");
        }




        #endregion
    }
}