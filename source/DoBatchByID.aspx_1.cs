using System;
using System.Collections.Generic;
using System.Globalization;
using COI.BLL.Parsers;
using COI.DAL;
using COI.Util;

namespace COI.WebUI.Investigator.Batch
{
    public partial class DoBatchByID : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                var batchType = (BatchManager.BatchNamesEnum)int.Parse(Request.QueryString["batchType"]);
                switch (batchType)
                {
                    case BatchManager.BatchNamesEnum.NameChangesFromSECCrawler:
                        DoNAmeChangesFromSEC();
                        break;
                    case BatchManager.BatchNamesEnum.NasdaqHistoricAllSymbols:
                        DoNasdaqHistoricAllSymbols();
                        break;
                    case BatchManager.BatchNamesEnum.SECImportCIK:
                        DoCIKImports();
                        break;
                    case BatchManager.BatchNamesEnum.SECLookupIndividuals:
                        DoSearchAllIndividualsinSEC();
                        break;
                    case BatchManager.BatchNamesEnum.ParseHistoricShareValuesFrom10K:
                        DoSECParseHistoricSharePricesFrom10K();
                        break;
                    case BatchManager.BatchNamesEnum.ParseLitigations:
                        DoSearchAllLitigations();
                        break;
                    default:
                        Response.Output.WriteLine(Batch2WebAux.Head);
                        Response.Output.WriteLine("Unknown batch type: " + Request.QueryString);
                        Response.Output.WriteLine(Batch2WebAux.Foot);
                        break;
                }
            }
            catch (Exception ex)
            {
                Response.Output.WriteLine(Batch2WebAux.Head);
                Response.Output.WriteLine("Unable to parse query string");
                Response.Output.WriteLine(ex.ToString());
                Response.Output.WriteLine(Batch2WebAux.Foot);
            }
        }
        private void DoSECParseHistoricSharePricesFrom10K()
        {
            var aux = new Batch2WebAux(Response, BatchManager.BatchNamesEnum.ParseHistoricShareValuesFrom10K);
            aux.AddHead();
            var c = 0D;
            var dal = new BatchManager();
            var forms = dal.GetFormsFor10KParsing();
            foreach (var form in forms)
            {
                c++;
                aux.WriteLine(string.Format("Parsing company {0} form {1} from date {2:yyyy-MM-dd}",form.CompanyName,form.FormType,form.FormDate));
                var parser = new Source10SharePriceFrom10K(form.formID,form.CompanyName);
                aux.WriteLine(parser.GenerateSnippet());
                aux.SetProgress(c/forms.Rows.Count);
            }
            aux.AddFoot();
            aux.Save();
        }
        private void DoNasdaqHistoricAllSymbols()
        {
            const string url = 
"http://charting.nasdaq.com/ext/charts.dll?2-1-14-0-0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0|0,0,0,0,0-5120-03NA000000{0}-&SF:4|5-WD=539-HT=395--XXCL-";
            var aux = new Batch2WebAux(Response, BatchManager.BatchNamesEnum.NasdaqHistoricAllSymbols);
            aux.AddHead();
            var dal = new CompanyManager();
            var companies = dal.GetCompanyIDsByType("symbol");
            var c = 0D;
            foreach (var company in companies)
            {
                c++;
                var url1 = string.Format(url, company.identifier);
                var parser = new Source70SharesTradeParser(url1, 99, company.company_name, 1,80);
                aux.WriteLine(parser.CreateSharesData());
                aux.SetProgress(c/companies.Rows.Count);
            }
            aux.AddFoot();
            aux.Save();
        }
        private void DoCIKImports()
        {
            var aux = new Batch2WebAux(Response, BatchManager.BatchNamesEnum.SECImportCIK);
            var dal = new BatchManager();
            aux.AddHead();
            aux.WriteLine(string.Format("Imported CIKs with no errors. Count= {0}", dal.DoCIKImports()));
            aux.AddFoot();
            aux.Save();
        }
        private void DoNAmeChangesFromSEC()
        {
            const string s0="FORMER CONFORMED NAME:";
            const string s1="DATE OF NAME CHANGE:";
            IFormatProvider culture = new CultureInfo("en-US", true);

            var dal = new DocumentManager();
            var dalC = new CompanyManager();
            var dalL = new LinkManager();
            var snippets = dal.GetSECSnippetsByType("former name");
            var aux = new Batch2WebAux(Response, BatchManager.BatchNamesEnum.NameChangesFromSECCrawler);
            aux.AddHead();
            var c = 0D;
            foreach (var snippet in snippets)
            try{
                aux.WriteLine(string.Format( "processing snippet for company {0} date of filing {1:yyyy-MM-dd}",snippet.CompanyName,snippet.FormDate));
                var s = snippet.snippet.Replace("\t"," ").Replace("\r\n"," ").Replace("    "," ").Replace("   "," ").Replace("  "," ");
                if (s.Contains(s0) && s.Contains(s1))
                {
                    var index0 = s.IndexOf(s0)+s0.Length;
                    var index1 = s.IndexOf(s1,index0);
                    var previousName = s.Substring(index0, index1 - index0);
                    var previousDate = s.Substring(index1 + s1.Length,10).Trim();
                    DateTime d;
                    try { d = DateTime.ParseExact(previousDate,"yyyyMMdd",culture); }
                    catch { try { d = DateTime.ParseExact(previousDate, "yyyy-MM-dd", culture); }
                    catch { try { d = DateTime.Parse(previousDate); } catch {d = DateTime.MinValue; } }}
                    if (d==DateTime.MinValue){aux.WriteLine("Unable to parse date "+previousDate);continue;}
                    aux.WriteLine(string.Format("Found previous name {0} and date of name change {1:yyyy-MM-dd}", previousName, d));
                    previousName = DAL.Util.Dbo.FilterName(previousName);
                    snippet.CompanyName = DAL.Util.Dbo.FilterName(snippet.CompanyName);
                    var parser = new Source80SystemChangesParser("ftp://ftp.sec.gov/" + snippet.FormPartialURL, 99);
                    var dbDoc = parser.GetOrCreateDbDoc();
                    aux.WriteLine( parser.CreateDbRecordsForNameChange(dalC, dalL, previousName, snippet.CompanyName, d, dbDoc));
                }
                else
                {
                    aux.WriteLine("snippet is not usable: " + s);
                }
                c++;
                aux.SetProgress(c/(double)snippets.Rows.Count);
            }catch(Exception ex1)
            {aux.WriteLine(string.Format("<hr /><b>exception</b><br />{0} <hr>", ex1.ToString()));}
            aux.AddFoot();
            aux.Save();
        }
        private void DoSearchAllIndividualsinSEC()
        {
            var dal = new IndividualManager();
            var dalL = new LinkManager();
            var dalI = new InvestigationManager();
            var aux = new Batch2WebAux(Response, BatchManager.BatchNamesEnum.SECLookupIndividuals);
            aux.AddHead();
            var table = dalI.GetInvestigatedIndividuals();
            var c = 0D;
            var alreadySearched = new List<string>();
            foreach (var ii in table)
            {
                if (alreadySearched.Contains(ii.individual_name.ToLower()))
                {
                    c++;
                    continue;
                }
                var names = new List<string> {ii.individual_name};
                var aliases = dalL.GetIILByName(ii.individual_name);
                foreach (var alias in aliases)
                {
                    if(alias.link_type=="alias" & !names.Contains(alias.individual_alias)) names.Add(alias.individual_alias);
                }
                NameAnalyzer.AutoAliases(ii.individual_name,names);
                foreach (var name in names)
                {
                    if (alreadySearched.Contains(name.ToLower())) continue;
                    try
                    {
                        if (ii.individual_name!=name)
                            aux.WriteLine("Searcing ALL SEC filings for: " + ii.individual_name + " under alias: " + name);
                        else
                            aux.WriteLine("Searcing ALL SEC filings for: " + ii.individual_name );
                        dal.ImportIndividualFromSrc10(ii.individual_name, name);
                    }catch(Exception ex2)
                    {aux.WriteLine(string.Format("<hr /><b>Exception:<br />\r\n</b>{0}<hr/>",ex2));}
                    alreadySearched.Add(name.ToLower());
                }
                c++;
                aux.SetProgress(c/table.Rows.Count);
            }
            aux.AddFoot();
            aux.Save();
        }
        private void DoSearchAllLitigations()
        {
            var aux = new Batch2WebAux(Response, BatchManager.BatchNamesEnum.ParseLitigations);
            var parser = new Source10LitigationsParser("http://sec.gov/rss/litigation/litreleases.xml", 99);
            parser.ParseLitigations(false,aux);
            aux.Save();
        }
    }
}
