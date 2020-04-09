using System;
using COI.DAL;
using System.Web.UI;
using System.Web.UI.WebControls;
namespace COI.WebUI.Investigator
{
    public partial class ManualFix1 : System.Web.UI.Page
    {
        #region events
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        protected void CheckAll_CheckedChanged(object sender, EventArgs e)
        {foreach (ListItem i in CheckResults.Items)i.Selected = CheckAll.Checked;}
        #endregion
        #region search
        protected void ButtonSrchPerson_Click(object sender, EventArgs e)
        {
            if (TextNamePerson.Text.Trim().Length < 1) return;
            var dal = new IndividualManager();
            var table = dal.SearchByName(this.TextNamePerson.Text);
            CheckAll.Checked = false;
            CheckResults.Items.Clear();
            CheckResults.DataSource = table;
            CheckResults.DataTextField = table.individual_nameColumn.ColumnName;
            CheckResults.DataValueField = table.individual_nameColumn.ColumnName;
            CheckResults.DataBind();
            Panel1.Visible = true;
        }
        protected void ButtonSrchCompany_Click(object sender, EventArgs e)
        {
            if (TextNameCompany.Text.Trim().Length < 2) return;
            var dal = new CompanyManager();
            var table=dal.SearchCompanies(this.TextNameCompany.Text);
            CheckResults.Items.Clear();
            CheckAll.Checked = false;
            CheckResults.DataSource = table;
            CheckResults.DataTextField = table.company_nameColumn.ColumnName;
            CheckResults.DataValueField = table.company_nameColumn.ColumnName;
            CheckResults.DataBind();
            Panel1.Visible = true;
        }
        protected void ButtonSrchAlias_Click(object sender, EventArgs e)
        {
            if (TextAliasPerson.Text.Trim().Length < 2) return;
            var dal = new LinkManager();
            var table = dal.GetIILByAliasSearch(TextAliasPerson.Text);
            CheckResults.Items.Clear();
            CheckAll.Checked = false;
            CheckResults.DataSource = table;
            CheckResults.DataTextField = table.individual_aliasColumn.ColumnName;
            CheckResults.DataValueField = table.individual_aliasColumn.ColumnName;
            CheckResults.DataBind();
            Panel1.Visible = true;
        }
        protected void ButtonUseSearchAsName_Click(object sender, EventArgs e)
        {
            if (CheckResults.SelectedIndex <= -1) return;
            TextNamePerson.Text = CheckResults.SelectedValue;
            Panel1.Visible = false;
        }
        protected void ButtonUseSearchAsAlias_Click(object sender, EventArgs e)
        {
            if (CheckResults.SelectedIndex <= -1) return;
            TextAliasPerson.Text = CheckResults.SelectedValue;
            Panel1.Visible = false;
        }
        protected void ButtonUseSearchAsCompany_Click(object sender, EventArgs e)
        {
            if (CheckResults.SelectedIndex <= -1) return;
            TextNameCompany.Text = CheckResults.SelectedValue;
            Panel1.Visible = false;
        }
        #endregion
        #region convert
        protected void ButtonI2CoAll_Click(object sender, EventArgs e)
        {
            LabelOut.Text = string.Empty;
            var dal = new CompanyManager();
            foreach (ListItem i in CheckResults.Items)
            {
                if (!i.Selected) continue;
                dal.ConvertIndividualToCompany(i.Text,string.Empty);
                LabelOut.Text += "Conversion reported no errors<br />\r\n";
                //CheckResults.Items.Remove(i);
            }
        }
        protected void ButtonI2Co_Click(object sender, EventArgs e)
        {
            try
            {
                if (TextNamePerson.Text.Trim().Length < 2) return;
                var dal = new CompanyManager();
                dal.ConvertIndividualToCompany(TextNamePerson.Text, TextNameCompany.Text);
                LabelOut.Text = "Conversion reported no errors";
            }
            catch( Exception ex)
            {
                LabelOut.Text = ex.Message;
            }
        }
        protected void ButtonP2A_Click(object sender, EventArgs e)
        {
            try
            {
                if (TextNamePerson.Text.Trim().Length < 2) return;
                if (TextAliasPerson.Text.Trim().Length < 2) return;
                var dal = new IndividualManager();
                dal.ConvertIndividualToAlias(TextNamePerson.Text, TextAliasPerson.Text);
                LabelOut.Text = "Conversion reported no errors";
            }
            catch (Exception ex)
            {
                LabelOut.Text = ex.Message;
            }
        }
        #endregion
    }
}
