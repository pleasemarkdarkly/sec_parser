using System;
using COI.BLL.Parsers;
using COI.DAL;

namespace COI.WebUI.Investigator.Batch
{
    public partial class SECOwnershipParse : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var dal = new CompanyManager();
            var q = dal.GetCompanyIDsByType("CIK");
            var outp = new Batch2WebAux(Response, BatchManager.BatchNamesEnum.SECOwnership);
            const string url = "http://sec.gov/cgi-bin/own-disp?action=getissuer&CIK=";
            outp.AddHead();
            var c = 0D;
            foreach (var company in q)
            {
                c++;
                outp.WriteLine("processing company " + company.company_name);
                var parser = new Source10OwnershipParser(url + company.identifier, 99);
                outp.WriteLine(parser.ParseOwnership(false));
                outp.SetProgress(c/(double)q.Rows.Count);
            }
            outp.AddFoot();
            outp.Save();
        }

    }
}
