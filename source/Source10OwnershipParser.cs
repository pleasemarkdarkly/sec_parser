using System.Collections.Generic;
using System.IO;
using COI.Util;
using COI.DAL;
using COI.DAL.Util;
using System;
namespace COI.BLL.Parsers
{
    public class Source10OwnershipParser:ParserBase
    {
        public Source10OwnershipParser(string url, int investigationId)
        {
            Url = url;
            InvestigationId = investigationId;
            SourceId = 10;
            DocType = "SEC Owner info";
        }
        public string ParseOwnership(bool readOnly)
        {
            ReadOnly = readOnly;
            var log = new StringWriter();
            var parsableDoc = GetDoc(log);
            var dbDoc = GetOrCreateDbDoc();
            log.WriteLine("Database doc ID: {0}", dbDoc.document_id);
            var company = SearchCompanyName(parsableDoc);
            log.WriteLine("Company: {0}",company);
            var owners = SearchOwners(parsableDoc);
            var trades = SearchSahresTraded(parsableDoc);
            foreach (var owner in owners)
            {
                log.WriteLine("Owner name: {0}",owner);
            }
            foreach (var trade in trades)
            {
                log.WriteLine("Shares traded: {0}",trade);
            }
            if (!readOnly)
            {
                log.WriteLine(CreateOwnerLinks(owners,company,dbDoc));
                log.WriteLine(CreateSharesTraded(trades,company,dbDoc));
            }
            return log.ToString();
        }
        private string CreateSharesTraded(List<string> trades,string companyName,DtstCOI.documentRow dbDoc)
        {
            companyName = Dbo.FilterName(companyName);
            if (ReadOnly) return "Readonly mode detected, Not saving share trade information";
            var log = new StringWriter();
            var dal = new SharesManager();
            var table = dal.GetIndividualSharesTradedByCo(companyName);
            foreach (var trade in trades)
            {
                try
                {
                    var details = trade.Split('|');
                    var date = DateTime.Parse(details[1]);
                    var traded = decimal.Parse(details[2]);
                    var owned = decimal.Parse(details[3]);
                    var form = int.Parse(details[5]);
                    var no = int.Parse(details[6]);
                    decimal price;
                    try
                    {
                        price = decimal.Parse(details[4]);
                    }
                    catch
                    {
                        price = 0;
                    }
                    var row = table.FindByindividual_namecompany_namedateformnumber
                        (details[0], companyName, date,form,no);
                    if (row == null)
                    {
                        table.Addindividual_company_sharesRow(details[0], companyName,
                            date,form,no, dbDoc.document_id, traded, owned, SourceId, price);
                    }
                }catch
                {
                }
            }
            dal.Save(table);
            return log.ToString();
        }
        private string CreateOwnerLinks(List<string> owners,string companyName,DtstCOI.documentRow dbdoc)
        {
            if (ReadOnly) return "Readonly mode detected, Not creating links";
            var log = new StringWriter();
            var dal = new LinkManager();
            foreach (var owner in owners)
            {
                try
                {

                    var details = owner.Split('|');
                    var date = DateTime.Parse(details[1]);
                    var positions =
                        details[2].ToLower().Replace("&", ",").Replace(" and ", ",").Replace(":", ",").Split(',');
                    foreach (var p in positions)
                    {
                        if (!NameAnalyzer.IsCompanyName(details[0]))
                        dal.CreateOrUpdate(InvestigationId, companyName, details[0], details[0],
                                           SourceId, dbdoc.document_id, "Owner", date, p.Trim());
                        else
                        {
                            dal.CreateOrUpdateCoCoLink(companyName,details[0],10,dbdoc.document_id,"Owner",p.Trim(),date);
                        }
                    }
                }
                catch(Exception ex)
                {log.WriteLine(ex.Message);}
            }
            return log.ToString();
        }
        private static string SearchCompanyName(string parsableDoc)
        {
            const string s1 = "<title>Ownership Information: ";
            const string s2 = "</title>";
            var companyName = string.Empty;
            var index1 = parsableDoc.IndexOf(s1);
            if (index1 > 0)
            {
                var index2 = parsableDoc.IndexOf(s2, index1 + s1.Length);
                companyName = parsableDoc.Substring(index1 + s1.Length, index2 - (index1 + s1.Length));
            }
            return companyName;
        }
        private List<string> SearchOwners(string parsableDoc)
        {
            var owners = new List<string>();
            try
            {
                const string s0 = "<b class=\"blue\">Ownership Reports from:</b>";
                const string s1 = "</table>";
                const string sOw0 = "getowner&amp;";
                const string sOw1 = ">";
                const string sOw2 = "</a>";
                var subdoc = GetSubDoc(parsableDoc, s0, s1);
                var rows = GetRows(subdoc);
                foreach (var row in rows)
                {
                    if (row.IndexOf(sOw0)==-1) continue;
                    var cells = GetCells(row);
                    var owner=new string[3];
                    //owner name
                    owner[0] = cells[0];
                    var indexOw0 = owner[0].IndexOf(sOw0);
                    var indexOw1 = owner[0].IndexOf(sOw1, indexOw0)+sOw1.Length;
                    var indexOw2 = owner[0].IndexOf(sOw2);
                    owner[0] = owner[0].Substring(indexOw1, indexOw2 - indexOw1);
                    owner[0] = NameAnalyzer.NameCapitalizer(owner[0]);
                    //date-position
                    owner[1] = cells[2];
                    owner[2] = cells[3];
                    owners.Add(string.Join("|",owner));
                }
                return owners;
            }catch (Exception ex)
            {
                return new List<string>{ex.ToString()};
            }
        }
        private List<string> SearchSahresTraded(string parsableDoc)
        {
            var trades = new List<string>();
            const string s0 = "is for derivative details.)<br>";
            const string s1 = "</table>";
            var subdoc = GetSubDoc(parsableDoc, s0, s1);
            if (subdoc == string.Empty) return trades;
            var rows = GetRows(subdoc);
            foreach (var row in rows)
            {
                var cells = GetCells(row);
                if( cells.Count!=13) continue;
                if (cells[0].Trim()==string.Empty || cells[0].Trim()=="&nbsp;")
                {  //this would be a derivative details row

                }
                else
                {  //name, date, shares, owned
                    var trade = new string[7];
                    trade[0] = NameAnalyzer.NameCapitalizer( cells[2]);
                    trade[1] = cells[1];
                    trade[2] = cells[6];
                    trade[3] = cells[8];
                    trade[4] = cells[7];
                    trade[5] = GetLinkText(cells[3]);
                    trade[6] = cells[9];
                    trades.Add(string.Join("|", trade));
                }
            }
            return trades;
        }
    }
}
