using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bOS.Services.CDN.Client
{
    public abstract class CdnPersistenceHelper
    {
        public abstract String GetContent(String path, String contentName, NameValueCollection nvc);
        public abstract void SaveContent(String path, String contentName, String content, NameValueCollection nvc);
        public abstract void SaveFileContent(String path, String contentName, String fileContent, NameValueCollection nvc);
    }
}
