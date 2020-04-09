using System;
using SECCrawler.DAL;
using SECCrawler.BLL;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Generic;
using COI.Util;
using System.IO;
using System.Web;
namespace SECCrawler.Controller.FormsBrowser
{
    public partial class MultiFormParagraphSearch : System.Web.UI.Page
    {
        #region declares
        private const string SearchType = "MPS";
        private const string AutoSaveSearchType = "ASMPS";
        protected List<string> Companies;
        protected List<DateTime> MinDates;
        protected List<DateTime> MaxDates;
        protected List<List<string>> KeywordsPerCo;
        private bool _alternateRowBackground = false;
        #endregion
        #region loadSave
        private void SaveSearch(string description,string searchType)
        {
            var dal = new SavedSearchManager();
            var table = dal.GetSearches(searchType);
            var srchRow = table.FindBydescriptiontype(description, SearchType);
            if (null == srchRow)
                table.AddtblCOI_savedSearchRow(description, searchType,
                    TextSearchForms.Text, TextSearchPars.Text, DropSort.SelectedValue, TextFormTypes.Text, null, null,DateTime.Now,
                    null,null,CheckCompany.Checked,false);
            else
            {
                srchRow.param1 = TextSearchForms.Text;
                srchRow.param2 = TextSearchPars.Text;
                srchRow.param3 = DropSort.SelectedValue;
                srchRow.param4 = TextFormTypes.Text;
                srchRow.optional5 = CheckCompany.Checked.ToString();
                srchRow.optional6 = TextBoxCompany.Text;
            }
            dal.Save(table);
            FillFormTypesAndSavedSearches();
        }
        private void LoadSearch(string description,string searchType)
        {
            var dal = new SavedSearchManager();
            var table = dal.GetSearches(searchType);
            var row = table.FindBydescriptiontype(description, searchType);
            if (row == null) return;
            TextSearchForms.Text = row.param1;
            TextSearchPars.Text = row.param2;
            DropSort.SelectedValue = row.param3;
            TextFormTypes.Text = row.param4 == string.Empty ? "%" : row.param4;
            try
            {
                CheckCompany.Checked = !row.Isoptional5Null() ? (row.optional5.ToLower() == "true") : false;
            }
            catch
            {
                CheckCompany.Checked = false;
            }
            TextBoxCompany.Text = !row.Isoptional6Null() ? row.optional6 : string.Empty;
            txtSaveName.Text = description;
        }
        #endregion
        #region events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                FillFormTypesAndSavedSearches();
        }
        protected void BtnSave_Click(object sender, EventArgs e)
        {
            var description = this.txtSaveName.Text;
            SaveSearch(description,SearchType);
        }
        protected void BtnLoad_Click(object sender, EventArgs e)
        {
            if (DropSavedSearches.SelectedIndex == -1) return;
            var description = DropSavedSearches.SelectedValue;
            LoadSearch(description,SearchType);
        }
        protected void BtnLoad2_Click(object sender, EventArgs e)
        {
            if (DropSavedSearches2.SelectedIndex == -1) return;
            var description = DropSavedSearches2.SelectedValue;
            LoadSearch(description, AutoSaveSearchType);
        }
        protected void Page_PreRender(object sender, EventArgs e)
        {
            Panel1.Visible = CheckBoxControlsVisible.Checked;
        }
        protected void CheckBoxControlsVisible_CheckedChanged(object sender, EventArgs e)
        {

        }
        protected void ButtonSearchPar_Click(object sender, EventArgs e)
        {
            if (TextSearchForms.Text.Trim()==string.Empty || TextSearchPars.Text.Trim()==string.Empty || TextFormsList.Text.Trim()==string.Empty)
            {
                Literal1.Text = "<span class=alarm>Please enter search parameters and make sure the first search returns at least 1 filing</span>";
                return;
            }
            if (RadioOutFormat.SelectedValue=="Excel")
            {
                ParSearchToXLS();
                return;
            }
            var gennerateControls = RadioOutFormat.SelectedValue == "Event";
            var d = new DtstFilingSearchResult();
            if (DropSort.SelectedValue == "2")
            {
                var sort = d.MPSresults.DateColumn.ColumnName + " asc, "
                              + d.MPSresults.CompanyColumn.ColumnName + " asc, "
                              + d.MPSresults.rowIDColumn + " asc";
                ParSearchToHTMLBySortOrder(sort, false, gennerateControls);
            }
            else
            {
                var sort = d.MPSresults.CompanyColumn.ColumnName + " asc, "
                              + d.MPSresults.DateColumn.ColumnName + " asc, "
                              + d.MPSresults.rowIDColumn + " asc";
                ParSearchToHTMLBySortOrder(sort, true, gennerateControls);
            }
        }
        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            if (TextSearchForms.Text.Trim()==string.Empty)
            {
                Literal1.Text = "<span class=alarm>Please enter search parameters</span>";
                return;
            }
            try
            {
                var manager = new SECFormsManager();
                if (TextFormTypes.Text.Trim() == string.Empty) TextFormTypes.Text = "%" ;
                var table = manager.GetFormsByFullTextSearchAndFormType(TextSearchForms.Text, TextFormTypes.Text,LinkResponseType.NotChanged, string.Empty);
                TextFormsList.Text = string.Empty;
                var vw = new DataView(table, "", "companyname asc, formdate asc", DataViewRowState.CurrentRows);
                var n = TextBoxCompany.Text.ToLower().Trim();
                foreach (DataRowView rowV in vw)
                {
                    var row = (secCrawlerData.tblSEC_FormsRow)rowV.Row;
                    if (!CheckCompany.Checked || row.CompanyName.ToLower().Contains(n))
                    TextFormsList.Text += row.FormID.ToString() + "\r\n";
                }
            }
            catch (Exception ex)
            {
                Literal1.Text = "<span class=alarm>" + ex.Message+ "</span>";
                TextFormsList.Text = string.Empty;
            }
        }
        protected void dropFormTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try{
                TextFormTypes.Text = dropFormTypes.SelectedValue;}
            catch
            {
                TextFormTypes.Text = string.Empty;}
        }
        #endregion
        #region DbIO
        private void FillFormTypesAndSavedSearches()
        {
            dropFormTypes.Items.Clear();
            DropSavedSearches.Items.Clear();
            var dal = new SECFormsManager();
            var dal2 = new SavedSearchManager();
            var table = dal.GetFormTypes();
            dropFormTypes.Items.Add("(all)");
            foreach (var rowFt in table)dropFormTypes.Items.Add(rowFt.formtype);
            var table2 = dal2.GetSearches(SearchType);
            DropSavedSearches.DataSource = table2;
            DropSavedSearches.DataTextField = table2.descriptionColumn.ColumnName;
            DropSavedSearches.DataValueField = table2.descriptionColumn.ColumnName;
            DropSavedSearches.DataBind();
            var table3 = dal2.GetSearches(AutoSaveSearchType);
            foreach (var searchRow in table3)searchRow.description = searchRow.description.ToLower();
            DropSavedSearches2.DataSource = table3;
            DropSavedSearches2.DataTextField = table3.descriptionColumn.ColumnName;
            DropSavedSearches2.DataValueField = table3.descriptionColumn.ColumnName;
            DropSavedSearches2.DataBind();
        }
        private DtstFilingSearchResult GetDtst()
        {
            Companies = new List<string>();
            MinDates = new List<DateTime>();
            MaxDates = new List<DateTime>();
            KeywordsPerCo = new List<List<string>>();
            var manager = new SECFormsManager();
            var analyzer = new SECFormsDocAnalyzer();
            var addPars = int.Parse(DropProximityAdd.SelectedValue);
            var keywords = this.TextSearchPars.Text.Replace("\r", "").Split('\n');
            var dtst = new DtstFilingSearchResult();
            var table = dtst.MPSresults;
            var i = 0;
            foreach (var line in this.TextFormsList.Text.Replace("\r\n", "\n").Split('\n'))
            {
                try
                {
                    if (string.IsNullOrEmpty(line)) continue;
                    var formID = new Guid(line);
                    var form = manager.GetFormsByFormID(formID)[0];
                    var companyIndex = Companies.IndexOf(form.CompanyName);
                    if (companyIndex == -1)
                    {
                        Companies.Add(form.CompanyName);
                        MinDates.Add(form.FormDate);
                        MaxDates.Add(form.FormDate);
                        KeywordsPerCo.Add(new List<string>());
                        companyIndex = Companies.IndexOf(form.CompanyName);
                    }
                    else //keep min and max date per company
                    {
                        if (form.FormDate < MinDates[companyIndex])
                            MinDates[companyIndex] = form.FormDate;
                        if (form.FormDate > MaxDates[companyIndex])
                            MaxDates[companyIndex] = form.FormDate;
                    }
                    var keywordsThisCo = KeywordsPerCo[companyIndex];
                    foreach (var keyword in keywords)
                    {
                        if (keyword.Trim()==string.Empty) continue;
                        var pars = analyzer.GetFormSubDocParagraphsWithSearch(
                            formID, -1, keyword, addPars, CheckMultiple.Checked);
                        if (pars.Count == 0) continue;
                        i++;
                        if (keywordsThisCo.IndexOf(keyword) == -1) keywordsThisCo.Add(keyword);
                        foreach (var par in pars)
                            if (par.Trim()!=string.Empty)
                            table.AddMPSresultsRow(i, form.FormDate, form.CompanyName,
                                form.FormType, keyword, par, form.FormPartialURL,formID);
                    }
                }
                catch (Exception ex)
                {
                    Literal1.Text = ex.ToString();
                }
            }
            return dtst;
        }
        #endregion
        #region FormatedOutputAuxiliar
        private const string TableHead = "<table border=0 cellpadding=0 style=\"width:100%;\"><tr class=inverse><td>Date </td><td>Company </td><td>Paragraphs</td></tr>";
        private const string TableHeadSimple = "<table border=0 cellpadding=0 style=\"width:100%;\"><tr class=inverse ><td colspan=3>Search Results</td>";
        // style="width:100%;" cellpadding="0" cellspacing="0"
        private const string TableFoot = "</table>";
        private Boolean _lightgray = true;
        private static string GenCell(int columns)
        {
            return string.Format("<td valign=\"top\" colspan=\"{0}\">&nbsp;</td>",columns);
        }
        private static string GenCell(string cellData)
        {
            return string.Format("<td valign=\"top\">{0}</td>", cellData);
        }
        private static string GenCell(string cellData,string cssClass)
        {
            return string.Format("<td valign=\"top\" class=\"{1}\">{0}</td>", cellData,cssClass);
        }
        private string GenRow(string cells)
        {
            var lightgray = false;
            if (_alternateRowBackground)
            {
                _lightgray = !_lightgray;
                lightgray = _lightgray;
            }
            return !lightgray ? string.Format("<tr>{0}</tr>", cells) : string.Format("<tr class=\"lightGray\">{0}</tr>", cells);
        }
        private static string GenRowInverse(string cells)
        {
            return string.Format("<tr class=\"inverse\">{0}</tr>", cells);
        }
        #endregion
        #region FormatedOutput
        private void ParSearchToHTMLBySortOrder(string sort, bool usingCompanyHeaders, bool generateControls)
        {
            var description = DateTime.Now.ToString("yyyy-MM-dd HH:mm") +" | " + TextSearchForms.Text.Replace("\r\n"," ");
            if (description.Length > 100) description = description.Substring(0, 100);
            try{SaveSearch(description, AutoSaveSearchType);}
            catch (Exception e){e.ToString(); }
            var lastCo = string.Empty;
            var paragraphResultsCount = 0;
            var dtst = GetDtst();
            var vw = new DataView(dtst.MPSresults, "", sort, DataViewRowState.CurrentRows);
            if (generateControls) Session["dtst"] = dtst;
            var table2 = new StringWriter();
            var table0 = new StringWriter();
            CheckBoxControlsVisible.Checked = false;
            //data
            table2.WriteLine(TableHead);
            _alternateRowBackground = true;
            foreach (DataRowView srchResV in vw)
            {
                var srchRes = (DtstFilingSearchResult.MPSresultsRow)srchResV.Row;
                if (srchRes.Paragraph.Trim() == string.Empty) srchRes.Paragraph = "-";
                if (usingCompanyHeaders && srchRes.Company!=lastCo)
                {
                    lastCo = srchRes.Company;
                    var cell ="Company: " + lastCo + "<a name=\"C" + Companies.IndexOf(lastCo) + "\">";
                    table2.WriteLine(GenRowInverse(GenCell(2)+ GenCell(cell)));
                }
                var tr = string.Empty;
                tr += GenCell(srchRes.Date.ToString("yyyy MMM-dd"));
                var c0 = srchRes.Company;
                if (Companies.IndexOf(c0) > -1 && !usingCompanyHeaders)
                    c0 += "<a name=\"C" + Companies.IndexOf(c0).ToString() + "\">";
                tr += GenCell(c0);
                string p1;
                try{
                    p1 = srchRes.Paragraph.Replace(
                        srchRes.Keyword, "<b><u>" + srchRes.Keyword + "</b></u>");
                }
                catch {
                    p1 = srchRes.Paragraph;
                }
                if (p1.Contains("<S>") & !p1.Contains("</S>")) p1 = p1.Replace("<S>", "");
                if (p1.Contains("<s>") & !p1.Contains("</s>")) p1 = p1.Replace("<s>", "");
                tr += GenCell("<b>Company: " + srchRes.Company
                                            + " | Date: " + srchRes.Date.ToString("yyyy-MM-dd")
                                            + " | Type: " + srchRes.Form
                                            + " | Keyword: " + srchRes.Keyword + "</b><br/>" + p1);
                paragraphResultsCount++;
                tr = GenRow(tr);
                table2.WriteLine(tr);
                if (!generateControls) continue;
                _lightgray = !_lightgray;
                table2.WriteLine(GenRow(GenCell(2)+
                                        GenCell("<a href=\"./SaveSnippetAsEvent.aspx?resultId=" +
                                                srchRes.rowID + "\" target=\"_blank\">Save this snippet as event</a> - "
                                                + "<a href=\"ViewEditForm.aspx?formID="
                                                + srchRes.FormID.ToString()
                                                + "\" target=\"_blank\">View Form</a>")));
            }
            table2.WriteLine(TableFoot);
            //header(statistics)
            table0.WriteLine(TableHeadSimple);
            _alternateRowBackground = false;
            table0.WriteLine(GenRow(GenCell("Form search criteria: ","bold")+GenCell(1) + GenCell(TextSearchForms.Text)));
            table0.WriteLine(GenRow(GenCell("Paragraph search criteria: ", "bold") + GenCell(1) + GenCell(TextSearchPars.Text.Replace("\r\n", " OR "))));
            var index = new SortedList<DateTime, string>();
            foreach (var c in Companies)
            {
                var i = Companies.IndexOf(c);
                var s = MinDates[i].ToString("MMM/yyyy") + " .. "
                        + MaxDates[i].ToString("MMM/yyyy")
                        + " <a href='#C" + i + "'>" + c + "</a> &nbsp;(";
                foreach (var k in KeywordsPerCo[i])
                {
                    s += k;
                    if (KeywordsPerCo[i].IndexOf(k) < KeywordsPerCo[i].Count - 1)
                        s += ", ";
                }
                s += ")<br /> ";
                index.Add(MinDates[i], s);
            }
            table0.WriteLine(GenRow(GenCell("Companies: ", "bold") + GenCell(1)));
            foreach (var s in index.Values)
            {
                table0.WriteLine(GenRow(GenCell(2) + GenCell(s)));
            }
            var stat = new CoiStatisicsManager().GetCrawlerStatistics();
            table0.WriteLine(GenRow(GenCell("Result set statistics", "bold") + GenCell(2)));
            table0.WriteLine(GenRow(GenCell(1) + GenCell(paragraphResultsCount.ToString("#,###"), "bodyRight") + GenCell("paragraphs")));
            table0.WriteLine(GenRow(GenCell(1) + GenCell(Companies.Count.ToString("#,###"), "bodyRight") + GenCell("companies")));
            table0.WriteLine(GenRow(GenCell("Database statistics", "bold") + GenCell(2)));
            foreach (var row in stat)
            {
                table0.WriteLine(GenRow(GenCell(1) + GenCell(row.Count.ToString("#,###"), "bodyRight") + GenCell(row.Description)));
            }
            table0.WriteLine(TableFoot);
            //output
            Literal1.Text =table0.ToString() + table2.ToString();

        }
        #endregion
        #region ExcelOutput
        private void ParSearchToXLS()
        {
            var dtst = GetDtst();
            var summary = new List<string> { "" };
            Response.ContentType = "application/ms-excel";
            Response.ClearContent();
            Response.AppendHeader("Content-disposition", "Attachment; filename=MultipleFilingSearchResult.xml");
            ExcelEngine.Convert(dtst, Response.OutputStream, 0, 0, summary);
            Response.End();
        }
        #endregion
    }
}
