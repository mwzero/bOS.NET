using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace bOS.Commons.Xml
{
    public class XmlHelper
    {
        public static XmlAttribute CreateAttribute(XmlDocument xmlDoc, String name, String value)
        {
            XmlAttribute attr = xmlDoc.CreateAttribute(name);
            attr.Value = value;
            return attr;
        }

        public XmlNode SelectOneNode(XmlNodeList nodes, string name)
        {
            foreach (XmlNode n in nodes)
            {
                if (n.Name == name)
                    return n;
            }

            return null;
        }

        public static XmlNode SelectOneNode(XmlNodeList nodes, string name, string value)
        {
            foreach (XmlNode n in nodes)
            {
                if (n.Attributes[name].Value == value)
                    return n;
            }

            return null;
        }

       
    }
}
