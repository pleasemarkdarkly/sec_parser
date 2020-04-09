using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using COI.DAL;
namespace COI.BLL.Parsers
{
    public class Source10LitigationsParser:ParserBase
    {
        private readonly DtstCOI.individualDataTable _individuals;
        private DtstCOI.individual_individual_linkDataTable _individualAliases;
        private readonly DtstCOI.companyDataTable _companies;
        private readonly CompanyManager _coMan;
        private readonly LinkManager _linMan;
        public Source10LitigationsParser(string url, int investigationId)
        {
            Url = url;
            InvestigationId = investigationId;
            SourceId = 10;
            DocType = "SEC litigations";
            _individuals = new IndividualManager().GetIndividuals();
            _coMan=new CompanyManager();
            _companies = _coMan.GetCompanies();
            _linMan=new LinkManager();
        }
        public void ParseLitigations(bool readOnly,TextWriter log)
        {
            var parsableDoc = GetDoc(log);
            var dbDoc = GetOrCreateDbDoc();
            log.WriteLine("Database doc ID: {0}", dbDoc.document_id);
            var litigations=ParseXML(log);
            var i = 1;
            foreach (var litigation in litigations) { 
                SearchOneLitigation(litigation, log, i, litigations.Count,readOnly);
                i++; }
        }
        private void SearchOneLitigation(rssChannelItem item,TextWriter log,int x,int y,bool readOnly)
        {   log.WriteLine("Processing: {0} ({1} of {2})",item.title,x,y);
            var litigationText = GetDoc(log, item.link).ToLower();
            var foundCompanies = new List<string>();
            DateTime date;
            try { date = DateTime.Parse(item.pubDate.Substring(4, 12)); 
            log.WriteLine("Litigation date: {0} parsed {1:yyyy-MM-dd}",item.pubDate,date);}
            catch (Exception ex) 
                { log.WriteLine(ex.ToString() + "\r\n----date:{0}\r\n----item{1}", item.pubDate, item.link);
                    return;}
            foreach (var company in _companies)
                if (litigationText.Contains(company.company_name.ToLower()))
                {
                    log.WriteLine("--Found company: {0}", company.company_name);
                    foundCompanies.Add(company.company_name);
                    if (!readOnly)
                    {
                        var dbDoc1 = GetOrCreateDbDoc(item.link, "full", date, "litigation");
                        var events = _coMan.GetEventsByCo(company.company_name);
                        if (events.FindBycompany_nameevent_typeevent_date(company.company_name, "litigation", date) ==null)
                        {
                            events.Addcompany_eventRow(company.company_name, "litigation",
                                                       "Company mentioned in SEC litigation release", date,
                                                       dbDoc1.document_id,
                                                       SourceId, "", false);
                            _coMan.SaveEvents(events);
                        }
                        else log.WriteLine("company event already exists in DB");
                    }
                }
            foreach (var individual in _individuals)
                if (litigationText.Contains(individual.individual_name.ToLower()))
                {
                    log.WriteLine("--Found individual: {0}", individual.individual_name);
                    if (readOnly) continue;
                    var links = _linMan.GetIndividualCompanyLinkByIndividual(individual.individual_name);
                    foreach (var coName in foundCompanies)
                    {
                        var found = false;
                        foreach (var link in links)
                        {
                            if (link.company_name != coName || link.link_type != "litigation") continue;
                            found = true;
                            break;
                        }
                        if (found) continue;
                        var dbDoc1 = GetOrCreateDbDoc(item.link, "full", date, "litigation");
                        links.Addindividual_company_linkRow(individual.individual_name, coName, SourceId,
                                                            dbDoc1.document_id, individual.individual_name,
                                                            coName,"litigation", date, string.Empty, date);
                        _linMan.Save(links);
                        log.WriteLine("created individual-company link: {0} -> {1}",individual.individual_name,coName);
                    }
                }
            log.Flush();
        }
        private List<rssChannelItem> ParseXML(TextWriter log)
        {
            var xrS = new XmlReaderSettings();
            var xr = XmlReader.Create(Url, xrS);
            var mySerializer = new XmlSerializer(typeof (rss));
            var myRss = (rss)mySerializer.Deserialize(xr);
            var list = new List<rssChannelItem>();
            foreach (var item in myRss.channel.item)list.Add(item);
            return list;
        }
    }
}
