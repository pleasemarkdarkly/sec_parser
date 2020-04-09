using System;
using System.IO;
using COI.DAL;
using System.Web;
namespace COI.WebUI.Charts
{
    public partial class CompanyView : System.Web.UI.Page
    {
        #region UI events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            if (string.IsNullOrEmpty(Request.QueryString["coName"])) return;
            HiddenCoName.Value = Request.QueryString["coName"];
            TextSearch.Text = HiddenCoName.Value;
            ButtonSearch_Click(sender, e);
            try
            {
                DropResults.SelectedValue = HiddenCoName.Value;
            }
            catch
            {
                
            }

            DisplayCo();
        }
        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            if (TextSearch.Text.Trim()==string.Empty) return;
            var manager = new CompanyManager();
            var table = manager.SearchCompanies(TextSearch.Text.Trim().ToLower());
            DropResults.Items.Clear();
            DropResults.DataSource = table;
            DropResults.DataTextField = table.company_nameColumn.ColumnName;
            DropResults.DataValueField = table.company_nameColumn.ColumnName;
            DropResults.DataBind();
        }
        protected void ButtonDisplayCo_Click(object sender, EventArgs e)
        {
            if (DropResults.Items.Count>0)
                try
                {
                    HiddenCoName.Value = DropResults.SelectedValue;
                    DisplayCo();
                }
                catch (Exception ex)
                {
                    LabelError.Text = ex.ToString();
                    LabelError.Visible = true;
                }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (DropResults.Items.Count > 0)
                try
                {
                    HiddenCoName.Value = DropResults.SelectedValue;
                    DisplayCo();
                }
                catch (Exception ex)
                {
                    LabelError.Text = ex.ToString();
                    LabelError.Visible = true;
                }
            var coName = HiddenCoName.Value;
            if (coName == string.Empty)
            {
                PanelCo.Visible = false;
                return;
            }
            Response.Redirect("./CompanyTimeline.aspx?coName="+HttpUtility.UrlEncode(coName));
        }
        protected void ButtonTimeline2_Click(object sender, EventArgs e)
        {
            if (DropResults.Items.Count > 0)
                try
                {
                    HiddenCoName.Value = DropResults.SelectedValue;
                    DisplayCo();
                }
                catch (Exception ex)
                {
                    LabelError.Text = ex.ToString();
                    LabelError.Visible = true;
                }
            var coName = HiddenCoName.Value;
            if (coName == string.Empty)
            {
                PanelCo.Visible = false;
                return;
            }
            Response.Redirect("./CompanyTimeline2.aspx?coName=" + HttpUtility.UrlEncode(coName));
        }
        #endregion
        public void DisplayCo()
        {
            const string linkBase = "./CompanyView.aspx?coName=";
            var coName = HiddenCoName.Value;
            if (coName==string.Empty)
            {
                PanelCo.Visible = false;
                return;
            }
            PanelCo.Visible = true;
            LabelCoName.Text = HiddenCoName.Value;
            LiteralIndividuals.Text = GetIndividualsRow(coName);
            LiteralCoCo.Text = GetCompaniesRows(coName);
            LiteralEvent.Text = GetEventsRows(coName);
            LiteralIdentifiers.Text = GetIDs(coName);
            var dal = new LinkManager();
            Literal1.Text = dal.GetPreviousLinks(coName, true, linkBase);
            Literal2.Text = dal.GetPreviousLinks(coName, false, linkBase);
            var docMan = new DocumentManager();
            var snippets = docMan.GetSnippets(coName);
            Literal3.Text = string.Empty;
            foreach (var snippet in snippets)
            {
                var doc = docMan.GetDocument(snippet.document_id);
                Literal3.Text += string.Format("Document Type: {0} Date: {1:yyyy-MM-dd} Snippet Type: {2}<br />",doc[0].document_type,doc[0].document_date,snippet.snippet_type);
                Literal3.Text += snippet.snippet;
            }
        }
        private static string GetEventsRows(string coName)
        {
            const string indRow = "<tr><td>{0}</td><td>{1}</td><td>{2:yyyy-MM-dd}</td><td>{3}</td></tr>";
            var linMan = new CompanyManager();
            var events = linMan.GetEventsByCo(coName);
            var s = new StringWriter();
            foreach (var event1 in events)
            {
                object[] args = { event1.event_type, event1.event_description, event1.event_date, 
                                    event1.supporting_doc_id};
                s.WriteLine(indRow, args);
            }
            return s.ToString();
        }
        private static string GetCompaniesRows(string coName)
        {
            const string indRow = "<tr><td>{0}</td><td>{1}</td><td>{2:yyyy-MM-dd}</td><td>{3}</td></tr>";
            var linMan = new LinkManager();
            var companyLinks = linMan.GetCoCoLinks(coName);
            var s = new StringWriter();
            foreach (var link in companyLinks)
            {
                object[] args = { link.company_name, link.link_type, link.link_date, link.supporting_doc_id };
                if (args[0].ToString() == coName) args[0] = link.company_name2;
                s.WriteLine(indRow, args);
            }
            return s.ToString();
        }
        private static string GetIDs(string coName)
        {
            const string indRow = "<tr><td>{0}</td><td>{1}</td><td>{2}</td></tr>";
            var coManager = new CompanyManager();
            var companyIDs = coManager.GetCompanyIDs(coName);
            var s = new StringWriter();
            foreach (var id in companyIDs)
            {
                object[] args = { id.identifier,id.identifier_type,id.source_id };
                s.WriteLine(indRow, args);
            }
            return s.ToString();
        }
        private static string GetIndividualsRow(string coName)
        {
            const string indRow = "<tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>";
            var linMan = new LinkManager();
            var individualCompanyLinks = linMan.GetIndividualCompanyLink(coName);
            var s = new StringWriter();
            foreach (var link in individualCompanyLinks)
            {
                var position = string.Empty;
                if (!link.IspositionNull()) position = link.position;
                var date = string.Empty;
                if (!link.Islink_dateNull()) date = link.link_date.ToString("yyyy-MM-dd");
                object[] args = { link.individual_name, link.link_type, date, position, link.supporting_document };
                if (link.Islink_dateNull()) args[2] = string.Empty;
                s.WriteLine(indRow, args);
            }
            return s.ToString();
        }
    }
}
