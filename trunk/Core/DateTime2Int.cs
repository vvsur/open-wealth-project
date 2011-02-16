using System;
using System.Collections.Generic;
using System.Text;

namespace OpenWealth
{
    /// <summary>
    /// В программе используется время в виде int, это позволяет сократить потребляемою память, и увеличить быстродействие. 
    /// Для преобразования времени DateTime в int используется класс DateTime2Int.
    // Решение не изящное, предложите лучше!
    ///
    /// Думаю что данный int похож на unix формат времени
    /// </summary>
    public static class DateTime2Int
    {
        private readonly static DateTime dt1970 = new DateTime(1970, 1, 1);
        /// <summary>
        /// Преобразовать int в DateTime
        /// </summary>
        public static DateTime DateTime(int i)
        {
            return dt1970 + new TimeSpan(TimeSpan.TicksPerSecond * i);
        }
        /// <summary>
        /// Преобразовать DateTime в int
        /// </summary>
        public static int Int(DateTime dt)
        {
            double result = ((TimeSpan)(dt - dt1970)).TotalSeconds;
            if (result <= 0)
                return 0;
            if (result >= int.MaxValue)
                return int.MaxValue;
            return (int)((TimeSpan)(dt - dt1970)).TotalSeconds;
        }

    }
}
