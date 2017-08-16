using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace bOS.Commons.Configuration
{
    public class FlagElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string Name
        {
            get
            {
                return ((string)(base["name"]));
            }
            set
            {
                base["name"] = value;
            }
        }

        [ConfigurationProperty("value", DefaultValue = "", IsKey = false, IsRequired = true)]
        public String Value
        {
            get
            {
                return ((String)(base["value"]));
            }
            set
            {
                base["value"] = value;
            }
        }

    }
}