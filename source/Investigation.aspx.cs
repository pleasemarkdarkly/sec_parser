using System;
using System.Collections.Generic;
using COI.DAL;
using COI.Util;
using COI.Util.Web;
using System.Data;
namespace COI.WebUI.Investigations
{
    public partial class Investigation : System.Web.UI.Page
    {
        private readonly InvestigationManager _dal = new InvestigationManager();
        #region UI-events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                _investigationId = int.Parse(DropInvestigations.SelectedValue);
                return;
            }
            PopulateInvestigations();
            PopulateAllIndividualNames();
            PopulateAllCompanyNames();
            Panel1.Visible = false;
        }
        protected void ButtonLoad_Click(object sender, EventArgs e)
        {
            try
            {
                _investigationId = int.Parse(DropInvestigations.SelectedValue);
                PopulateInvestigatedIndividuals();
                PopulateInvestigatedCompanies();
                LabelInvestigation.Text = DropInvestigations.SelectedItem.Text +
                    string.Format(" ({0})", DropInvestigations.SelectedValue);
                Panel1.Visible = true;
                LinkAddNV.NavigateUrl =
                    "../Investigator/AddLinksFromSite.aspx?SourceID=50&InvestigationID=" + _investigationId;
                LinkAddSEC.NavigateUrl =
                    "../Investigator/AddLinksFromSite.aspx?SourceID=10&InvestigationID=" + _investigationId;
            }
            catch (Exception)
            {

                Panel1.Visible = false;
            } 
        }
        protected void ButtonStarlight_Click(object sender, EventArgs e)
        {
            var engine = new StarlightEngine();
            var xml = engine.GetLinks(_investigationId);
            Response.ClearContent();
            Response.ClearHeaders();
            Response.ContentType = "text/xml";
            Response.AppendHeader("Content-disposition", "Attachment; filename=IndividualCompanyLinks.xml");

            Response.Write(xml);
            Response.End();
        }
        protected void ButtonShowHideIndividuals_Click(object sender, EventArgs e)
        {
            CheckIndividualNames.Visible = !CheckIndividualNames.Visible;
            PanelAddRemoveIndividuals.Visible = CheckIndividualNames.Visible;
            PanelAddMultipleIndividuals.Visible = CheckIndividualNames.Visible;
        }
        protected void ButtonShowHideCompanies_Click(object sender, EventArgs e)
        {
            CheckCompanyNames.Visible = !CheckCompanyNames.Visible;
            PanelAddRemoveCompanies.Visible = CheckCompanyNames.Visible;
            PanelAddMultipleCompanies.Visible = CheckCompanyNames.Visible;
        }
        protected void ButtonShowLinks_Click(object sender, EventArgs e)
        {
            var dal = new LinkManager();
            var table= dal.GetIndividualCompanyLink(_investigationId);
            GridView1.DataSource = table;
            GridView1.DataBind();
            if (CheckIndividualNames.Visible) ButtonShowHideIndividuals_Click(sender, e);
            if (CheckCompanyNames.Visible)ButtonShowHideCompanies_Click(sender,e);
            GridView1.Visible = true;
        }
        protected void ButtonExcel_Click(object sender, EventArgs e)
        {
            var dal = new LinkManager();
            var dtst = new DataSet();
            var table = dal.GetIndividualCompanyLink(_investigationId);
            table.TableName = "Individual_company_links";
            dtst.Tables.Add( table);
            ExcelAttachment.ReplaceResponseWithXLS(dtst,Response,new List<string>(),"individual_company_links" );
        }
        #endregion
        #region investigation
        private int _investigationId;
        protected void ButtonCreateNew_Click(object sender, EventArgs e)
        {
            if (TextCreateNew.Text.Trim()==string.Empty) return;
            var table = _dal.GetInvestigations();
            int maId;
            if (table.Rows.Count == 0)
                maId = 0;
            else
                maId =(int) table.Compute("max(" + table.investigation_idColumn.ColumnName + ")", string.Empty);
            maId++;
            table.AddinvestigationRow(maId, TextCreateNew.Text.Trim());
            _dal.Save(table);
            PopulateInvestigations();
            _investigationId = maId;
            DropInvestigations.SelectedValue = maId.ToString();
            ButtonLoad_Click(sender,e);
        }
        private void PopulateInvestigations()
        {
            DropInvestigations.Items.Clear();
            var table = _dal.GetInvestigations();
            DropInvestigations.DataSource = table;
            DropInvestigations.DataTextField = table.investigation_titleColumn.ColumnName;
            DropInvestigations.DataValueField = table.investigation_idColumn.ColumnName;
            DropInvestigations.DataBind();
        }
        #endregion
        #region individuals
        private void PopulateInvestigatedIndividuals()
        {
            CheckIndividualNames.Items.Clear();
            var table = _dal.GetInvestigatedIndividuals(_investigationId);
            CheckIndividualNames.DataSource = table;
            CheckIndividualNames.DataTextField = table.individual_nameColumn.ColumnName;
            CheckIndividualNames.DataBind();
        }
        protected void ButtonAddInvestigatedIndividual0_Click(object sender, EventArgs e)
        {   var name = NameAnalyzer.NameCapitalizer(DropExistingIndividuals.SelectedValue);
            if (name==string.Empty) return;
            AddNewIndividual(new List<string>{name});
            PopulateInvestigatedIndividuals();
        }
        protected void ButtonAddInvestigatedIndividual_Click(object sender, EventArgs e)
        {   var name = NameAnalyzer.NameCapitalizer(TextNewIndividual.Text.Trim());
            if (name == string.Empty) return;
            AddNewIndividual(new List<string> {name});
            PopulateInvestigatedIndividuals();
        }
        protected void ButtonAddIndividualS_Click(object sender, EventArgs e)
        {
            var names1 = TextNewIndividualS.Text.Replace("\r\n","\n").Split('\n');
            var names = new List<string>();
            foreach (var s in names1)
            {
                var name = NameAnalyzer.NameCapitalizer(s);
                if (name.Length>0) names.Add(name);
            }
            if (names.Count==0) return;
            AddNewIndividual(names);
            PopulateInvestigatedIndividuals();
        }
        protected void AddNewIndividual(List<string> names)
        {   var individualManager = new IndividualManager();
            var tableI = individualManager.GetIndividuals();
            var table = _dal.GetInvestigatedIndividuals(_investigationId);
            foreach (var name in names)
            {   
                if (tableI.FindByindividual_name(name) == null)
                {   tableI.AddindividualRow(name, true);
                    individualManager.Save(tableI);
                }
                var existing = table.FindByindividual_nameinvestigation_id(name, _investigationId);
                if (existing != null) continue;
                table.Addinvestigated_individualRow(name, _investigationId);
                _dal.Save(table);
            }
        }
        protected void ButtonDeleteInvestigatedIndividuals_Click(object sender, EventArgs e)
        {

        }
        private void PopulateAllIndividualNames()
        {
            var individualManager = new IndividualManager();
            var table = individualManager.GetIndividuals();
            DropExistingIndividuals.DataSource = table;
            DropExistingIndividuals.DataTextField = table.individual_nameColumn.ColumnName;
            DropExistingIndividuals.DataValueField = table.individual_nameColumn.ColumnName;
            DropExistingIndividuals.DataBind();
        }
        #endregion
        #region companies
        private void PopulateInvestigatedCompanies()
        {
            CheckCompanyNames.Items.Clear();
            var table = _dal.GetInvestigatedCompanies(_investigationId);
            CheckCompanyNames.DataSource = table;
            CheckCompanyNames.DataTextField = table.company_nameColumn.ColumnName;
            CheckCompanyNames.DataBind();
        }
        private void PopulateAllCompanyNames()
        {
            var dal = new CompanyManager();
            var table = dal.GetCompanies();
            DropExistingICompanies.DataSource = table;
            DropExistingICompanies.DataTextField = table.company_nameColumn.ColumnName;
            DropExistingICompanies.DataValueField = table.company_nameColumn.ColumnName;
            DropExistingICompanies.DataBind();
        }
        protected void ButtonAddInvestigatedCompany_Click(object sender, EventArgs e)
        {   var name = NameAnalyzer.FilterName(TextNewCompany.Text);
            if (name.Length>0) AddNewCompanies(new List<string>{name});
            PopulateInvestigatedCompanies();
        }
        protected void ButtonAddInvestigatedCompany0_Click(object sender, EventArgs e)
        {   AddNewCompanies(new List<string> { DropExistingICompanies.SelectedValue });
            PopulateInvestigatedCompanies();
        }
        protected void ButtonAddCompanieS_Click(object sender, EventArgs e)
        {   var names1 = TextNewCompanieS.Text.Replace("\r\n","\n").Split('\n');
            var names = new List<string>();
            foreach (var s in names1)
            {
                var name = NameAnalyzer.FilterName(s);
                if(name.Length>0)
                    names.Add(name);
            }
            AddNewCompanies(names);
            PopulateInvestigatedCompanies();
        }
        public void AddNewCompanies(List<string> coNames)
        {   var coManager = new CompanyManager();
            var tableCo = coManager.GetCompanies();
            var table = _dal.GetInvestigatedCompanies(_investigationId);
            foreach (var coName in coNames)
                if(tableCo.FindBycompany_name(coName)==null && coName.Length>0)
                    tableCo.AddcompanyRow(coName);
            coManager.Save(tableCo);
            foreach (var coName in coNames)
                if (table.FindBycompany_nameinvestigation_id(coName, _investigationId) == null && coName.Length>0)
                    table.Addinvestigated_companyRow(coName, _investigationId);
            _dal.Save(table);
        }
        protected void ButtonDeleteInvestigatedCompanies_Click(object sender, EventArgs e)
        {

        }
        protected void LinkAddAlLinkedCompanies_Click(object sender, EventArgs e)
        {
            _dal.PopulateInvestigatedCompaniesByIndividuals(_investigationId);
            PopulateInvestigatedCompanies();
        }
        protected void LinkAddAlMultiLinkedCompanies_Click(object sender, EventArgs e)
        {
            _dal.PopulateInvestigatedCompaniesWith2OrMoreIndividualsByInvestigationID(_investigationId);
            PopulateInvestigatedCompanies();
        }
        #endregion
    }
}
