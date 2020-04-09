using NUnit.Framework;
using SECCrawler.IngestLib;
namespace SECCrawler.Test
{
    [TestFixture]
    public class TestIngestor
    {
        [Test]
        public void TestIngestorTick()
        {
            var ingestor = new IngestSecFormBlob(true);
            ingestor.DoIngest();
        }
    }
}
