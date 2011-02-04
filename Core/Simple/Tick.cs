using System;
using System.Collections.Generic;

using System.Runtime.InteropServices;
using System.IO;

namespace OpenWealth.Simple
{
    /// <summary>
    /// Тик, реализующий IBar
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Tick : IBar
    {
        private readonly static ILog l = Core.GetLogger(typeof(Tick));

        public const int Size = 3 * sizeof(int) + sizeof(float);

        private readonly int number;
        private readonly float price;
        private readonly int volume;
        private readonly int dt;

        public int DT
        {
            get
            {
                return dt;
            }
        }

        public int EndDT
        {
            get
            {
                return dt;
            }
        }

        public int Number
        {
            get
            {
                return this.number;
            }
        }
        public float Open
        {
            get
            {
                return price;
            }
        }
        public float High
        {
            get
            {
                return price;
            }
        }
        public float Low
        {
            get
            {
                return price;
            }
        }
        public float Close
        {
            get
            {
                return price;
            }
        }
        public int Volume
        {
            get
            {
                return volume;
            }
        }
        public DateTime GetDateTime()
        {
            return DateTime2Int.DateTime(dt);
        }

        public Tick(IBar bar)
        {
            if (bar.High != bar.Low)
                l.Error("Преобразую в Tick бар, который тиком не является " + bar);
            this.number = bar.Number;
            this.price = bar.Close;
            this.volume = bar.Volume;
            this.dt = bar.DT;
        }
        public Tick(DateTime dt, int number, float price, int volue)
        {
            this.number = number;
            this.price = price;
            this.volume = volue;
            this.dt = DateTime2Int.Int(dt);
        }
        public Tick(int dt, int number, float price, int volue)
        {
            this.number = number;
            this.price = price;
            this.volume = volue;
            this.dt = dt;
        }

        public Tick(BinaryReader br)
        {
            this.dt = br.ReadInt32();
            this.number = br.ReadInt32();
            this.price = br.ReadSingle();
            this.volume = br.ReadInt32();
        }
        public void WriteTo(BinaryWriter bw)
        {
            bw.Write(this.dt);
            bw.Write(this.number);
            bw.Write(this.price);
            bw.Write(this.volume);
        }

        public override string ToString()
        {
            return string.Concat("Tick ", GetDateTime().ToString("yyyyMMdd hhmmss"), " ", Number, " ", price, " ", volume);
        }
    }

}
