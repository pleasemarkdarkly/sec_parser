using System;
using System.Globalization;
using System.IO;
using COI.DAL.Util;
using COI.DAL;
using System.Collections.Generic;
namespace COI.BLL.Parsers
{
    public class Source80SystemChangesParser : ParserBase
    {
        protected readonly string OriginalURL;
        private const string MultiMAtch = "<p>More than one issue matches your search criteria.  Please select the issue from the following list.</p>";
        public Source80SystemChangesParser(string url, int investigationId)
        {
            Url = url;
            OriginalURL = url;
            InvestigationId = investigationId;
            SourceId = 80;
            DocType = "OTCBB System Change";
        }
        public List<string> ExtractMultipleSymbols(string doc)
        {
            const string urlbase = "http://otcbb.com/asp/";
            const string s0 = "href=\"";
            const string s1 = "\"";
            if (doc == null) doc = GetDoc();
            var urls = new List<string>();
            var subdoc = GetSubDoc(doc, MultiMAtch, "</table>");
            var rows = GetRows(subdoc);
            foreach (var row in rows)
            {
                var cells = GetCells(row);
                foreach (var cell in cells)
                {
                    if (!cell.Contains(s0)) continue;
                    var index0 = cell.IndexOf(s0) + s0.Length;
                    var index1 = cell.IndexOf(s1, index0 + 1);
                    if (index1 - index0 > 0)
                        urls.Add(urlbase+ cell.Substring(index0, index1 - index0));
                }
            }
            return urls;
        }
        public string SaveNameSymbolChanges()
        {
            var log = new StringWriter();
            //if the linkg goes to a disambiguation page, run the process for each of the options in said page
            var doc = GetDoc(log);
            if (doc.Contains(MultiMAtch))
            {
                var urls = ExtractMultipleSymbols(doc);
                log.WriteLine(string.Format("multiple symbols found: {0}",urls.Count));
                foreach (var url in urls)
                {
                    log.WriteLine(url);
                }
                log.WriteLine("parsing every individual symbol");
                foreach (var url in urls)
                {
                    Url = url;
                    log.WriteLine( SaveNameSymbolChanges());
                }
                return log.ToString();
            }
            const string s0 = "<b>NAME/SYMBOL CHANGES</b>";
            const string s1 = "<hr>";
            const string symbol = "symbol";
            var dal = new CompanyManager();
            var dalL = new LinkManager();
            IFormatProvider culture = new CultureInfo("en-US", true);
            var dbDoc = GetOrCreateDbDoc();
            var subdoc = GetSubDoc(doc, s0, s1);
            var rows = GetRows(subdoc);
            foreach (var row in rows)
            {
                var cells = GetCells(row);
                if (cells.Count < 6) continue;
                try
                {
                    DateTime dateOfChange;
                    try{dateOfChange = DateTime.Parse(cells[1].Replace("&nbsp;", ""), culture);
                    }catch{continue;}
                    var oldSymbol = cells[2].Replace("&nbsp;", "").Replace("*","");
                    var newSymbol = cells[4].Replace("&nbsp;", "").Replace("*", "");
                    var oldName =
                        cells[3].Replace("&nbsp;", "").Replace("New Common Stock", "").Replace("Common Stock", "");
                    var newName =
                        cells[5].Replace("&nbsp;", "").Replace("New Common Stock", "").Replace("Common Stock", "");
                    oldName = Dbo.FilterName(oldName);
                    newName = Dbo.FilterName(newName);
                   log.WriteLine( CreateDbRecordsForNameChange(dal,dalL,oldName,newName,dateOfChange,dbDoc));
                   //identifiers
                   var oldIden = dal.GetOrCreateIdentifier(oldName, symbol, oldSymbol, SourceId);
                   log.WriteLine("In date {2:yyyy-MM-dd} Company name/symbol: {0}/{1}", oldIden.company_name,
                                 oldIden.identifier, dateOfChange);
                   var newIden = dal.GetOrCreateIdentifier(newName, symbol, newSymbol, SourceId);
                   log.WriteLine("Changed to: {0}/{1}", newIden.company_name, newIden.identifier);

                }
                catch (Exception ex)
                {
                    log.WriteLine("unable to parse this line");
                    foreach (var cell in cells)
                    {
                        log.Write(cell+"|");
                    }
                    log.WriteLine();
                    log.WriteLine(ex.Message);
                }
            }
            return log.ToString();
        }
        public string CreateDbRecordsForNameChange(CompanyManager dal,LinkManager dalL, string oldName, string newName, DateTime dateOfChange, DtstCOI.documentRow dbDoc)
        {
            const string eventType = "name/symbol change";
            const string linkType = "previous";
            var log = new StringWriter();
            //company names
            var companies = dal.SearchCompanies(oldName);
            var oldCo = companies.FindBycompany_name(oldName);
            if (oldCo == null)
            {
                companies.AddcompanyRow(oldName); dal.Save(companies);
                log.WriteLine("created co: {0}", oldName);
            }
            companies = dal.SearchCompanies(newName);
            var newco = companies.FindBycompany_name(newName);
            if (newco == null)
            {
                companies.AddcompanyRow(newName); dal.Save(companies);
                log.WriteLine("created co: {0}", newName);
            }
            //name change events and links
            var links = dalL.GetCoCoLinks(oldName, linkType);
            var link = links.FindBycompany_namecompany_name2supporting_doc_idlink_type(oldName, newName,
                    dbDoc.document_id, linkType);
            if (link == null)
            {
                links.Addcompany_company_linkRow(oldName, newName, SourceId, dbDoc.document_id,
                    linkType, linkType, dateOfChange, dateOfChange);
                dalL.Save(links);
                log.WriteLine("created company-company link");
            }
            var events = dal.GetEventsByCo(oldName);
            if (events.FindBycompany_nameevent_typeevent_date(oldName, eventType, dateOfChange) == null)
            {
                events.Addcompany_eventRow(oldName, eventType, eventType, dateOfChange, dbDoc.document_id,
                                           SourceId, string.Empty, false);
                dal.SaveEvents(events);
                log.WriteLine("created event {0} for company {1}", eventType, oldName);
            }
            events = dal.GetEventsByCo(newName);
            if (events.FindBycompany_nameevent_typeevent_date(newName, eventType, dateOfChange) == null)
            {
                events.Addcompany_eventRow(newName, eventType, eventType, dateOfChange, dbDoc.document_id,
                                           SourceId, string.Empty, false);
                dal.SaveEvents(events);
                log.WriteLine("created event {0} for company {1}", eventType, newName);
            }
            return log.ToString();
        }
    }
}