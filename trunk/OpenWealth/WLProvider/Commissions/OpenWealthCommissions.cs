using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

using WealthLab;
using OpenWealth;

namespace OpenWealth.WLProvider
{
    public class Commissions : Commission, Fidelity.Components.ICustomSettings
    {
        static ILog l = Core.GetLogger(typeof(Commissions).FullName);

        double B = 0;
        double F = 0;
        double M = 0;

        public override double Calculate(TradeType tradeType, OrderType orderType, double orderPrice, double shares, Bars bars)
        {
            return B + F * shares + (M / 100) * (shares * orderPrice);
        }
        
        public override string Description
        {
            get
            {
                return B + " рублей за сделку " + F + " рублей за контракт в одну сторону + " + M + " % от суммы сделки.";
            }
        }

        public override string FriendlyName
        {
            get
            {
                return "OpenWealth";
            }
        }


        public void ChangeSettings(UserControl ui)
        {
            CommissionUserControl settings = ui as CommissionUserControl;
            this.B = settings.B;
            this.F = settings.F;
            this.M = settings.M;
        }

        public UserControl GetSettingsUI()
        {
            CommissionUserControl settings = new CommissionUserControl();
            settings.B = this.B;
            settings.F = this.F;
            settings.M = this.M;
            return settings;
        }

        public void ReadSettings(Fidelity.Components.ISettingsHost notUsing)
        {
            ISettingsHost host = Core.GetGlobal("SettingsHost") as ISettingsHost;
            if (host != null)
            {
                this.B = host.Get("Commissions.B", (double)0);
                this.F = host.Get("Commissions.F", (double)0);
                this.M = host.Get("Commissions.M", (double)0);
            }
            else
                l.Error("Ошибка загрузки SettingsHost");
                
        }

        public void WriteSettings(Fidelity.Components.ISettingsHost notUsing)
        {
            ISettingsHost host = Core.GetGlobal("SettingsHost") as ISettingsHost;
            if (host != null)
            {
                host.Set("Commissions.B", this.B);
                host.Set("Commissions.F", this.F);
                host.Set("Commissions.M", this.M);
            }
            else
                l.Error("Ошибка загрузки SettingsHost");
        }
    }
}