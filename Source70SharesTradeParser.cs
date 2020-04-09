using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using COI.DAL;

namespace COI.BLL.Parsers
{
    public class Source70SharesTradeParser:ParserBase 
    {
        public string CompanyName { get; set; }
        public int IntervalDays { get; set; }
        public Source70SharesTradeParser(string url, int investigationId, string companyName,int intervalDays)
        {
            Url = url;
            InvestigationId = investigationId;
            SourceId = 70;
            DocType = "Google finance";
            CompanyName = companyName;
            IntervalDays = intervalDays;
        }
        public Source70SharesTradeParser(string url, int investigationId, string companyName, int intervalDays,int infoSource)
        {
            Url = url;
            InvestigationId = investigationId;
            SourceId = infoSource;
            DocType = "Share trade data";
            CompanyName = companyName;
            IntervalDays = intervalDays;
        }
        public string CreateSharesData()
        {
            IFormatProvider culture = new CultureInfo("en-US", true);
            var dal = new SharesManager();
            var l = new StringWriter();
            var doc = GetDoc(l);
            var dbDoc = GetOrCreateDbDoc();
            var data = Url.ToLower().IndexOf("nasdaq") == -1 ? GetCellFromCSV(doc) : GetCellFromTSV(doc);
            var table = dal.GetSharesTraded(CompanyName,SourceId);
            foreach (var row in data)
            {
                try
                {
                    //Date,Open,High,Low,Close,Volume
                    //13-Oct-09,0.01,0.01,0.01,0.01,300
                    if (row.Count<6) continue;
                    if (row[0].ToLower()=="date") continue;
                    var date = DateTime.Parse(row[0], culture);//Exact(row[0], "dd-MMM-yy", culture);
                    for (var z = 1; z <= 5; z++) 
                    {   if (row[z] == "-") row[z] = 0.ToString();
                        if (row[z].IndexOf(',') != -1) row[z] = row[z].Replace(",", "");
                    }
                    var price = new decimal[]
                                    {decimal.Parse(row[1]),decimal.Parse(row[2])
                                    ,decimal.Parse(row[3]),decimal.Parse(row[4])};
                    var volume = int.Parse(row[5]);
                    var existing = table.FindBycompany_namedatesource_idintervalDays
                        (CompanyName, date,SourceId,IntervalDays);
                    if (existing == null)
                        table.Addcompany_shares_tradedRow(CompanyName, date, price[0], 
                            price[1], price[2], price[3],volume,dbDoc.document_id,SourceId,IntervalDays);
                }
                catch (Exception ex)
                {
                    l.WriteLine(ex.Message+string.Join(",",row.ToArray()));
                }
            }
            try
            {
                dal.Save(table);
                l.WriteLine("Shares traded saved");
            }
            catch (Exception ex)
            {
                l.WriteLine(ex.Message);
            }
            return l.ToString();
        }
    }
}
