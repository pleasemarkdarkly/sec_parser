using System;
using System.Web.UI.WebControls;
using SECCrawler.DAL;
using System.Data;
using System.Collections.Generic;
using System.Web;
namespace SECCrawler.Controller.FormsBrowser
{
    public partial class FormsList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                FillFormTypes();
        }
        private void FillFormTypes()
        {
            var dal = new SECFormsManager();
            var table = dal.GetFormTypes();
            dropFormTypes.Items.Add("(all)");
            var li = new ListItem("(all 10-Qs)","10%q%");
            dropFormTypes.Items.Add(li);
            li = new ListItem("(all 10-Ks)", "10%k%");
            dropFormTypes.Items.Add(li);
            li = new ListItem("(all 8-Ks)", "8%k%");
            dropFormTypes.Items.Add(li);
            foreach (var rowFt in table) dropFormTypes.Items.Add(rowFt.formtype);
        }
        protected void btnProcess_Click(object sender, EventArgs e)
        {
            LabelStatus.Text = string.Empty;
            var criteria = this.textCriteria.Text.Trim();
            if (criteria == string.Empty)
            {
                LabelStatus.Text = "Enter search criteria before searching";
                return;
            }
            try
            {
                if (TextFormTypes.Text.Trim() == string.Empty) TextFormTypes.Text = "%";
                var dal = new SECFormsManager();
                var table = dal.GetFormsByFullTextSearchAndFormTypeWithSnippet(criteria, TextFormTypes.Text,
                       LinkResponseType.ClickableWithModifiedLink,"./ViewEditForm.aspx?formId=",TextBoxCompanyName.Text + "%");
                GridView2.AutoGenerateColumns = false;
                var v = new DataView(table, "", this.DropSortBy.SelectedValue, DataViewRowState.CurrentRows);
                GridView2.DataSource = v;
                GridView2.DataBind();
                if (GridView2.Columns.Count == 9)
                {
                    GridView2.Columns.RemoveAt(8);
                    GridView2.Columns.RemoveAt(7);
                    GridView2.Columns.RemoveAt(6);
                    GridView2.Columns.RemoveAt(1);

                }
                this.LabelStatus.Text = "Records found : " + table.Rows.Count;
            }
            catch (Exception ex)
            {
                this.LabelStatus.Text = ex.Message;
            }
        }
        protected void btnProcesXLS_Click(object sender, EventArgs e)
        {
            var criteria = this.textCriteria.Text.Trim();
            if (criteria == string.Empty)
            {
                LabelStatus.Text = "Enter search criteria before searching";
                return;
            }
            if (TextFormTypes.Text.Trim() == string.Empty) TextFormTypes.Text = "%";
            var dal = new SECFormsManager();
            var table = dal.GetFormsByFullTextSearchAndFormTypeWithSnippet(criteria, dropFormTypes.SelectedItem.Text,LinkResponseType.NotChanged,"./ViewEditForm?FormID=",TextBoxCompanyName.Text+'%');
            var dtst = new DataSet();
                dtst.Tables.Add(table);
            table.TableName = "Search_Results";
            Response.ContentType ="text/xml";
            Response.ClearContent();
            Response.AppendHeader("Content-disposition", "Attachment; filename=SECfilingsSearchResult.xls");
            //attachments
            var summary = new List<string>
              {
                  "Searched for:" + HttpUtility.HtmlEncode(this.textCriteria.Text),
                  "Documents found:" + table.Rows.Count.ToString(),
                  "Search Date:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm")
              };
            COI.Util.ExcelEngine.Convert(dtst, Response.OutputStream, 2, 2, summary);
            Response.End();
        }
        protected void dropFormTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            try {
                TextFormTypes.Text = dropFormTypes.SelectedValue; }
            catch {
                TextFormTypes.Text = string.Empty; }
        }
    }
}
