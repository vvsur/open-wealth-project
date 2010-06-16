using System;
using System.Collections.Generic;
using System.Text;

namespace owp.FDownloader
{
    public static class FinamDataScale
    {
        //public readonly owp.Cap.DataScale dataScale;

       // FinamDataScale(int i)
      //  {
     //       dataScale = new owp.Cap.DataScale(Finam2scale(i), Finam2interval(i), Finam2from(i));
      //  }
        static owp.Cap.DataScaleEnum Finam2scale(int i)
        {
            if ((i < 1) || (i > 11))
                throw new IndexOutOfRangeException("FinamDataScale поддерживает только периоды от 1 до 11");
            switch (i)
            {
                case 1: return owp.Cap.DataScaleEnum.tick;
                case 10: return owp.Cap.DataScaleEnum.month;
                default: return owp.Cap.DataScaleEnum.sec;
            }
        }
        static int Finam2interval(int i)
        {
            if ((i < 1) || (i > 11))
                throw new IndexOutOfRangeException("FinamDataScale поддерживает только периоды от 1 до 11");
            switch (i)
            {
                case 1: return 1;
                case 10: return 1;

                case 2: return 60;
                case 3: return 60 * 5;
                case 4: return 60 * 10;
                case 5: return 60 * 15;
                case 6: return 60 * 30;
                case 11:
                case 7: return 60 * 60;
                case 8: return 60 * 60 * 24;
                case 9: return 60 * 60 * 24 * 7;

                default: return 0;
            }
        }
        static DateTime Finam2from(int i)
        {
            if (i == 11)
                return new DateTime(1, 1, 1, 10, 30, 0);
            return DateTime.MinValue;
        }

        public static string ToString(int i)
        {
            owp.Cap.DataScale dataScale = new owp.Cap.DataScale(Finam2scale(i), Finam2interval(i), Finam2from(i));
            return dataScale.ToString();
        }
    }
}
