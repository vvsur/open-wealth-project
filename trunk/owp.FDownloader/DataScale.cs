using System;
using System.Collections.Generic;

namespace owp.Cap
{
    public class DataScale
    {
        public DataScale(DataScaleEnum scale, int interval)
            : this(scale, interval, DateTime.MinValue)
        {
        }

        public DataScale(DataScaleEnum scale, int interval, DateTime from) 
        {
            this.from = from;
            this.scale = scale;
            this.interval = interval;
            if (interval < 1)
                throw new ArgumentException("interval не может быть меньше 0, а он равен " + interval);
            if ((scale == DataScaleEnum.volume) && (interval == 1))
                throw new ArgumentException("interval не может быть равен 1, для DataScaleEnum.volume");
        }

        public readonly DataScaleEnum scale;
        public readonly int interval;
        public readonly DateTime from;

        public override string ToString()
        {

            string from = String.Empty;
            if (this.from != DateTime.MinValue)
                from = "From" + this.from.ToString("yy.MM.dd hh-mm-ss");

            if ((interval == 1)&&(scale != DataScaleEnum.month))
                return scale.ToString()+from;
            if (scale == DataScaleEnum.sec)
            {
                if ((interval >= 604800) && (interval % 604800 == 0))
                    return interval / 604800 + "w" + from;

                if ((interval >= 86400) && (interval % 86400 == 0))
                    return interval / 86400 + "d" + from;

                if ((interval >= 3600) && (interval % 3600 == 0))
                    return interval / 3600 + "h" + from;

                if ((interval >= 60) && (interval % 60 == 0))
                    return interval / 60 + "min" + from;
                return interval + "sec" + from;
            }
            else
                if (scale == DataScaleEnum.month)
                    return interval.ToString() + "m" + from;
                else
                    return interval.ToString() + scale.ToString() + from;
        }
        //TODO  public static DataScale Parse(string s);
        //TODO  public static bool operator !=(DataScale a, DataScale b)
        //TODO  public static bool operator ==(DataScale a, DataScale b)
        public bool CanConvertTo(DataScale dataScale)
        {
            if (from!=dataScale.from)
                return false;
            return Equals(dataScale) || ((interval == 1) && (scale == DataScaleEnum.tick)) || ((scale == dataScale.scale) && (0==dataScale.interval % interval));
        }

        public override bool Equals(object obj)
        {
            var k = obj as DataScale;
            if (k != null)
            {
                return (this.interval == k.interval) && (this.scale == k.scale) && (this.from == k.from);
            }
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            int h = interval ^ scale.GetHashCode();
            return h.GetHashCode();
        }

    }

    public enum DataScaleEnum { tick, sec, volume, month} // TODO расширить данный enum, в соответствии с tostring
}
