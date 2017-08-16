using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace bOS.Commons.Configuration
{
    public class LinkElement : ConfigurationElement
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

        [ConfigurationProperty("url", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string Url
        {
            get
            {
                return ((string)(base["url"]));
            }
            set
            {
                base["url"] = value;
            }
        }

        [ConfigurationProperty("id", IsKey = false, IsRequired = false)]
        public long Id
        {
            get
            {
                return ((long)(base["id"]));
            }
            set
            {
                base["id"] = value;
            }
        }

        [ConfigurationProperty("user", IsKey = false, IsRequired = false)]
        public string User
        {
            get
            {
                return ((string)(base["user"]));
            }
            set
            {
                base["user"] = value;
            }
        }

        [ConfigurationProperty("password", IsKey = false, IsRequired = false)]
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

        [ConfigurationProperty("type", IsKey = false, IsRequired = false)]
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

        [ConfigurationProperty("description", IsKey = false, IsRequired = false)]
        public string Description
        {
            get
            {
                return ((string)(base["description"]));
            }
            set
            {
                base["description"] = value;
            }
        }
    }
}