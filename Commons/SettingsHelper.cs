using System;
using System.Xml;
using System.Xml.Serialization;

namespace bOS.Commons
{
    public class SettingsHelper
    {
        private static volatile SettingsHelper instance;
        private static object syncRoot = new Object();
        public static String Path = String.Empty;

        bOS.Commons.XSD.Settings settings;

        private SettingsHelper() 
        {
            using (System.IO.StreamReader str = new System.IO.StreamReader(Path))
            {
                System.Xml.Serialization.XmlSerializer xSerializer = new System.Xml.Serialization.XmlSerializer(typeof(bOS.Commons.XSD.Settings));
                settings = (bOS.Commons.XSD.Settings)xSerializer.Deserialize(str);
            }
        }

        public static SettingsHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new SettingsHelper();
                    }
                }

                return instance;
            }
        }

        public static bOS.Commons.XSD.Entity buildEntity(String xml)
        {
            bOS.Commons.XSD.Entity entity = null;

            if (String.IsNullOrWhiteSpace(xml))
            {
                entity = new bOS.Commons.XSD.Entity();
            }
            else
            {

                using (System.IO.StringReader str = new System.IO.StringReader(xml))
                {
                    System.Xml.Serialization.XmlSerializer xSerializer = new System.Xml.Serialization.XmlSerializer(typeof(bOS.Commons.XSD.Entity));
                    entity = (bOS.Commons.XSD.Entity)xSerializer.Deserialize(str);
                    str.Close();
                }
            }

            return entity;
        }

        public static String GetXml(bOS.Commons.XSD.Entity entity)
        {
            String xml = String.Empty;

            System.Xml.Serialization.XmlSerializer xsSubmit = new System.Xml.Serialization.XmlSerializer(typeof(bOS.Commons.XSD.Entity));
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.OmitXmlDeclaration = true;

            using (System.IO.StringWriter str = new System.IO.StringWriter())
            {
                XmlWriter writer = XmlWriter.Create(str, settings);
                xsSubmit.Serialize(writer, entity, ns);
                xml = str.ToString(); // Your xml
            }

            return xml;
        }
    
    }
}