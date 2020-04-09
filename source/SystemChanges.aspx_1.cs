using System;
using System.IO;
using System.Web;
using COI.BLL.Parsers;
using COI.DAL;

namespace COI.WebUI.Investigator.Batch
{
    public partial class SystemChanges : System.Web.UI.Page
    {
        protected static BatchManager.BatchNamesEnum OTCBB1 = BatchManager.BatchNamesEnum.OTCBBSystemChange;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            var outp=new Batch2WebAux(Response,OTCBB1);
            outp.AddHead();
            const string l = "http://otcbb.com/asp/dailylist_search.asp?SearchSymbolForm=TRUE&OTCBB=OTCBB&searchby=name&image1.x=33&image1.y=7&searchwith=Contains&searchfor=";
            var companies = new CompanyManager().GetCompanies();
            var countCo = 0D;
            foreach (var company in companies)
            {
                countCo++;
                var url = l + HttpUtility.UrlEncode(company.company_name);
                var parser = new Source80SystemChangesParser(url, 99);
                var log1 = parser.SaveNameSymbolChanges();
                outp.WriteLine(log1);
                outp.SetProgress(countCo/(double)companies.Rows.Count);
            }
            outp.AddFoot();
            outp.Save();
        }
    }
}
