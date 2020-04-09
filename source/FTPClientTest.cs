using NUnit.Framework;
using SECCrawler.FTPClient;
namespace SECCrawler.Test
{
    [TestFixture]
    public class FtpClientTest
    {
        [Test]
        public void TestDownloadOne()
        {
            const string remotePath = "ftp://ftp.sec.gov/";
            const string localPath = @"C:\Data\Work\DataStores\SECcrawler";
            const string formPartialURL = "edgar/data/1081418/0000950134-00-010446.txt";
            string outText;
            var downloader = new FtpClient(remotePath,localPath);
            downloader.Download(formPartialURL,true,out outText);
            Assert.IsEmpty(outText);
        }
        [Test]
        public void TestProcessQ()
        {
            var downloader = new Downloader(false);
            var errors=downloader.ProcessQueue();
            
        }
        [Test]
        public void TestProcessTick()
        {
            var processInitiator = new DownloadInitiator();
            var errors = processInitiator.ProcessTickConsoleVersion();
            
        }
    }
}