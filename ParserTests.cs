using System;
using NUnit.Framework;
using COI.BLL.Parsers;
using System.IO;
namespace COI.Test
{
    [TestFixture]public class ParserTests
    {
        [Test]public void TestNVparser()
        {
            var parser = new Source50PrintPageParser("https://esos.state.nv.us/SoSServices/AnonymousAccess/CorpSearch/PrintCorp.aspx?lx8nvq=GsZs0e2M6dadBn%252fJvzv4og%253d%253d", 99);
            Console.Write(parser.ParseEntitiyURL(true));
        }
        [Test][Explicit]
        public void TestNVparserWithDBWrites()
        {
            var parser = new Source50PrintPageParser("https://esos.state.nv.us/SoSServices/AnonymousAccess/CorpSearch/PrintCorp.aspx?lx8nvq=GsZs0e2M6dadBn%252fJvzv4og%253d%253d", 99);
            Console.Write(parser.ParseEntitiyURL(false));
        }
        [Test][Explicit]
        public void TestNSsearchResultParser()
        {
            var parser = new Source50SearchByAgentResultsParseToPrintEntityDetailsFormat
                ("https://esos.state.nv.us/SoSServices/AnonymousAccess/CorpSearch/RACorps.aspx?fsnain=YftlDHXPhX1ONrvPrtBqRA%253d%253d&RAName=cane+clark+agency+llc",
                 99);
            var log = new StringWriter();
            var links = parser.GetEntityDetailsPagesLinks(log);
            Console.Write(log);
            Assert.IsNotEmpty(links);
        }
        [Test]public void TestHtmlExtract()
        {
            const string t = "<a href=\"/Archives/edgar/data/0000878146/000125529405000179/0001255294-05-000179-index.htm\">5</a>";
            var p = new ParserBase();
            var o=p.GetLinkText(t);
            Assert.AreEqual(o,"5");
        }
        [Test]public void TestSystemChangeParser1()
        {
            const string l = "http://otcbb.com/asp/dailylist_search.asp?DirectSymbol=LATI&OTCBB=OTCBB";
            var parser = new Source80SystemChangesParser(l, 99);
            var log=parser.SaveNameSymbolChanges();
            Console.WriteLine(log);
        }
        //http://otcbb.com/asp/dailylist_search.asp?SearchSymbolForm=TRUE&OTCBB=OTCBB&searchby=name&image1.x=33&image1.y=7&searchwith=Contains&searchfor={0}
        [Test]public void TestSystemChangeParser2()
        {
            const string l = "http://otcbb.com/asp/dailylist_search.asp?SearchSymbolForm=TRUE&OTCBB=OTCBB&searchby=name&image1.x=33&image1.y=7&searchwith=Contains&searchfor=MW+medical";
            var parser = new Source80SystemChangesParser(l, 99);
            var log = parser.SaveNameSymbolChanges();
            Console.WriteLine(log);
        }
        [Test]public void TestLitigationsParser()
        {
            var parser = new Source10LitigationsParser("http://sec.gov/rss/litigation/litreleases.xml", 99);
            parser.ParseLitigations(false,Console.Out);
        }
    }
}
