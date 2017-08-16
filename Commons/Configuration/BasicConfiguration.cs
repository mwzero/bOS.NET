using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace bOS.Commons.Configuration
{
    public class BasicConfiguration : ConfigurationSection
    {
        [ConfigurationProperty("Folders")]
        public FoldersCollection FolderItems
        {
            get { return ((FoldersCollection)(base["Folders"])); }
        }
        
        [ConfigurationProperty("Emails")]
        public EmailsCollection EmailsItems
        {
            get { return ((EmailsCollection)(base["Emails"])); }
        }

        [ConfigurationProperty("Links")]
        public LinksCollection LinksItems
        {
            get { return ((LinksCollection)(base["Links"])); }
        }

        [ConfigurationProperty("Flags")]
        public FlagsCollection FlagsItems
        {
            get { return ((FlagsCollection)(base["Flags"])); }
        }

    }

    [ConfigurationCollection(typeof(EmailElement))]
    public class EmailsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new EmailElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EmailElement)(element)).Name;
        }

        public EmailElement this[int idx]
        {
            get
            {
                return (EmailElement)BaseGet(idx);
            }
        }

        new public EmailElement this[String key]
        {
            get
            {
                return (EmailElement)BaseGet(key);
            }
        }
    }

    

    [ConfigurationCollection(typeof(FolderElement))]
    public class FoldersCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FolderElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FolderElement)(element)).Name;
        }

        public FolderElement this[int idx]
        {
            get
            {
                return (FolderElement)BaseGet(idx);
            }
        }

        new public FolderElement this[String key]
        {
            get
            {
                return (FolderElement)BaseGet(key);
            }
        }
    }

    [ConfigurationCollection(typeof(LinkElement))]
    public class LinksCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new LinkElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((LinkElement)(element)).Name;
        }

        public LinkElement this[int idx]
        {
            get
            {
                return (LinkElement)BaseGet(idx);
            }
        }

        new public LinkElement this[String key]
        {
            get
            {
                return (LinkElement)BaseGet(key);
            }
        }
    }

    [ConfigurationCollection(typeof(FlagElement))]
    public class FlagsCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FlagElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FlagElement)(element)).Name;
        }

        public FlagElement this[int idx]
        {
            get
            {
                return (FlagElement)BaseGet(idx);
            }
        }

        new public FlagElement this[String key]
        {
            get
            {
                return (FlagElement)BaseGet(key);
            }
        }
    }

}
