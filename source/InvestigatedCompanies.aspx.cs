using System;
using COI.DAL;
namespace SECCrawler.Controller.FormsBrowser
{
    public partial class InvestigatedCompanies : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var manager = new CompanyManager();
            var table = manager.GetCompaniesKnown();
            this.GridView1.DataSource = table;
            this.GridView1.DataBind();
        }
    }
}
