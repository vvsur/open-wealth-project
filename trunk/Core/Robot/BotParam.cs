using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    public class BotParam
    {
        public readonly string Name;
        public readonly double MinValue;
        public readonly double MaxValue;
        public readonly double DefaultValue;
        public double Value { get; set; }

        public BotParam(string name, double minValue, double maxValue, double defaultValue)
        {
            this.Name = name;
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.DefaultValue = defaultValue;
            Value = defaultValue;
        }

        public BotParam(BotParam bp)
        {
            this.Name = bp.Name;
            this.MinValue = bp.MinValue;
            this.MaxValue = bp.MaxValue;
            this.DefaultValue = bp.DefaultValue;
            this.Value = bp.Value;
        }
    }
}
