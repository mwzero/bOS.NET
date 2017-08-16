using log4net;
using Lucene.Net.Analysis;
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Search;
using Lucene.Net.Store;
using LuceneNetUtil = Lucene.Net.Util;
using System;
using System.Collections.Generic;

namespace bOS.Commons.Lucene
{
    public class LuceneEmbeddedHelper : ILuceneClient 
    {
        protected static readonly ILog logger = LogManager.GetLogger(typeof(LuceneEmbeddedHelper));

        private static volatile LuceneEmbeddedHelper instance;
        private static object syncRoot = new Object();

        private System.Object lockThis = new System.Object();

        private Analyzer analyzer = new StandardAnalyzer(LuceneNetUtil.Version.LUCENE_30);
        private Directory luceneIndexDirectory;
        private IndexWriter writer;

        public static LuceneEmbeddedHelper Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new LuceneEmbeddedHelper();
                    }
                }

                return instance;
            }
        }

        public void Open(String path)
        {
            logger.Info(String.Format("Opening lucene Directory on {0}", path));
            try
            {
                luceneIndexDirectory = FSDirectory.Open(path);
            }
            catch ( Exception err)
            {
                logger.Error("Something wrong opening lucene Directory", err);
                throw err;
            }
        }

        public void Close()
        {
            //analyzer.Close();
            if ( luceneIndexDirectory != null )
                luceneIndexDirectory.Dispose();
        }

        public void Add(Document doc, Term idTerm)
        {
            try
            {
                if (Exist(idTerm))
                {
                    Update(doc, idTerm);
                    return;
                }
            }
            catch ( Exception err)
            {
                logger.Error("Error on serarch document. Maybe segments not found", err);
            }

            logger.Debug("Creating new lucene Document");
           

            lock (lockThis)
            {
                logger.Debug("Writing lucene document to lucene index");
                writer = new IndexWriter(luceneIndexDirectory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED);
                writer.AddDocument(doc);

                try
                {
                    writer.Optimize(); // Call optimize once to improve performance
                    //writer.Flush();
                    writer.Dispose(); // Commit and dispose the object
                }
                catch (Exception err)
                {
                    logger.Error("Error on lucene", err);
                    writer.Rollback();
                    writer.Dispose();
                }
            }
        }

        public Boolean Exist(Term searchTerm)
        {
 
            IndexSearcher searcher = new IndexSearcher(luceneIndexDirectory);
            BooleanQuery innerExpr = new BooleanQuery();

            Query queryResult = new TermQuery(searchTerm);

            innerExpr.Add(queryResult, Occur.SHOULD);
            //execute the query
            TopDocs topDocs = searcher.Search(innerExpr, null, 1);

            return topDocs.TotalHits > 0;
        }

        public void Delete(Term idTerm)
        {
            writer = new IndexWriter(luceneIndexDirectory, analyzer, false, IndexWriter.MaxFieldLength.UNLIMITED);
            writer.DeleteDocuments(idTerm);
            writer.Commit();

            writer.Optimize(); // Should be done with low load only ...
            writer.Dispose();
        }

        public void Update(Document docToUpdate, Term idTerm)
        {
            // http://manfredlange.blogspot.it/2010/07/updating-lucene-index-green-version.html
            // http://www.ifdefined.com/blog/post/2009/02/Full-Text-Search-in-ASPNET-using-LuceneNET.aspx
            // http://hrycan.com/2009/11/26/updating-document-fields-in-lucene/
            // https://github.com/yusuke/lucene-examples/blob/master/src/test/java/luceneexamples/UpdateDocument.java

            LuceneNetUtil.Version LuceneVersion = LuceneNetUtil.Version.LUCENE_30;
            Analyzer analyzer = new StandardAnalyzer(LuceneVersion);
            
            writer = new IndexWriter(luceneIndexDirectory, 
                        analyzer,
                        false, // Don't create index
                        IndexWriter.MaxFieldLength.UNLIMITED);

            // Scenario 2: Delete a document and then Update the document
            // writer.DeleteDocuments(idTerm); 
            writer.UpdateDocument(idTerm, docToUpdate);
            writer.Commit();

            writer.Optimize(); // Should be done with low load only ...
            writer.Dispose();
        }

        public List<Int32> Search(BooleanQuery innerExpr, ILuceneDocumentComparator comparator)
        {
            List<Int32> ids = new List<Int32>();

            IndexSearcher searcher = new IndexSearcher(luceneIndexDirectory);
            
            TopDocs topDocs = searcher.Search(innerExpr, null, 1000);

            if (topDocs != null)
            {
                int totalResults = topDocs.TotalHits;
                ScoreDoc[] scoreDocs = topDocs.ScoreDocs;

                foreach (ScoreDoc scoreDoc in scoreDocs)
                {
                    Document doc = searcher.Doc(scoreDoc.Doc);
                    if ( comparator.Check(doc) )
                        ids.Add(Int32.Parse(doc.GetField("id").StringValue));
                }
    
            }

            searcher.Dispose(); // da testare. bisogna fare la disponse della search alla fine
 
            return ids;
        }
    }
}