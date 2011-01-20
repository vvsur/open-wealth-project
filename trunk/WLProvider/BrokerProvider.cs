#define WL5 //ставить WL5 или WL6

using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;
using WealthLab;

namespace OpenWealth.WLProvider
{
#if WL6 // данный атрибут был введен в версии 6
    [ProductionBrokerProvider]
#endif
    public class BrokerProvider : WealthLab.BrokerProvider, Fidelity.Components.ICustomSettings
    {
        static readonly ILog l = Core.GetLogger(typeof(BrokerProvider).FullName);

        public BrokerProvider()
            : base()
        {
            l.Debug("BrokerProvider()");
        }

#if WL6  // данный метод был введен в версии 6
        public override List<string> AccountTradeTypesAllowed(string acctNumber, string action)
        {
            return new List<string>();
        }
#endif
        public override void AccountPositionsMofidied(Account account)
        {
            l.Debug("AccountPositionsMofidied " + account.AccountNumber);
        }
        public override bool AllowCancelReplace(Order order, string bracketConditional)
        {
            l.Debug("AllowCancelReplace " + order.OrderID + " " + bracketConditional);
            return false;
        }

        public override bool AllowDecimalsForExtendedOrderType(string orderType)
        {
            l.Debug("AllowDecimalsForExtendedOrderType " + orderType);
            return true;
        }
        public override bool AllowOrderTypeForRoute(string route, OrderType orderType)
        {
            l.Debug("AllowOrderTypeForRoute " + route + " " + orderType.ToString());
            return true;
        }
        public override void CancelOrder(IList<Order> orders)
        {
            l.Debug("CancelOrder Count=" + orders.Count);
        }
        public override void CancelOrder(Order order)
        {
            l.Debug("CancelOrder " + order.OrderID);
        }
        public override void CancelReplace(Order order, Order newOrder)
        {
            l.Debug("CancelReplace " + order.OrderID + " " + newOrder.OrderID);
        }
        public override IList<string> CancelReplaceOrderTypesAllowed(Order order)
        {
            l.Debug("CancelReplaceOrderTypesAllowed " + order.OrderID);
            return new List<string>();
        }
        public override DateTime ConvertToMarketTimeZone(string symbol, DateTime dt)
        {
            l.Debug("ConvertToMarketTimeZone " + dt);
            return dt;
        }
        public override List<string> ExtendedOrderTypesAllowed(string acctNum, string route, string action)
        {
            l.Debug("ExtendedOrderTypesAllowed "+acctNum+" "+route+" "+action);
            return new List<string>();
        }
        protected override List<Account> GetAccounts()
        {
            l.Debug("GetAccounts");

            Account a = new Account();
            a.AccountNumber = "111111111";
            a.AccountValue = 1;
            a.AccountValueTimeStamp = DateTime.Now;
            a.AvailableCash = 1;
            a.BuyingPower = 1;
            a.IsPaperAccount = false;

            List<Account> accounts = new List<Account>();
            accounts.Add(a);
            return accounts;
        }
        public override Quote GetQuote(string symbol)
        {
            l.Debug("GetQuote " + symbol);

            Quote q = new Quote();
            q.Symbol = symbol;
            return q;
        }
        public override void GetStateImages(ImageList img)
        {
            if (img==null)
                l.Debug("GetStateImages null");
            else
                l.Debug("GetStateImages Count" + img.Images.Count);
        }

        IBrokerHost brokerHost;
        public override void Initialize(IBrokerHost brokerHost, AuthenticationProvider authProvider)
        {
            l.Debug("Initialize " + brokerHost.GetType().FullName + " " + authProvider.GetType().FullName);
            base.Initialize(brokerHost, authProvider);
            this.brokerHost = brokerHost;
        }
        public override void PlaceOrder(IList<Order> orders)
        {
            l.Debug("PlaceOrder "+orders.Count);
        }
        public override void PlaceOrder(Order order)
        {
            l.Debug("PlaceOrder "+order.OrderID);
        }     
        public override void RequestOrderStatusUpdates(Account acct)
        {
            if (acct == null)
                l.Debug("RequestOrderStatusUpdates null");
            else
                l.Debug("RequestOrderStatusUpdates "+acct.AccountNumber);
        }
        public override void RequestOrderStatusUpdatesForOrders(IList<Order> orders)
        {
            l.Debug("RequestOrderStatusUpdatesForOrders " + orders.Count);
        }
        public override string RouteForStrategyOrders(BarDataScale scale)
        {
            l.Debug("RouteForStrategyOrders " + scale.ToString());
            return "RouteForStrategyOrders";
        }
        public override void UpdateAccounts()
        {
            l.Debug("UpdateAccounts");

            Account a = new Account();
            a.AccountNumber = "111111111";
            a.AccountValue = 22222222;
            a.AccountValueTimeStamp = DateTime.Now;
            a.AvailableCash = 3333333;
            a.BuyingPower = 44444;
            a.IsPaperAccount = false;
            a.Positions = new List<AccountPosition>();

            brokerHost.AccountBalanceUpdate(a);
        }



        public void ChangeSettings(UserControl ui)
        {
            l.Debug("ChangeSettings");
        }
        public UserControl GetSettingsUI()
        {
            l.Debug("GetSettingsUI");
            return new UserControl();
        }
        public void ReadSettings(Fidelity.Components.ISettingsHost notUsing)
        {
            l.Debug("ReadSettings");
        }
        public void WriteSettings(Fidelity.Components.ISettingsHost notUsing)
        {
            l.Debug("WriteSettings");
        }

        #region Tifs
        public override string TifForStrategyOrders(BarDataScale scale)
        {
            l.Debug("TifForStrategyOrders " + scale.ToString());
            return "Day";
        }
        public override List<string> TifsAllowed(string acctNumber, string route, string orderType)
        {
            l.Debug("TifsAllowed " + acctNumber + " " + route + " " + orderType);
            List<string> ls = new List<string>();
            ls.Add("Day");
            return ls;
        }
        #endregion Tifs

        #region Routes
        public override IList<string> RoutesForAccountNumber(string acctNumber)
        {
            l.Debug("RoutesForAccountNumber " + acctNumber);
            List<string> ls = new List<string>();
            ls.Add("OpenWealth");
            return ls;
        }
        public override IList<string> Routes
        {
            get
            {
                l.Debug("Routes");
                IList<string> ls = new List<string>();
                ls.Add("OpenWealth");
                return ls;
            }
        }
        #endregion Routes

    }
}