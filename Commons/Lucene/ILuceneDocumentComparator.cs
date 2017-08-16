using Lucene.Net.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace bOS.Commons.Lucene
{
    public interface ILuceneDocumentComparator
    {
        Boolean Check(Document doc);
    }
}