using System;
using SECCrawler.BLL;
using NUnit.Framework;

namespace SECCrawler.Test
{
    [TestFixture]public class DocAnalyzerTest
    {
        [Test]public void TestTableExtractor()
        {
            var a = new SECFormsDocAnalyzer();
            var t = a.GetTable(new Guid("a927c1ca-0e1b-46a4-b0bb-98ca8465f767"),1,"market information" );
            foreach (var row in t)
            {
                foreach (var cell in row)
                {
                    Console.Write(cell.Trim()+"|");
                }
                Console.WriteLine();
            }
        }
    }
}
