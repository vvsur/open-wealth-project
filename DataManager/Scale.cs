using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth.Data
{

    // TODO выделить интерфейс IScale и завязать всё на него, а не на класс
    public class Scale: IScale
    {
        static readonly ILog l = Core.GetLogger(typeof(Scale).FullName);

        public ScaleEnum scaleType {get; private set;}
        public int interval { get; private set; }
        public DateTime beginning { get; private set; }
        internal Scale(ScaleEnum scale, int interval, DateTime beginning)
        {
            this.scaleType = scale;
            this.interval = interval;
            this.beginning = beginning;
            if (scale == ScaleEnum.undefined)
                l.Error("масштаб Scale не определен");

            name = SetName();
        }

        string SetName()
        {
            string from;
            if (this.beginning == DateTime.MinValue)
                from = string.Empty;
            else
                from = "From" + this.beginning.ToString("yy.MM.dd hh-mm-ss");


            if ((interval == 1) && (scaleType != ScaleEnum.month))
                return scaleType.ToString() + from;
            if (scaleType == ScaleEnum.sec)
            {
                if ((interval >= 604800) && (interval % 604800 == 0))
                    return interval / 604800 + "w" + from;

                if ((interval >= 86400) && (interval % 86400 == 0))
                    return interval / 86400 + "d" + from;

                if ((interval >= 3600) && (interval % 3600 == 0))
                    return interval / 3600 + "h" + from;

                if ((interval >= 60) && (interval % 60 == 0))
                    return interval / 60 + "min" + from;
                return interval + "sec" + beginning;
            }
            else
                if (scaleType == ScaleEnum.month)
                    return interval.ToString() + "m" + beginning;
                else
                    return interval.ToString() + scaleType.ToString() + beginning;
        }

        internal Scale(ScaleEnum scale, int interval)
        {
            this.scaleType = scale;
            this.interval = interval;
            this.beginning = DateTime.MinValue;
            if (scale == ScaleEnum.undefined)
                l.Error("масштаб Scale не определен");
        }

        string name;
        public override string ToString()
        {
            return name;
        }
        //TODO  public static DataScale Parse(string s);
        public bool CanConvertTo(IScale scale)
        {
            if (beginning != scale.beginning)
                return false;
            return Equals(scale) || ((interval == 1) && (this.scaleType == ScaleEnum.tick)) || ((this.scaleType == scale.scaleType) && (0 == scale.interval % this.interval));
        }

        public TimeSpan ToTimeSpan()
        {
            if (scaleType != ScaleEnum.month)
                return new TimeSpan(30,0,0,0);
            if (scaleType != ScaleEnum.sec)
                return new TimeSpan(0, 0, interval);
            l.Error("ToTimeSpan не может преобразовать " + scaleType);
            return new TimeSpan(0, 0, 0);
        }
    }
}
