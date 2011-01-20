using System;
using System.Globalization;

namespace OpenWealth.QuikDataProvider
{

    public partial class XLTableTyped : System.Collections.IEnumerable, System.Collections.IEnumerator
    {

        private byte[] rawData;
        private int readerIndex;

        private object CurrentObject;
        private XlTableDataBlock currentBlock;
        public Type currentType; 

        public XLTableTyped(byte[] rawData)
        {
            this.rawData = rawData;
            readerIndex = 8;
            this.currentBlock = ReadDataBlock();
        }

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            return (System.Collections.IEnumerator)this;
        }

        #endregion

        #region IEnumerator Members

        public object Current
        {
            get
            {
                return this.CurrentObject;
            }
        }

        public bool MoveNext()
        {
            if (readerIndex == 8 && readerIndex == rawData.Length) return false;
            if (currentBlock.MoveNext())
            {
                this.CurrentObject = currentBlock.Current;
                this.currentType = currentBlock.currentType;
                return true;
            }
            this.currentBlock = ReadDataBlock();
            return this.MoveNext();
        }

        public void Reset()
        {
            readerIndex = 8;
        }

        #endregion

        private XlTableDataBlock ReadDataBlock()
        {
            XlTableDataBlock rValue = new XlTableDataBlock();
            rValue.TableDataType =
                System.BitConverter.ToInt16(new byte[] { rawData[readerIndex], rawData[readerIndex + 1] }, 0);
            readerIndex += 2;
            rValue.DataSize =
                System.BitConverter.ToInt16(new byte[] { rawData[readerIndex], rawData[readerIndex + 1] }, 0);
            readerIndex += 2;
            rValue.data = new byte[rValue.DataSize];
            Array.Copy(rawData, readerIndex, rValue.data, 0, rValue.DataSize);
            readerIndex += rValue.DataSize;
            return rValue;
        }

    }

    public static class XLTableWraper
    {
        private static readonly Type stringType = typeof(string);

        private static readonly ILog l = Core.GetLogger(typeof(XLTableWraper).FullName);
        private static readonly IDataManager data = Core.GetGlobal("Data") as IDataManager;
        private static readonly IScale tickScale;

        static XLTableWraper()
        {
            l.Debug("static XLTableWraper()");
            if (data != null)
            {
                tickScale = data.GetScale(ScaleEnum.tick, 1);
            }
            else
                l.Error("data == null");
        }

        /// <summary>
        /// Вселенское зло спит тут!
        /// </summary>
        /// <remarks>
        /// Обязательно!!!!! Необходимо соблюсти порядок столбцов в квике!
        /// Номер,Дата, Время, Класс Код, Код инстурмента, цена, кол-во!
        /// </remarks>
        /// <returns></returns>
        internal static void GetDeals(byte[] rawData)
        {
            if (data == null)
                return;

            int rowsNumber = BitConverter.ToInt16(new byte[] { rawData[4], rawData[5] }, 0);

            XLTableTyped table = new XLTableTyped(rawData);
            for (int i = 0; i < rowsNumber; i++)
            {
                double number = -1;
                try
                {
                    table.MoveNext();
                    number = (double)table.Current;
                    table.MoveNext();
                    string DateStr = (string)table.Current;
                    table.MoveNext();
                    string TimeStr = (string)table.Current;
                    table.MoveNext();
                    string MarketStr = (string)table.Current;
                    table.MoveNext();
                    string CodeStr = (string)table.Current;
                    table.MoveNext();
                    double price = (double)table.Current;
                    table.MoveNext();
                    double volume = (double)table.Current;

                    IBar tick = new OpenWealth.Simple.Tick(ParseDateTimeWithCache(String.Concat(DateStr, " ", TimeStr)), (int)number, (float)price, (int)volume);

                    l.Debug("GetDeals Tick " + MarketStr + "." + CodeStr + " " + tick.ToString());
                    // TODO избавится от null
                    data.GetBars(data.GetSymbol(MarketStr, CodeStr), tickScale).Add(null, tick);
                }
                catch (Exception e)
                {
                    l.Error("Exception в GetDeals, при обработке сделки номер " + number, e);
                }
            }
        }



        // Данные переменные необходимы для работы ParseDateTimeWithCache
        static string dateTimeCacheString = string.Empty;
        static DateTime dateTimeCacheDateTime = new DateTime();
//        static readonly string[] formats = { "dd.MM.yyyy HH:mm:ss", "MM/dd/yyyy hh:mm:sstt", "MM/dd/yyyy HH:mm:ss", "G" };
        static readonly string[] formats = { "G" };
        static readonly CultureInfo cultInfo = CultureInfo.CurrentCulture;
       /// <summary>
        /// Если данная строка толькочто преобразовывалась в DateTime то будет использовано предыдущий результат
        /// Потребовалось ввести при профилировании GetDeals
        /// </summary>
        static DateTime ParseDateTimeWithCache(string s)
        {
            lock (dateTimeCacheString)
            {
                if (dateTimeCacheString == s)
                    return dateTimeCacheDateTime;
                try
                {                    
                    dateTimeCacheDateTime = DateTime.ParseExact(s, formats, cultInfo, DateTimeStyles.None);
                }
                catch (Exception e)
                {
                    l.Error("Не смог преобразовать строку к дате " + s, e);
                }
                dateTimeCacheString = s;
                return dateTimeCacheDateTime;
            }
        }
    }
}