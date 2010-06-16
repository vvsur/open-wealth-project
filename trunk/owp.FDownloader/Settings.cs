// http://stackoverflow.com/questions/46282/how-do-i-create-an-xmlnode-from-a-call-to-xmlserializer-serialize
// http://openlandscape.wordpress.com/2007/10/13/dot-net-custom-name-value-config-sections/

using System;
using System.Collections.Generic;
using System.Text;

namespace owp.FDownloader
{
    public class Settings 
    {
        public bool autoFlag { get; set; } // берется не из файла, а из ключей запуска программы !!!!

        public bool useProxy { get; set; }
        public String proxy { get; set; }
        public bool proxyWithPassword { get; set; }
        public String proxyUser { get; set; }
        public String proxyPassword { get; set; }

        public bool loadFromFinam { get; set; }
        public bool fromYesterday { get; set; }    //checkBoxYesterday 
        public bool fromCSV { get; set; } //checkBoxFromCSV
        public bool toToday { get; set; }//checkBoxToday
        public DateTime from { get; set; }//dateTimePickerFrom
        public DateTime to { get; set; }//dateTimePickerTo
        public int period { get; set; }//comboBoxPeriod +1

        public string csvDir { get; set; } //textBoxCSVDir
        public bool margeCsv { get; set; } //checkBoxMargeCSV
        public bool convertCSV2WL { get; set; }//checkBoxConvertCSV2WL
        public bool delCSV { get; set; }//checkBoxDelCSV
        public string wlDir { get; set; }//textBoxWLDir

        public List<EmitentInfo> Emitents { get; set; }
        public Settings()
        {
            useProxy = false;
            proxy = String.Empty;
            proxyWithPassword = false;
            proxyUser = String.Empty;
            proxyPassword = String.Empty;
            Emitents = new List<EmitentInfo>();
        }
        public static Settings Load(String FileName)
        {
            Settings settings;
            if (System.IO.File.Exists(FileName))
            {
                System.Xml.Serialization.XmlSerializer nser = new System.Xml.Serialization.XmlSerializer(typeof(Settings));
                System.IO.TextReader reader = new System.IO.StreamReader(FileName);
                settings = (Settings)nser.Deserialize(reader);
                reader.Close();
            }
            else
            {
                settings = new Settings();
            }
            settings.autoFlag = false;
            return settings;
        }
        public void Save(String FileName)
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(Settings));
            System.IO.TextWriter writer = new System.IO.StreamWriter(FileName);
            ser.Serialize(writer, this);
            writer.Close();
        }
    }
}
