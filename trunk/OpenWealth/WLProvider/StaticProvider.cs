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

        /// Wizard pages
        private WizardPage Page;
                
        public override void Initialize(IDataHost dataHost)
        {
            l.Debug("Initialize " + dataHost.GetType().FullName);

            base.Initialize(dataHost);

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
                l.Debug("CanModifySymbols");
                return true;
            }
        }

        // Indicates that provider supports deleting symbols
        public override bool CanDeleteSymbolDataFile
        {
            get
            {
                l.Debug("CanDeleteSymbolDataFile");
                return true;
            }
        }

        // Strategy Monitor support not implemented
        public override bool CanRequestUpdates
        {
            get
            {
                l.Debug("CanRequestUpdates");
                return true;
            }
        }

        // Должен вернуть True, если данный scale поддерживается
        // Вызывается, когда меняешь таймфрэйм через интерфейс у существующего символа
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
                l.Debug("SupportsDataSourceUpdate");
                return true;
            }
        }

        // Indicates that provider updates are supported ("Update all data for..." in the Data Manager)
        public override bool SupportsProviderUpdate
        {
            get
            {
                l.Debug("SupportsProviderUpdate");
                return true;
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
        
        public override Bars RequestData(DataSource ds, string symbol, DateTime startDate, DateTime endDate, int maxBars, bool includePartialBar)
        {
            l.Debug("RequestData " + symbol + " from " + startDate.ToString("yy.MM.dd hh:mm:ss") + " to " + endDate.ToString("yy.MM.dd hh:mm:ss") + " maxBars=" + maxBars.ToString() + " includePartialBar=" + includePartialBar.ToString());
            Bars bars = IBars2WLBars(symbol, ds.Scale, ds.BarInterval, data.GetBars(data.GetSymbol(symbol), WLScale2Scale(ds.Scale, ds.BarInterval)), includePartialBar);
            return bars;
        }

        public override void UpdateDataSource(DataSource ds, IDataUpdateMessage dataUpdateMsg)
        {
            l.Debug("UpdateDataSource");
        }

        public override void UpdateProvider(IDataUpdateMessage dataUpdateMsg, List<DataSource> dataSources, bool updateNonDSSymbols, bool deleteNonDSSymbols)
        {
            l.Debug("UpdateProvider dataSources.Count=" + dataSources.Count.ToString());
        }

        public override void CancelUpdate()
        {
            l.Debug("CancelUpdate");
        }

        #endregion Implementing StaticDataProvider

        #region Преобразование типов OpenWealth к WealthLab

        Bars IBars2WLBars(string symbol, BarScale scale, int barInterval, IBars bars, bool includePartialBar)
        {
            Bars result = new Bars(symbol, scale, barInterval);
            foreach (IBar b in bars)
                result.Add(b.dt, b.open, b.high, b.low, b.close, b.volume);
            if (!includePartialBar)
                result.Delete(result.Count - 1);
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