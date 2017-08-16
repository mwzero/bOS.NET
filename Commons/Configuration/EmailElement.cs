using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace bOS.Commons.Configuration
{
    public class EmailElement : ConfigurationElement
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

        [ConfigurationProperty("type", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Type
        {
            get
            {
                return ((string)(base["type"]));
            }
            set
            {
                base["type"] = value;
            }
        }

        [ConfigurationProperty("server", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Server
        {
            get
            {
                return ((string)(base["server"]));
            }
            set
            {
                base["server"] = value;
            }
        }

        [ConfigurationProperty("port", DefaultValue = "25", IsKey = false, IsRequired = true)]
        public int Port
        {
            get
            {
                return ((int)(base["port"]));
            }
            set
            {
                base["port"] = value;
            }
        }

        [ConfigurationProperty("username", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Username
        {
            get
            {
                return ((string)(base["username"]));
            }
            set
            {
                base["username"] = value;
            }
        }

        [ConfigurationProperty("password", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Password
        {
            get
            {
                return ((string)(base["password"]));
            }
            set
            {
                base["password"] = value;
            }
        }

        [ConfigurationProperty("nrmail", DefaultValue = "5", IsKey = false, IsRequired = true)]
        public int NrMail
        {
            get
            {
                return ((int)(base["nrmail"]));
            }
            set
            {
                base["nrmail"] = value;
            }
        }

    }
}