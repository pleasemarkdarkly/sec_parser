using System;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using COI.DAL.Util;
using COI.DAL;
using COI.Util;
namespace COI.BLL.Parsers
{
    public class Source50PrintPageParser:ParserBase
    {
        private const string RegistredAgent = "Registered Agent";
        public Source50PrintPageParser(string url, int investigationId)
        {
            Url = url;
            InvestigationId = investigationId;
            SourceId = 50;
            DocType = "NV SOS link";
        }
        public string ParseEntitiyURL(bool readOnly)
        {
            ReadOnly = readOnly;
            var log = new StringWriter();
            var parsableDoc = GetDoc(log);
            var dbDoc = GetOrCreateDbDoc();
            log.WriteLine("Database doc ID: {0}",dbDoc.document_id);
            var companyName = SearchCompanyName(parsableDoc);
            if (companyName.Trim()==string.Empty)
            {log.WriteLine("Error! company name unknown");
             return log.ToString();}
            log.WriteLine("Company name: {0}", companyName);
            if (dbDoc.keywords == string.Empty) dbDoc.keywords = companyName;
            var fileDate = SearchFileDate(parsableDoc);
            log.WriteLine("Doc date: {0:yyy-MM-dd}", fileDate);
            dbDoc.document_date = fileDate;
            //Registered agent
            var regAg = SearchRegisteredAgentName(parsableDoc);
            log.WriteLine("{0}: {1}",RegistredAgent, regAg);
            var officers = SearchOfficers(parsableDoc);
            foreach (var officer in officers)
            {
                log.WriteLine("Officer position - name: {0}", officer);
                
            }
            var actDates = new List<DateTime> ();
            var acts = SearchActs(parsableDoc, actDates);
            var i = 0;
            foreach (var act in acts)
            {
                log.WriteLine("Acts: {0} {1:dd-MM-yyyy}",act,actDates[i]);
                i++;
            }
            if (!readOnly)
            {
                new DocumentManager().Save((DtstCOI.documentDataTable)dbDoc.Table);
                CreateLinks(dbDoc,officers,regAg,companyName);
                CreateEvents(acts,actDates,companyName,dbDoc);
            }
            return log.ToString();
        }
        private void CreateEvents(IList<string> acts, IList<DateTime> dates, string companynName,DtstCOI.documentRow doc)
        {
            companynName = Dbo.FilterName(companynName);
            var dal = new CompanyManager();
            var events = dal.GetEventsByCo(companynName);
            for (var i=0;i<acts.Count;i++)
            {
                var row = events.FindBycompany_nameevent_typeevent_date(companynName, acts[i], dates[i]);
                if (row == null)
                    events.Addcompany_eventRow(companynName, acts[i], acts[i], dates[i], doc.document_id, SourceId,string.Empty,false);
            }
            dal.SaveEvents(events);
        }
        private void CreateLinks(DtstCOI.documentRow doc,
            IList<string> officers, string registeredAgent,string companyName)
        {
            //officers
            var dal = new LinkManager();
            for (var i=0;i<officers.Count;i++)
            {
                var pos = officers[i].Split('-')[0].Trim().ToLower();
                var name =NameAnalyzer.NameCapitalizer( officers[i].Split('-')[1].Trim());
                dal.CreateOrUpdate(InvestigationId, companyName, name,name, SourceId, doc.document_id,
                                   "Officer", doc.document_date, pos);
            }
            //registred agent
            if (NameAnalyzer.IsCompanyName(registeredAgent))
            {
                registeredAgent = Dbo.FilterName(registeredAgent);
                companyName = Dbo.FilterName(companyName);
                var table = dal.GetLinksByCoDateAndLinkType(companyName, registeredAgent, doc.document_id,RegistredAgent);
                if (table.Rows.Count==0)
                {
                    table.Addcompany_company_linkRow(companyName, registeredAgent, SourceId, doc.document_id,
                                                     RegistredAgent, RegistredAgent, doc.document_date,doc.document_date
                                                     );
                    dal.Save(table);
                }
                var dal3 = new CompanyManager();
                var companies = dal3.SearchCompanies(registeredAgent);
                if (companies.FindBycompany_name(registeredAgent)==null)
                {
                    companies.AddcompanyRow(registeredAgent);
                    dal3.Save(companies);
                }
            }
            else
            {
                dal.CreateOrUpdate(InvestigationId, companyName, registeredAgent,registeredAgent, SourceId, doc.document_id,
                                    "Registered Agent", doc.document_date, "Registered Agent"); 
            }
        }
        private static List<string> SearchActs(string doc,ICollection<DateTime> dates)
        {
            dates.Clear();
            IFormatProvider culture = new CultureInfo("en-US", true);
            const string s0 = "<!-- Actions Table -->";
            const string s1 = "<td width=\"79%\" class=\"TDTypeFPrint\" colspan=\"3\">&nbsp;";
            const string s2 = "</td>";
            const string s3 = "File Date:&nbsp;</td><td width=\"29%\" class=\"TDTypeFPrint\">&nbsp;";
            var srch = new List<string>();
            var index0 = doc.IndexOf(s0);
            if (index0 > 0)
            {
                var index1 = index0 + s0.Length;
                while (doc.IndexOf(s1, index1) > 0)
                {
                    index1 = doc.IndexOf(s1, index1);
                    var index2 = doc.IndexOf(s2, index1 + s1.Length);
                    srch.Add(doc.Substring(index1 + s1.Length, index2 - (index1 + s1.Length)));
                    index1 = index2 + s2.Length;
                    var index3 = doc.IndexOf(s3, index1);
                    index2 = doc.IndexOf(s2, index3 + s3.Length);
                    var dString = doc.Substring(index3 + s3.Length, index2 - (index3 + s3.Length));
                    dates.Add(DateTime.Parse( dString ,culture));
                }
            }
            return srch;
        }
        private static List<string> SearchOfficers(string doc)
        {
            const string s0 = "<!--Officer Info -->";
            const string s1 = "<td class=\"TDTypeFPrint\" colspan=\"4\">&nbsp;" ;
            const string s2 = "</td>";
            var srch = new List<string>();
            var index0 = doc.IndexOf(s0);
            if (index0 > 0)
            {
                var index1 = index0 + s0.Length;
                while (doc.IndexOf(s1, index1) > 0)
                {
                    index1 = doc.IndexOf(s1, index1);
                    var index2 = doc.IndexOf(s2, index1 + s1.Length);
                    if (index2>0)
                        srch.Add(doc.Substring(index1 + s1.Length, index2 - (index1 + s1.Length)));
                    index1 = index2 + s2.Length;
                }
            }
            return srch;
        }
        private static string SearchRegisteredAgentName(string doc)
        {
            const string s0 = "Registered Agent Information";
            const string s1 = "Name:&nbsp;</td><td width=\"29%\" class=\"TDTypeFPrint\">&nbsp;";
            const string s2 = "</td>";
            var srch = string.Empty;
            var index0 = doc.IndexOf(s0);
            if (index0 > 0)
            {
                var index1 = doc.IndexOf(s1,index0+s0.Length);
                if (index1 > 0)
                {
                    var index2 = doc.IndexOf(s2, index1 + s1.Length);
                    srch = doc.Substring(index1 + s1.Length, index2 - (index1 + s1.Length));
                }
            }
            return srch;
        }
        private static string SearchCompanyName(string doc)
        {
            const string s1= "CorpDetailsGrid_lblCompanyName\">";
            const string s1A="<span id=\"ctl00_lblCompanyName\">";
            const string s2 = "</span>";
            var companyName = string.Empty;
            var index1 = doc.IndexOf(s1);
            if (index1 == -1) index1 = doc.IndexOf(s1A);
            if (index1>0)
            {
                var index2 = doc.IndexOf(s2,index1+s1.Length);
                companyName = doc.Substring(index1 + s1.Length, index2-(index1+s1.Length));
            }
            return companyName;
        }
        private static DateTime SearchFileDate(string doc)
        {   //"File Date:&nbsp;</td><td width=\"29%\" class=\"TDTypeFPrint\">&nbsp;9/15/2009 11:51:18 AM</td>";
            const string s1 = "File Date:&nbsp;</td><td width=\"29%\" class=\"TDTypeFPrint\">&nbsp;";
            const string s2="</td>";
            var fileDate = DateTime.Now;
            var index1 = doc.IndexOf(s1);
            if (index1>0)
            {
                var index2 = doc.IndexOf(s2,index1+s1.Length);
                if (index2>0)
                    fileDate =DateTime.Parse( doc.Substring(index1 + s1.Length, index2-(index1+s1.Length)));
            }
            return fileDate;
        }
    }
}