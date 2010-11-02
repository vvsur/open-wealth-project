using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;

namespace OpenWealth.Simple
{
    /// <summary>
    /// Dictionary - который можно сериализовывать. Потребывался для SettingsHost
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public sealed class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
        where TKey : IConvertible
        where TValue : IConvertible
    {

        static ILog l = Core.GetLogger(typeof(SerializableDictionary<TKey, TValue>).FullName);

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }


        public void ReadXml(System.Xml.XmlReader reader)
        {
            if (reader.IsEmptyElement)
            {
                reader.Read();
                return;
            }

            reader.ReadStartElement("SerializableDictionaryOfStringString");
            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                /*
                reader.Read();
                string key = reader.ReadElementContentAsString();
                string value = reader.ReadElementContentAsString();
                reader.ReadEndElement();
                base.Add((TKey)Convert.ChangeType(key, typeof(TKey), CultureInfo.InvariantCulture), (TValue)Convert.ChangeType(value, typeof(TValue), CultureInfo.InvariantCulture));*/

                if (reader.HasAttributes)
                {
                    string key = reader.GetAttribute("Key");
                    string value = reader.GetAttribute("Value");

                    l.Debug("key=" + key + " value=" + value);

                    base.Add((TKey)Convert.ChangeType(key, typeof(TKey), CultureInfo.InvariantCulture), (TValue)Convert.ChangeType(value, typeof(TValue), CultureInfo.InvariantCulture));
                    reader.MoveToElement();
                }
                reader.ReadStartElement("KeyValuePair");
                //reader.ReadEndElement();
                //reader.MoveToContent();
            }
            reader.ReadEndElement();
        }


        public void WriteXml(System.Xml.XmlWriter writer)
        {
            foreach (TKey key in base.Keys)
            {
                /* writer.WriteStartElement("KeyValuePair");
                 writer.WriteStartElement("Key");
                 writer.WriteString(key.ToString(CultureInfo.InvariantCulture));
                 writer.WriteEndElement();
                 writer.WriteStartElement("Value");
                 writer.WriteString(base[key].ToString(CultureInfo.InvariantCulture));
                 writer.WriteEndElement();
                 writer.WriteEndElement();*/

                writer.WriteStartElement("KeyValuePair");
                writer.WriteAttributeString("Key", key.ToString(CultureInfo.InvariantCulture));
                writer.WriteAttributeString("Value", base[key].ToString(CultureInfo.InvariantCulture));
                writer.WriteEndElement();
            }
        }

    }
}