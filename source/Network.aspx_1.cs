using System;
using COI.DAL;

namespace COI.WebUI.Diagrams
{
    public partial class Network : System.Web.UI.Page
    {
        #region declares
        private readonly InvestigationManager _dal=new InvestigationManager();
        public string FrameURL = string.Empty;

        #endregion
        #region initialize
        protected void Page_Load(object sender, EventArgs e)
        {
            FrameURL = string.Empty;
            if (IsPostBack) return;
            PopulateInvestigations();
        }
        private void PopulateInvestigations()
        {
            DropInvestigations.Items.Clear();
            var table = _dal.GetInvestigations();
            if (table.FindByinvestigation_id(0) == null)
                table.AddinvestigationRow(0, "(all)");
            DropInvestigations.DataSource = table;
            DropInvestigations.DataTextField = table.investigation_titleColumn.ColumnName;
            DropInvestigations.DataValueField = table.investigation_idColumn.ColumnName;
            DropInvestigations.DataBind();
            DropInvestigations.SelectedValue = "0";
        }
        #endregion
        #region diagram creation
        private void DoCircular()
        {
            if (DropInvestigations.SelectedValue=="0")
                FrameURL = "./alert.aspx?a="+Server.UrlEncode("This diagram requieres an investigation to be selected");
            else
                FrameURL = Request.Url.ToString().Replace("Network.aspx","") + "DependencyGraph.aspx?investigationID="+DropInvestigations.SelectedValue;
        }
        private void DoTree(string type)
        {   
            if (DropDownListViewTreesAs.SelectedValue=="text" &&
                ((type == "1" & DropIndividual.SelectedValue == "NO INVESTIGATED PERSON SELECTED")
                || (type == "0" & DropIndividual.SelectedValue == "NO COMPANY SELECTED")))
            {
                FrameURL = "./alert.aspx?a=" + Server.UrlEncode("This diagram requieres an individual to be selected");
                return;
            }
            switch (DropDownListViewTreesAs.SelectedValue)
            {
                case "text":
                    FrameURL = "TreeXML.aspx?&view=text&";
                    break;
                case "html":
                    FrameURL = "LinkTreeView.aspx?";
                    break;
                case "xml":
                    FrameURL = "TreeXML.aspx?view=xml&";
                    break;
            }
            FrameURL += "type=" + type + "&depth=" + DropDepth.SelectedValue;
            FrameURL += "&maxNodes=" + DropDownMaxNodes.SelectedValue;
            FrameURL += "&subject=";
            FrameURL += type == "1"
                            ? Server.UrlEncode(DropIndividual.SelectedValue)
                            : Server.UrlEncode(DropCompany.SelectedValue);
        }
        #endregion
        #region UIEvents
        protected void ButtonGo_Click(object sender, EventArgs e)
        {
            switch (RadioButtonList1.SelectedValue)
            {
                case "0":
                    DoCircular();
                    break;
                case "1":
                    DoTree("1");
                    break;
                case "2":
                    DoTree("0");
                    break;
                default:
                    FrameURL = string.Empty;
                    break;
            }
        }
        protected void ButtonFilterCompany_Click(object sender, EventArgs e)
        {
            if (TextBoxCompany.Text.Trim() == string.Empty) return;
            var manager = new CompanyManager();
            var table = manager.SearchCompanies(TextBoxCompany.Text.Trim().ToLower());
            DropCompany.Items.Clear();
            DropCompany.DataSource = table;
            DropCompany.DataTextField = table.company_nameColumn.ColumnName;
            DropCompany.DataValueField = table.company_nameColumn.ColumnName;
            DropCompany.DataBind();
            DropCompany.Enabled = DropCompany.Items.Count > 0;
        }
        protected void ButtonFilterIndividual_Click(object sender, EventArgs e)
        {
            if (TextBoxIndividual.Text.Trim()==string.Empty) return;
            var manager = new IndividualManager();
            var table = manager.SearchByName(TextBoxIndividual.Text.Trim().ToLower());
            DropIndividual.Items.Clear();
            DropIndividual.DataSource = table;
            DropIndividual.DataTextField = table.individual_nameColumn.ColumnName;
            DropIndividual.DataValueField = table.individual_nameColumn.ColumnName;
            DropIndividual.DataBind();
            DropIndividual.Enabled = DropIndividual.Items.Count > 0;
        }
        #endregion
    }
}
