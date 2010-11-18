using System;
using System.Globalization;

namespace OpenWealth.QuikDataProvider.DDE
    {
    /// <summary>
    /// Специальный класс для прыжкам по FastTableFormat
    /// Разобраться может даже ребенок
    /// автоматический кастинг
    /// предоставление текущего типа
    /// возможность пихать в качестве итератора
    /// </summary>
    public struct XlTableDataBlock : System.Collections.IEnumerable, System.Collections.IEnumerator
        {
        public short TableDataType;
        public short DataSize;
        public byte[] data;
        private int readerIndex;
        private object CurrentObject;
        public static readonly System.Text.Encoding enc = System.Text.Encoding.GetEncoding(1251);
        public static readonly CultureInfo cultInfo = new CultureInfo("ru-Ru");
        public Type currentType;
        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator()
        {
            return (System.Collections.IEnumerator) this;
        }

        #endregion

        #region IEnumerator Members

        public object Current
        {
            get { return this.CurrentObject; }
        }

        public bool MoveNext()
        {
            if (this.DataSize <= readerIndex) return false;
            
                switch (TableDataType)
                {
                    // если флоат
                    case 0x0001:
                        this.CurrentObject =
                            BitConverter.ToDouble(
                                new byte[]
                                    {
                                        data[readerIndex], data[readerIndex + 1], data[readerIndex + 2],
                                        data[readerIndex + 3], data[readerIndex + 4], data[readerIndex + 5],
                                        data[readerIndex + 6], data[readerIndex + 7]
                                    }, 0);
                        readerIndex += 8;
                        this.currentType = typeof (double);
                        break;
                    // если строка
                    case 0x0002:
                        //Сперва надо узнать её размер
                        byte strLen = data[readerIndex];
                        readerIndex += 1;
                        //А потом нужно считать данные
                        this.CurrentObject = enc.GetString(data, readerIndex, strLen);
                        readerIndex += strLen;
                        this.currentType = typeof (string);
                        break;
                    case 0x0003:
                        this.CurrentObject =
                            BitConverter.ToBoolean(new byte[] {data[readerIndex]}, 0);
                        //Значимый только 1 байт а размер в 2 байта
                        readerIndex += 2;
                        this.currentType = typeof (bool);
                        break;
                    case 0x0004:
                        // с ерором тут хрен знает че делать
                        this.CurrentObject = data[readerIndex];
                        readerIndex += 2;
                        this.currentType = null;
                        break;
                    case 0x0005:
                        //С пустыми ячейками почти так же как и со строками
                        this.CurrentObject = null;
                        readerIndex += BitConverter.ToInt16(new byte[] {data[readerIndex], data[readerIndex]}, 0) + 2;
                        this.currentType = null;
                        break;
                    case 0x0006:
                        this.CurrentObject = BitConverter.ToInt16(new byte[]{data[readerIndex], data[readerIndex]}, 0);
                        readerIndex += 2;
                        this.currentType = typeof (short);
                        break;
                    case 0x0007:
                        // на самом деле тут все сложнее. тоесть прийдет число, тех ячеек,которые можно пропустить, так как они не изменили своего значения
                        readerIndex += 2;
                        this.currentType = null;
                        break;
                    default:
                        throw new FormatException("unknown Data_type");
                }
                return true;
        }

        public void Reset()
        {
            this.readerIndex = 0;
        }

        #endregion
        }
    }
