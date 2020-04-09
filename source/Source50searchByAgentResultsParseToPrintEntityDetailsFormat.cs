using System.Collections.Generic;
using System.IO;
namespace COI.BLL.Parsers
{
    public class Source50SearchByAgentResultsParseToPrintEntityDetailsFormat : ParserBase
    {
        public Source50SearchByAgentResultsParseToPrintEntityDetailsFormat(string url, int investigationId)
        {
            Url = url;
            InvestigationId = investigationId;
            SourceId = 50;
            DocType = "NV SOS results page";
        }
        public List<string> GetEntityDetailsPagesLinks()
        {
            return GetEntityDetailsPagesLinks(new StringWriter());
        }
        public List<string> GetEntityDetailsPagesLinks(StringWriter log)
        {
            const string s0 = "objSearchGrid_tblResutls";
            const string s1="CorpDetails.aspx?lx8nvq=";
            const string s2 = "\">";
            const string snew = "https://esos.state.nv.us/SoSServices/AnonymousAccess/CorpSearch/PrintCorp.aspx?lx8nvq=";
            var links = new List<string>();
            var doc = GetDoc(log);
            if (doc.IndexOf(s0)>0)
            {
                doc = doc.Substring(doc.IndexOf(s0));
            }
            while (doc.IndexOf(s1)>0)
            {
                var link = doc.Substring(doc.IndexOf(s1) + s1.Length, doc.IndexOf(s2,doc.IndexOf(s1)+s1.Length) - (doc.IndexOf(s1) + s1.Length));
                link = snew + link;
                if (!links.Contains(link))
                {
                    links.Add(link);
                    log.WriteLine("Found link: {0}", link);
                }
                doc = doc.Substring(doc.IndexOf(s2, doc.IndexOf(s1) + s1.Length) + s2.Length);                
            }
            return links;
        }
    }
}
