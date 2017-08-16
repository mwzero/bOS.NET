using log4net;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;

namespace bOS.Commons.Lucene
{
    public interface ILuceneClient
    {
        void Open(String path);
        void Close();
        void Add(Document doc, Term idTerm);
        bool Exist(Term searchTerm);
        void Delete(Term idTerm);
        void Update(Document docToUpdate, Term idTerm);
        List<Int32> Search(BooleanQuery innerExpr, ILuceneDocumentComparator comparator);
    }
}