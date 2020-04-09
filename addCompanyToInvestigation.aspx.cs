using System;
using SECCrawler.DAL;
using System.Web.UI.WebControls;
using System.Web;
namespace SECCrawler.Controller.FormsBrowser
{
    public partial class AddCompanyToInvestigation : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var manager = new COIInvestigationManager();
                var table = manager.GetInvestigations();
                this.DropInvestigations.DataSource = table;
                this.DropInvestigations.DataValueField = "investigationId";
                this.DropInvestigations.DataTextField = "InvestigationShortName";
                this.DropInvestigations.DataBind();
            }
        }

        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            if (this.TextSearch.Text.Trim() != string.Empty)
            {
                var manager = new SECCompaniesManager();
                var table = manager.GetCompaniesByNameSearc(this.TextSearch.Text.Trim());
                this.ButtonSubmit.Enabled=(table.Rows.Count>0);
                this.TextComment.Enabled = (table.Rows.Count > 0);
                this.CheckBoxCompanies.DataSource = table;
                CheckBoxCompanies.DataTextField = "CompanyName";
                CheckBoxCompanies.DataValueField = "CompanyCIK";
                CheckBoxCompanies.DataBind();
            }
        }

        protected void ButtonSubmit_Click(object sender, EventArgs e)
        {
            var manager = new COIInvestigationManager();
            int investigationId = int.Parse(DropInvestigations.SelectedValue);
            var table = manager.GetCompaniesByInvestigation(investigationId);
            var tableI = manager.GetInvestigations();
            var rowI = tableI.FindByInvestigationID(investigationId);
            var nw = 0;
            var old = 0;
            foreach (ListItem v in CheckBoxCompanies.Items)
            {
                if(v.Selected)
                {
                    var row = table.FindByCompanyCIKInvestigationID(decimal.Parse(v.Value), investigationId);
                    if (row == null)
                    {
                        table.AddtblCOI_InvestigationCompanyRow(decimal.Parse(v.Value), rowI, false,
                                                                this.TextComment.Text);
                        nw++;
                    }
                    else old++;
                }
            }
            manager.Save(table);
            Response.Redirect("../default.aspx?msg=" +
                HttpUtility.UrlEncode("Succesfuly added " + nw.ToString() + " companies also found "+
                old.ToString() + " companies that were already under investigation")
                );
        }
    }
}
