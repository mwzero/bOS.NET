using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace bOS.Commons.Configuration
{
    public class FolderElement : ConfigurationElement
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

        [ConfigurationProperty("folderType", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string FolderType
        {
            get
            {
                return ((string)(base["folderType"]));
            }
            set
            {
                base["folderType"] = value;
            }
        }

        [ConfigurationProperty("path", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string Path
        {
            get
            {
                return ((string)(base["path"]));
            }
            set
            {
                base["path"] = value;
            }
        }

    }
}