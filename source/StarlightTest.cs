using System;
using COI.Util;
using NUnit.Framework;
namespace COI.Test
{
    [TestFixture]
    public class StarlightTest
    {
        [Test]public void TestLinkExport()
        {
            var s = new StarlightEngine();
            Console.Write( s.GetLinks(101));
        }
    }
}
