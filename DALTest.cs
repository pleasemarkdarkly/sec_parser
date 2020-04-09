using System;
using NUnit.Framework;
using COI.DAL;
using COI.Util;
namespace COI.Test
{
    [TestFixture]
    public class DALTest
    {
        [Test]
        public void TestXMLExportOfLinks()
        {
            var dal = new XMLManager();
            var reader = dal.GetLinks(100);
            Console.Write(reader);

        }
    }
}
