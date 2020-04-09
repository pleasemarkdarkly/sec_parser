using System;
using COI.DAL;
using COI.DAL.Util;
namespace SECCrawler.Controller.FormsBrowser
{
    public partial class SaveSnippetAsEvent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            try
            {
                var dtst = (DtstFilingSearchResult) Session["dtst"];
                var resId = int.Parse(Request.QueryString["resultId"]);
                var result = dtst.MPSresults.FindByrowID(resId);
                LabelSnippet.Text = result.Paragraph;
                TextCompany.Text =Dbo.FilterName( result.Company);
                Calendar1.SelectedDate = result.Date;
                Calendar1.VisibleDate = result.Date;
                TextBoxYear.Text = result.Date.Year.ToString();
                TextBoxMonth.Text = result.Date.Month.ToString();
                TextEvent.Text = result.Keyword;
                TextBoxDescription.Text = "";
                TextBoxKeywords.Text = result.Keyword;
                Panel1.Visible = true;
                LabelAlarm.Visible = false;
                var evnts = new CompanyManager().GetEventTypes();
                DropDownEvents.DataSource = evnts;
                DropDownEvents.DataTextField = evnts.event_typeColumn.ColumnName;
                DropDownEvents.DataValueField = evnts.event_typeColumn.ColumnName;
                DropDownEvents.DataBind();
            }
            catch (Exception ex)
            {
                Panel1.Visible = false;
                LabelAlarm.Text = "ERROR: most likely cause, your session has expired." + "<br /><br /><br />" + ex.ToString();
                LabelAlarm.Visible = true;
            }
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                var dtst = (DtstFilingSearchResult) Session["dtst"];
                var resId = int.Parse(Request.QueryString["resultId"]);
                var result = dtst.MPSresults.FindByrowID(resId);
                var m = new CompanyManager();
                m.CreateCompanyEvent(TextCompany.Text, TextEvent.Text, TextBoxDescription.Text
                                     , Calendar1.SelectedDate, result.FormID, TextBoxKeywords.Text, result.Paragraph);
                LabelAlarm.Text = "Event saved, you may close this window now";
                LabelAlarm.Visible = true;
                Panel1.Visible = false;
            }
            catch(Exception ex)
            {
                Panel1.Visible = false;
                LabelAlarm.Text = "ERROR: most likely cause, your session has expired." + "<br /><br /><br />" + ex.ToString();
                LabelAlarm.Visible = true;
            }
        }

        protected void DropDownEvents_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextEvent.Text = DropDownEvents.SelectedValue;
        }

        protected void ButtonYear_Click(object sender, EventArgs e)
        {
            try
            {
                var y=int.Parse(TextBoxYear.Text);
                var m = int.Parse(TextBoxMonth.Text);
                Calendar1.VisibleDate=new DateTime(y,m,1);
            }
            catch (Exception)
            {
            }
        }
    }
}
