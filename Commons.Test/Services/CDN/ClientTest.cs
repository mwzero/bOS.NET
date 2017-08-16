using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using bOS.Services.CDN.Client;
using System.Collections.Specialized;

namespace bOS.Services.CDN
{
    [TestClass]
    public class ClientTest
    {
        [TestMethod]
        public void PostFile()
        {
            CdnWebHelper cdnClient = new CdnWebHelper("http://localhost:50241/api");

            NameValueCollection nv = new NameValueCollection();
            nv.Add ("paziente","5");
            nv.Add ("visita","29214");
            nv.Add ("folderType","0");

            cdnClient.SaveFileContent("ArchivexFile", "CDN.pdf", "C:\\Dati\\mySoftware\\bosnet\\Commons.Test\\files\\CDN.pdf", nv);
            Assert.AreEqual("ecco", "ecco", true);
        }
    }
}
