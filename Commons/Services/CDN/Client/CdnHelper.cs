using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bOS.Services.CDN.Client
{
    public sealed class CdnHelper
    {
        private static volatile CdnHelper instance;
        private static object syncRoot = new Object();

        private CdnPersistenceHelper helper;

        private CdnHelper() {
            helper = null;
        }

        public void Initialize(CdnPersistenceHelper persistenceHelper)
        {
            helper = persistenceHelper;
        }

        public static CdnHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new CdnHelper();
                    }
                }

                return instance;
            }
        }

        public String GetContent(String path, String contentName, NameValueCollection nvc)
        {
            return helper.GetContent(path, contentName, nvc);
        }

        public void SaveContent(String path, String contentName, String content, NameValueCollection nvc)
        {
            helper.SaveContent(path, contentName, content, nvc);
        }

        public void SaveFileContent(String path, String contentName, String fileContent, NameValueCollection nvc)
        {
            helper.SaveFileContent(path, contentName, fileContent, nvc);
        }

    }
 
}
