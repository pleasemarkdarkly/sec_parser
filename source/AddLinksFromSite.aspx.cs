using System;
using COI.BLL.Parsers;
using COI.DAL;
using System.IO;
using System.Web;
namespace COI.WebUI.Investigator
{
    public partial class AddLinksFromSite : System.Web.UI.Page
    {
        #region declaresAndProperties
        private int _investigationId = 0;
        private int _infoSourceId = 0;
        private InfoSourceManager _dalSrc;
        protected InfoSourceManager InfoSourceDAL
        {
            get
            {
                if (_dalSrc == null)
                    _dalSrc = new InfoSourceManager();
                return _dalSrc;
            }
        }
        #endregion
        #region populate
        private void PopulateInvestigations()
        {
            var dalInv = new InvestigationManager();
            DropInvestigations.Items.Clear();
            var table = dalInv.GetInvestigations();
            DropInvestigations.DataSource = table;
            DropInvestigations.DataTextField = table.investigation_titleColumn.ColumnName;
            DropInvestigations.DataValueField = table.investigation_idColumn.ColumnName;
            DropInvestigations.DataBind();
        }
        private void PopulateInfoSources()
        {
            DropInfoSource.Items.Clear();
            var table = InfoSourceDAL.GetInfoSources();
            DropInfoSource.DataSource = table;
            DropInfoSource.DataTextField = table.source_nameColumn.ColumnName;
            DropInfoSource.DataValueField = table.source_idColumn.ColumnName;
            DropInfoSource.DataBind();
        }
        private void PopulatePages()
        {
            DropPagesInfoSource.Items.Clear();
            var table = InfoSourceDAL.GetPagesByInfoSourceId(_infoSourceId);
            DropPagesInfoSource.DataSource = table;
            DropPagesInfoSource.DataTextField = table.shortNameColumn.ColumnName;
            DropPagesInfoSource.DataValueField = table.shortNameColumn.ColumnName;
            DropPagesInfoSource.DataBind();
        }
        private void PopulateLinkTypes()
        {
            var dal = new LinkManager();
            var table = dal.GetAllLinkTypes();
            DropManualLinkTypes.Items.Clear();
            DropManualLinkTypes.DataSource = table;
            DropManualLinkTypes.DataTextField = table.link_typeColumn.ColumnName;
            DropManualLinkTypes.DataValueField = table.link_typeColumn.ColumnName;
            DropManualLinkTypes.DataBind();
            var table2 = dal.GetAllPositionsOrTitles();
            DropManualPosition.Items.Clear();
            DropManualPosition.DataSource = table2;
            DropManualPosition.DataTextField = table2.link_titleColumn.ColumnName;
            DropManualPosition.DataValueField = table2.link_titleColumn.ColumnName;
            DropManualPosition.DataBind();
        }
        #endregion
        #region misc
        protected void ReadSelectedOptions()
        {
            _investigationId = int.Parse(DropInvestigations.SelectedValue);
            _infoSourceId = int.Parse(DropInfoSource.SelectedValue);
        }
        protected string GetIframeURL()
        {
            return TextURL.Text;
        }
        private void HideSearchControls()
        {
            Label1.Visible = false;
            Label2.Visible = false;
            TextBox1.Visible = false;
            TextBox2.Visible = false;
        }
        #endregion
        #region events
        protected void Page_PreRender(object sender, EventArgs e)
        {
            LabelAlarm.Visible = !string.IsNullOrEmpty(LabelAlarm.Text);
            PanelSrc50.Visible = DropInfoSource.SelectedValue == "50";
            PanelSrc10.Visible = DropInfoSource.SelectedValue == "10";
            PanelSrc70.Visible = DropInfoSource.SelectedValue == "70";
            PanelManualControls.Visible = CheckShowManualPanel.Checked;
            LabelGen10.Visible = LabelGen10.Text.Length > 0;
            LabelGen50ic.Visible = LabelGen50ic.Text.Length > 0;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                ReadSelectedOptions();
                return;
            }
            Calendar1.VisibleDate = DateTime.Now;
            Calendar2.VisibleDate = DateTime.Now;
            PopulateInfoSources();
            PopulateInvestigations();
            var loadInvId = Request.QueryString["InvestigationID"];
            if (!string.IsNullOrEmpty(loadInvId))
            {
                DropInvestigations.SelectedValue = loadInvId;
            }
            var sourceId = Request.QueryString["SourceID"];
            if (!string.IsNullOrEmpty(sourceId))
            {
                DropInfoSource.SelectedValue = sourceId;
            }
            ReadSelectedOptions();
            PopulatePages();
            PopulateLinkTypes();
            DropPagesInfoSource_SelectedIndexChanged(sender, e);
        }
        protected void LinkButtonCal1OneYearUp_Click(object sender, EventArgs e)
        { Calendar1.VisibleDate = Calendar1.VisibleDate.AddYears(1); }
        protected void LinkButtonCal1OneYearDown_Click(object sender, EventArgs e)
        { Calendar1.VisibleDate = Calendar1.VisibleDate.AddYears(-1); }
        protected void LinkButtonCal2OneYearDown_Click(object sender, EventArgs e)
        { Calendar2.VisibleDate = Calendar1.VisibleDate.AddYears(-1); }
        protected void LinkButtonCal2OneYearUp_Click(object sender, EventArgs e)
        { Calendar2.VisibleDate = Calendar1.VisibleDate.AddYears(1); }
        protected void DropPagesInfoSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            var pages = InfoSourceDAL.GetPagesByInfoSourceId(_infoSourceId);
            var row = pages.FindBysourceIdshortName(_infoSourceId, this.DropPagesInfoSource.SelectedValue);
            HideSearchControls();
            if (row == null)
            {

            }
            else
            {
                var searches = row.SearchTerms.Split('|');
                if (row.SearchTerms.Length > 0)
                {
                    Label1.Text = searches[0].Split('^')[0];
                    Label1.Visible = true;
                    TextBox1.Visible = true;
                    TextBox1.Text = "";
                }
                if (searches.Length > 1)
                {
                    Label2.Text = searches[1].Split('^')[0];
                    Label2.Visible = true;
                    TextBox2.Visible = true;
                    TextBox2.Text = "";
                }
            }
        }
        protected void DropInfoSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulatePages();
            DropPagesInfoSource_SelectedIndexChanged(sender, e);
        }
        protected void ButtonPreview_Click(object sender, EventArgs e)
        {
        }
        protected void DropManualLinkTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextLinkType.Text = DropManualLinkTypes.SelectedValue;
        }
        protected void DropManualPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            TextPosition.Text = DropManualPosition.SelectedValue;
        }
        protected void BtnDoLink_Click(object sender, EventArgs e)
        {
            var pages = InfoSourceDAL.GetPagesByInfoSourceId(_infoSourceId);
            var row = pages.FindBysourceIdshortName(_infoSourceId, this.DropPagesInfoSource.SelectedValue);
            //HideSearchControls();
            if (row == null)
            {
                var source = InfoSourceDAL.GetInfoSources();
                var iSrow = source.FindBysource_id(_infoSourceId);
                TextURL.Text = iSrow.landing_page;
            }
            else
            {
                var url = row.URL;
                var i = 0;
                var searches = row.SearchTerms.Split('|');
                foreach (var search in searches)
                {
                    var key = search.Split('^')[1];
                    var value = i == 0 ? TextBox1.Text : TextBox2.Text;
                    value = value.Replace(' ', '+');
                    url = url.Replace(key, value);
                    i++;
                }
                TextURL.Text = url;
            }
        }
        protected void ButtonGen50ic_Click(object sender, EventArgs e)
        {
            CreateLinksSrc50ParseSearchResults();
        }
        protected void ButtonSaveManual_Click(object sender, EventArgs e)
        {
            CreateLinksManually();
        }
        protected void ButtonGen50FromPrintPageClick(object sender, EventArgs e)
        {
            CreateLinks50ParseBusinessEntityPrintPage();
        }
        protected void ButtonSrc10Owner_Click(object sender, EventArgs e)
        {
            CreateLinksSrc10Owner();
        }
        protected void CheckShowManualPanel_CheckedChanged(object sender, EventArgs e){}
        #endregion
        #region createLinks
        public void CreateLinksSrc50ParseSearchResults()
        {
            var log = new StringWriter();
            var parseresult = new Source50SearchByAgentResultsParseToPrintEntityDetailsFormat(TextURL.Text, _investigationId);
            var links = parseresult.GetEntityDetailsPagesLinks(log);
            foreach (var link in links)
            {
                var parseEntity = new Source50PrintPageParser(link, _investigationId);
                log.Write(parseEntity.ParseEntitiyURL(false));
            }
            LabelGen50ic.Text = log.ToString().Replace("\r\n", "<br />\r\n");
        }
        public void CreateLinks50ParseBusinessEntityPrintPage()
        {
            var log = new StringWriter();
            var parseEntity = new Source50PrintPageParser(TextURL.Text, _investigationId);
            log.Write(parseEntity.ParseEntitiyURL(false));
            LabelGen50ic.Text = log.ToString().Replace("\r\n", "<br />\r\n");
        }
        public void CreateLinksManually()
        {
            try
            {   if (HttpUtility.UrlDecode(TextURL.Text)==string.Empty)
                    throw new Exception("The URL does not appeat to be valid");
                var d = new DocumentManager();
                var doc=d.CreateOrGetDocumentByURL(this.TextURL.Text, _infoSourceId, Calendar1.SelectedDate, 0, "", "WebPage");
                if (doc.Rows.Count==0) throw new Exception("Unable to create or retrieve database document for URL " + TextURL.Text);
                var m = new LinkManager();
                if (CheckBoxIIL.Checked)
                {

                    var table = m.GetIILByName(TextIndName.Text);
                    table.Addindividual_individual_linkRow(TextIndName.Text, TextIndAlias.Text, _infoSourceId,
                        doc[0].document_id, TextLinkType.Text, Calendar1.SelectedDate);
                    m.Save(table);
                    LabelAlarm.Text += "Individual-individual link created<br />";
                }
                if (CheckBoxICoL.Checked)
                {
                    var alias = TextIndAlias.Text == string.Empty ? TextIndName.Text : TextIndAlias.Text; 
                    if (CheckBoxDateEnds.Checked)
                        m.CreateOrUpdate(_investigationId, TextCoName.Text, TextIndName.Text,alias, _infoSourceId,
                                         doc[0].document_id, TextLinkType.Text, Calendar1.SelectedDate, TextPosition.Text,Calendar2.SelectedDate);
                    else
                        m.CreateOrUpdate(_investigationId, TextCoName.Text, TextIndName.Text,alias, _infoSourceId,
                                     doc[0].document_id, TextLinkType.Text, Calendar1.SelectedDate, TextPosition.Text);
                    LabelAlarm.Text += "Individual-Company link created<br />";
                }
                if (CheckBoxCoCoL.Checked)
                {
                    var title = TextPosition.Text == string.Empty ? TextLinkType.Text : TextPosition.Text;
                    var table = m.GetCoCoLinks(TextCoName.Text);
                    var row = table.FindBycompany_namecompany_name2supporting_doc_idlink_type(TextCoName.Text,
                                                                                              TextCoName2.Text,
                                                                                              doc[0].document_id,
                                                                                              TextLinkType.Text);
                    if (row == null)
                        if (CheckBoxDateEnds.Checked)
                            row = table.Addcompany_company_linkRow(TextCoName.Text, TextCoName2.Text, _infoSourceId
                                                                   , doc[0].document_id, TextLinkType.Text,
                                                                   title, Calendar1.SelectedDate,
                                                                   Calendar2.SelectedDate);
                        else
                            row = table.Addcompany_company_linkRow(TextCoName.Text, TextCoName2.Text, _infoSourceId
                                                                   , doc[0].document_id, TextLinkType.Text,
                                                                   title, Calendar1.SelectedDate,
                                                                   Calendar1.SelectedDate);
                    else
                    {
                        row.link_date = Calendar1.SelectedDate;
                        row.link_date_end = CheckBoxDateEnds.Checked ? Calendar2.SelectedDate : Calendar1.SelectedDate;
                        row.link_title = title;
                    }
                    m.Save(table);
                }
            }
            catch(Exception e)
            {
                LabelAlarm.Text = e.Message;
            }
        }
        public void CreateLinksSrc10Owner()
        {
            var parser = new Source10OwnershipParser(this.TextURL.Text, _investigationId);
            var log=parser.ParseOwnership(false);
            LabelGen10.Text = log.Replace("\r\n", "<br />\r\n"); 
        }
        protected void ButtonSrc70csv_Click(object sender, EventArgs e)
        {
            var dal1 = new CompanyManager();
            var companies = dal1.GetCompanyIDs(70, this.TextBox1.Text);
            if (companies.Count==0)
            {
                LabelGen70.Text = "The symbol '" + TextBox1.Text +"' does not belong to any known companies";
                LabelAlarm.Text = LabelGen70.Text;
                return;
            }
            if(TextBox2.Text.ToLower().Trim()!="daily" && TextBox2.Text.ToLower().Trim()!="weekly")
            {   LabelAlarm.Text = "Period needs to be either daily or weekly";
                return;}
            var period = TextBox2.Text.ToLower().Trim()=="daily" ? 1 : 7;
            var companyName = companies[0].company_name;
            var parser = new Source70SharesTradeParser(this.TextURL.Text, _investigationId,companyName,period );
            LabelGen70.Text= parser.CreateSharesData().Replace("\r\n","<br />\r\n");
        }
        protected void ButtonSrc10Litigation_Click(object sender, EventArgs e)
        {

        }
        #endregion
    }
}