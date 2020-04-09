using NUnit.Framework;
using SECCrawler.BLL;
using SECCrawler.DAL;
using COI.DAL;
using System;
namespace SECCrawler.Test
{
    [TestFixture]
    public class DALTests
    {
        [Test]
        public void TestGetDownloadQItem()
        {
            var dal = new SECFormsManager();
            var dtst =dal.GetFormDownloadQueue();
            Assert.IsNotNull(dtst);
        }
        [Test]
        public void TestGetIngestQItem()
        {
            var dal = new SECFormsManager();
            var dtst = dal.GetFormIngestQueue();
            Assert.IsNotNull(dtst);
            Assert.IsTrue(dtst.tblSEC_Forms.IsInitialized);
        }
        [Test]
        public void TestFullTextSearch()
        {
            var dal = new SECFormsManager();
            var table = dal.GetFormsByFullTextSearchAndFormType("Chris","(all)",LinkResponseType.NotChanged,string.Empty);
            Assert.IsNotNull(table);
            Assert.Greater(table.Rows.Count,0);
        }
        [Test]
        public void TestStatisticsSp()
        {
            var dal = new CoiStatisicsManager();
            var table = dal.GetCrawlerStatistics();
            Assert.IsNotNull(table);
            Assert.Greater(table.Rows.Count, 0);
            foreach (var row in table)
            {
                Console.WriteLine(row.Count.ToString("#,###") + " " + row.Description);
            }
        }
        [Test]
        public void TestGetEventTypes()
        {
            var dal = new CompanyManager();
            var table = dal.GetEventTypes();
            Assert.Greater(table.Rows.Count,0);
        }
        [Test] public void TestGetSubdoc()
        {
            var analyzer = new SECFormsDocAnalyzer();
            string newFilename;
            string type;
            var doc=analyzer.GetFormSubDoc(new Guid("08AA514B-BDF2-4DBA-B9F2-0139C3CA5512"), 0,out newFilename,out type);
            Console.WriteLine(doc);
        }
        [Test] public void TestGetParagraphs()
        {
            var analyzer = new SECFormsDocAnalyzer();
            var doc = analyzer.GetFormSubDocParagraphs(new Guid("08AA514B-BDF2-4DBA-B9F2-0139C3CA5512"), 0);
            foreach (var par in doc)
            {
                Console.WriteLine(par);
            }
        }
        [Test]
        public void TestGetSnippet()
        {
            var analyzer = new SECFormsDocAnalyzer();
            var doc = analyzer.GetFormSnippet(new Guid("08AA514B-BDF2-4DBA-B9F2-0139C3CA5512"), 0, "FORMER CONFORMED NAME");
                Console.WriteLine(doc);
        }
    }
}
