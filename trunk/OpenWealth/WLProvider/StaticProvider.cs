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
        
        IData data;

        private bool _cancelUpdate;
        private BarDataStore _dataStore;
        private IDataUpdateMessage _dataUpdateMsg;
        private bool _updating = false;

        /// Wizard pages
        private WizardPage Page;
                
        public override void Initialize(IDataHost dataHost)
        {
            l.Debug("Initialize " + dataHost.GetType().FullName );

            base.Initialize(dataHost);
            this._dataStore = new BarDataStore(dataHost, this);

            // инициализация ядра и плагинов
            if (Core.SingleInit())
            {
                Core.Init(
                    System.IO.Path.Combine(
                        System.IO.Path.GetDirectoryName(
                            System.Reflection.Assembly.GetExecutingAssembly().Location
                                                       )
                        , "OWPlugin")
                      );
                Core.LoadPlugin(System.Reflection.Assembly.GetExecutingAssembly().Location);
                Core.LoadPlugin();
                Core.InitPlugin();
            }
            this.data = Core.GetGlobal("data") as IData;
            if (this.data == null)
                throw new Exception("Не найден модуль data");
        }

        // Checking connection with server (not implemented here)
        public override void CheckConnectionWithServer()
        {
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
        public override bool CanModifySymbols
        {
            get
            {
                return true;
            }
        }

        // Indicates that provider supports deleting symbols
        public override bool CanDeleteSymbolDataFile
        {
            get
            {
                return true;
            }
        }

        // Strategy Monitor support not implemented
        public override bool CanRequestUpdates
        {
            get
            {
                return false;
            }
        }

        // Должен вернуть True, если данный scale поддерживается
        // Вызывается, когда меняешь таймфрэйм через интерфейс у существующего символа
        public override bool SupportsDynamicUpdate(BarScale scale)
        {
            return true;
        }

        // Indicates that dataset updates are supported by provider
        public override bool SupportsDataSourceUpdate
        {
            get
            {
                return true;
            }
        }

        // Indicates that provider updates are supported ("Update all data for..." in the Data Manager)
        public override bool SupportsProviderUpdate
        {
            get
            {
                return true;
            }
        }

        #endregion

        #region Wizard pages

        public override System.Windows.Forms.UserControl WizardFirstPage()
        {
            if (Page == null)
                Page = new WizardPage();

            Page.Initialize(_dataStore.RootPath);

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

        // TODO You can return a suggested DataSet name here if you like
        public override string SuggestedDataSourceName
        {
            get { return "OpenWealth"; }
        }

        public override string ModifySymbols(DataSource ds, List<string> symbols)
        {
            l.Debug("ModifySymbols from " + ds.DSString + " !!!TO!!! " + symbols.ToString());
            ds.DSString = symbols.ToString();
            return ds.DSString;
        }

        public override void DeleteSymbolDataFile(DataSource ds, string symbol)
        {
            l.Debug("DeleteSymbolDataFile " + symbol +" from " +ds.DSString);
            this._dataStore.RemoveFile(symbol, ds.Scale, ds.BarInterval);
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
        
        public override Bars RequestData(DataSource ds, string symbol, DateTime startDate, DateTime endDate, int maxBars, bool includePartialBar)
        {
            l.Debug("RequestData " + symbol + " from " + startDate.ToString("yy.MM.dd hh:mm:ss") + " to " + endDate.ToString("yy.MM.dd hh:mm:ss") + " maxBars=" + maxBars.ToString() + " includePartialBar=" + includePartialBar.ToString());

            Bars bars = new Bars(symbol.Trim(new char[] { ' ', '"' }), ds.Scale, ds.BarInterval);
            Bars barsNew;

            if (this._dataStore.ContainsSymbol(symbol, ds.Scale, ds.BarInterval))
            {
                if ((base.DataHost.OnDemandUpdateEnabled || this._updating) && this.UpdateRequired(ds, symbol))
                {
                    DateTime lastBar = this._dataStore.SymbolLastUpdated(symbol, BarScale.Daily, 0);

                    barsNew = IBars2WLBars(symbol, ds.Scale, ds.BarInterval, data.GetBars(data.GetSymbol(symbol), WLScale2Scale(ds.Scale, ds.BarInterval)));

                    if (barsNew != null)
                        this.LoadAndUpdateBars(ref bars, barsNew);
                }

                _dataStore.LoadBarsObject(bars, startDate, DateTime.MaxValue, maxBars);
            }
            else
                if (base.DataHost.OnDemandUpdateEnabled || this._updating)
                {
                    bars = IBars2WLBars(symbol, ds.Scale, ds.BarInterval, data.GetBars(data.GetSymbol(symbol), WLScale2Scale(ds.Scale, ds.BarInterval)));
                    this._dataStore.SaveBarsObject(bars);
                    this._dataStore.LoadBarsObject(bars);
                }

            return bars;
        }

        public override void UpdateDataSource(DataSource ds, IDataUpdateMessage dataUpdateMsg)
        {
            l.Debug("UpdateDataSource");

            this._dataUpdateMsg = dataUpdateMsg;
            this._cancelUpdate = false;
            this._updating = true;
            Bars barsNew; // The Bars object for a newly downloaded symbol, or just the new data for an existing/updated symbol

            /* Main loop */
            try
            {
                // User requested 'Cancel update'
                if (this._cancelUpdate) return;

                List<string> up2date = new List<string>();
                List<string> updateRequired = new List<string>();
                List<string> newSymbols = new List<string>();
                string sym;

                foreach (string s in ds.Symbols)
                {
                    sym = s;

                    if (!this.UpdateRequired(ds, sym))
                        up2date.Add(sym);
                    else if (this._dataStore.ContainsSymbol(sym, ds.Scale, ds.BarInterval))
                        updateRequired.Add(sym);
                    else
                        if (!this._dataStore.ContainsSymbol(sym, ds.Scale, ds.BarInterval))
                            newSymbols.Add(sym);

                }
                // For debugging:
                dataUpdateMsg.DisplayUpdateMessage(
                    "Up-to-date symbols: " + up2date.Count.ToString() + ", " +
                    "Update required for: " + updateRequired.Count.ToString() + ", " +
                    "New symbols: " + newSymbols.Count.ToString());

                /* 1. Symbols already up-to-date */

                // Process symbols which require no data update
                if (up2date.Count > 0)
                {
                    string alreadyUp2Date = null;
                    foreach (string str in up2date)
                        alreadyUp2Date += str + ",";

                    dataUpdateMsg.DisplayUpdateMessage("Symbols already up to date: " + alreadyUp2Date);
                    dataUpdateMsg.ReportUpdateProgress((up2date.Count * 100) / ds.Symbols.Count);
                }

                /* 2. Symbols to update */

                Bars bars1;

                if (updateRequired.Count > 0)
                {
                    foreach (string s in updateRequired)
                    {
                        try
                        {
                            if (!this._cancelUpdate)
                            {
                                bars1 = new Bars(s, BarScale.Daily, 0);

                                if (_dataStore.ContainsSymbol(s, ds.Scale, ds.BarInterval))
                                    _dataStore.LoadBarsObject(bars1);

//TODO не коректно возвращаю    barsNew = zaglushka.RequestData(ds, s, bars1.Date[bars1.Count - 1], DateTime.Now, int.MaxValue, true);
                                barsNew = IBars2WLBars(s, ds.Scale, ds.BarInterval, data.GetBars(data.GetSymbol(s), WLScale2Scale(ds.Scale, ds.BarInterval)));
                                this.LoadAndUpdateBars(ref bars1, barsNew);
                            }
                            else
                                return;
                        }
                        catch (Exception e)
                        {
                            dataUpdateMsg.DisplayUpdateMessage("Error: " + e.Message);
                        }
                    }
                }

                DateTime maxValue = DateTime.MaxValue;
                string[] newSymbolsArray = new string[newSymbols.Count];

                if (newSymbols.Count > 0)
                {
                    foreach (string str in newSymbols)
                    {
                        DateTime timeOfNewSymbol = this._dataStore.SymbolLastUpdated(str, ds.Scale, ds.BarInterval); //.Date;
                        if (timeOfNewSymbol < maxValue)
                            maxValue = timeOfNewSymbol;
                    }
                }

                dataUpdateMsg.DisplayUpdateMessage("Updating...");

                /* 3. New symbols */

                // Load the Bars object from BarDataStore for updating
                Bars bars2;

                foreach (string s in newSymbols)
                {
                    try
                    {
                        if (!this._cancelUpdate)
                        {
                            bars2 = new Bars(s, ds.Scale, ds.BarInterval);
                            _dataStore.LoadBarsObject(bars2);

                            // Google doesn't provide company names, so we'll get them from Yahoo!
                //TODO возвращать название компании
                //            if (zaglushka.GetCompanyName(s) != null)
                //                bars2.SecurityName = zaglushka.GetCompanyName(s);
                            bars2.SecurityName = s;

                            // After some trial and error, I figured out that setting the starting date to 1/1/1971 allows to fetch all available data
                    //        barsNew = zaglushka.RequestData(ds, s, DateTime.MinValue , DateTime.Now, int.MaxValue, true);
                            barsNew = IBars2WLBars(s, ds.Scale, ds.BarInterval, data.GetBars(data.GetSymbol(s), WLScale2Scale(ds.Scale, ds.BarInterval))); 
                            dataUpdateMsg.DisplayUpdateMessage("Symbol: " + s + ", existing bars : " + bars2.Count + ", new bars: " + barsNew.Count);
                            this.LoadAndUpdateBars(ref bars2, barsNew);
                        }
                        else
                            return;
                    }
                    catch (Exception e)
                    {
                        dataUpdateMsg.DisplayUpdateMessage("Error: " + e.Message);
                    }
                }
            }
            catch (Exception e)
            {
                dataUpdateMsg.DisplayUpdateMessage("Error: " + e.Message);
            }
            finally
            {
                this._updating = false;
            }
        }

        public override void UpdateProvider(IDataUpdateMessage dataUpdateMsg, List<DataSource> dataSources, bool updateNonDSSymbols, bool deleteNonDSSymbols)
        {
            l.Debug("UpdateProvider dataSources.Count=" + dataSources.Count.ToString());

            this._cancelUpdate = false;

            try
            {
                dataUpdateMsg.DisplayUpdateMessage("Updating daily data from Google Finance...");

                // User requested 'Cancel update'
                if (this._cancelUpdate) return;

                // NOTE: Since only Daily data is made available by Google, this code does not support multiple bar scales
                // If your vendor supports different bar scales, you will need to handle it
                // by looping for all existing bar data scales in BarDataStore!

                // Create a list of all "visible" symbols of the provider
                List<string> allVisibleSymbols = new List<string>();
                foreach (DataSource ds in dataSources)
                    foreach (string sym in ds.Symbols)
                        if (!allVisibleSymbols.Contains(sym))
                            allVisibleSymbols.Add(sym);

                // If 'Update Non DS Symbols' is selected in DM, need to build the *complete* symbol list (incl. which have been deleted from existing DataSets)
                List<string> allExistingSymbols = new List<string>();
                allExistingSymbols.AddRange(allVisibleSymbols);
                IList<string> existingSymbols = this._dataStore.GetExistingSymbols(BarScale.Daily, 0);
                if (updateNonDSSymbols)
                {
                    foreach (string str in existingSymbols)
                        if (!allExistingSymbols.Contains(str))
                            allExistingSymbols.Add(str);
                }

                // Create a virtual DataSource on-the-fly that contains the entire symbol list of the provider
                DataSource ds_ = new DataSource(this);
                ds_.BarDataScale = new BarDataScale(BarScale.Daily, 0);
                ds_.DSString = allExistingSymbols.ToString();

                // Update all by creating a virtual DataSet
                this._updating = true;
                this.UpdateDataSource(ds_, dataUpdateMsg);

                // Delete symbols not present in existing DataSets
                if (deleteNonDSSymbols)
                {
                    int num = 0;
                    string str3 = "";
                    foreach (string str in existingSymbols)
                    {
                        if (!allVisibleSymbols.Contains(str))
                        {
                            num++;
                            // It's important to specify the right BarScale/Interval here (see note above re: multiple bar data scales)
                            this._dataStore.RemoveFile(str, BarScale.Daily, 0);
                            if (str3 != "")
                            {
                                str3 += ", ";
                            }
                            str3 = str3 + str;
                        }
                    }
                    if (num > 0)
                    {
                        dataUpdateMsg.DisplayUpdateMessage("Deleted Symbols " + str3);
                        dataUpdateMsg.DisplayUpdateMessage("Deleted " + num + " Symbol data files");
                    }
                }
                dataUpdateMsg.DisplayUpdateMessage("");

            }
            catch (Exception e)
            {
                dataUpdateMsg.DisplayUpdateMessage("Error: " + e.Message);
            }
            finally
            {
                this._updating = false;
                _dataUpdateMsg = null;
            }

        }

        public override void CancelUpdate()
        {
            l.Debug("CancelUpdate");
            this._cancelUpdate = true;
        }

        #endregion Implementing StaticDataProvider

        #region Helper methods
      
        private bool UpdateRequired(DataSource ds, string symbol)
        {
            bool result = false;

            // Last update time of a symbol as reported by the helpful BarsDataStore
            DateTime updateTime = this._dataStore.SymbolLastUpdated(symbol, ds.Scale, ds.BarInterval);

            // Update is required when symbol's date isn't current
            // As Google's got no partial bar for today, let's not request today's data if the last updated bar is "yesterday":
            if (DateTime.Now.Date > updateTime.Date.AddDays(1))
                result = true;

            return result;
        }
        
        private void LoadAndUpdateBars(ref Bars bars, Bars barsUpdate)
        {
            this._dataStore.LoadBarsObject(bars);
            bars.Append(barsUpdate);
            this._dataStore.SaveBarsObject(bars);
        }
        
        #endregion Helper methods


        #region Преобразование типов OpenWealth к WealthLab

        Bars IBars2WLBars(string symbol, BarScale scale, int barInterval, IBars bars)
        {
            Bars result = new Bars(symbol, scale, barInterval);
            foreach (IBar b in bars)
                result.Add(b.dt, b.open, b.high, b.low, b.close, b.volume);
            return result;
        }

        IScale WLScale2Scale(BarScale barScale, int barInterval)
        {
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



/*

public class FidelityStaticProvider : StaticDataProvider, ICustomSettings
{
    // Fields
    private const string a = ",.MB_ADV.A,.MB_ADV.N,.MB_ADV.Q,.MB_DEC.A,.MB_DEC.N,.MB_DEC.Q,.MB_NH.A,.MB_NH.N,.MB_NH.Q,.MB_NL.A,.MB_NL.N,.MB_NL.Q,.MB_TV.A,.MB_TV.N,.MB_TV.Q,.MB_UNC.A,.MB_UNC.N,.MB_UNC.Q,.STI.A,.STI.N,.STI.O,";
    private bool aa;
    private ManualResetEvent ab;
    private bool ac;
    private const string b = ";";
    private const string c = ".";
    private string d;
    private RoadRunner e;
    private MarketHours f;
    private BarDataStore g;
    private FidelityFMDFundamentalProvider h;
    private FidelityStaticProviderSettings i;
    private bool j;
    private int k;
    private int l;
    private bool m;
    private static bool n;
    private IDataUpdateMessage o;
    private string p;
    private bool q;
    private FidelityWizardPageStart r;
    private FidelityWizardPageSymbols s;
    private FidelityWizardPageGics t;
    private FidelityWizardImport u;
    private static readonly ILog v;
    private static DateTime w;
    private static bool x;
    private static b y;
    private bool z;

    // Methods
    static FidelityStaticProvider();
    public FidelityStaticProvider();
    private string a();
    private void a(bool A_0);
    private int a(int A_0);
    private int a(string A_0);
    private void a(Bars A_0);
    private bool a(BarScale A_0);
    private void a(DataSource A_0);
    private void a(ref Bars A_0);
    private bool a(Bars A_0, Bars A_1);
    private void a(ref Bars A_0, Bars A_1);
    private void a(Bars A_0, int A_1);
    private void a(BarScale A_0, int A_1);
    private bool a(DataSource A_0, string A_1);
    private Bars a(DataSource A_0, string A_1, DateTime A_2, DateTime A_3);
    private void b();
    private string b(string A_0);
    private void b(Bars A_0);
    private bool c(string A_0);
    private bool c(Bars A_0);
    public override void CancelUpdate();
    public void ChangeSettings(UserControl ui);
    public override void CheckConnectionWithServer();
    public override DataSource CreateDataSource();
    private void d();
    private void d(string A_0);
    private void d(Bars A_0);
    public override void DeleteSymbolDataFile(DataSource ds, string symbol);
    private void e();
    private void e(Bars A_0);
    private void g();
    public override MarketInfo GetMarketInfo(string symbol);
    public UserControl GetSettingsUI();
    public override void Initialize(IDataHost dataHost);
    public override string ModifySymbols(DataSource ds, List<string> symbols);
    public override void PopulateSymbols(DataSource ds, List<string> symbols);
    public void ReadSettings(ISettingsHost host);
    public override Bars RequestData(DataSource ds, string symbol, DateTime startDate, DateTime endDate, int maxBars, bool includePartialBar);
    public override void RequestUpdates(List<string> symbols, DateTime startDate, DateTime endDate, BarScale scale, int barInterval, IUpdateRequestCompleted requestCompleted);
    public override void SaveEditedSymbolDataFile(DataSource ds, Bars bars);
    public override bool SupportsDynamicUpdate(BarScale scale);
    public override void UpdateDataSource(DataSource ds, IDataUpdateMessage dataUpdateMsg);
    public override void UpdateProvider(IDataUpdateMessage dataUpdateMsg, List<DataSource> dataSources, bool updateNonDSSymbols, bool deleteNonDSSymbols);
    public override bool WarnUserAboutDelay(string symbol, BarScale scale, int barInterval);
    public override UserControl WizardFirstPage();
    public override UserControl WizardNextPage(UserControl currentPage);
    public override UserControl WizardPreviousPage(UserControl currentPage);
    public void WriteSettings(ISettingsHost host);

    // Properties
    public string BaseURL { get; set; }
    public override bool CanDeleteSymbolDataFile { get; }
    public override bool CanEditSymbolDataFile { get; }
    public override bool CanModifySymbols { get; }
    public override bool CanRequestUpdates { get; }
    public static bool comingFromWebService { get; set; }
    private string DataLocation { get; }
    public override BarDataStore DataStore { get; }
    public override string Description { get; }
    public override IList<DataBehaviorUserControl> ExtendedBehaviors { get; }
    public override string FriendlyName { get; }
    public override Bitmap Glyph { get; }
    private bool OnDemandUpdate { get; }
    public override string SuggestedDataSourceName { get; }
    public override bool SupportsDataSourceUpdate { get; }
    public override bool SupportsProviderUpdate { get; }
    public override string URL { get; }
    public FidelityWizardImport WizardImport { get; }

    // Nested Types
    private enum a
    {
        a = 60,
        b = 90,
        c = 120,
        d = 150,
        e = 180,
        f = 210,
        g = 240,
        h = 360
    }

    private enum b
    {
        a,
        b,
        c
    }
}

  
*/