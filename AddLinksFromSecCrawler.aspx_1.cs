using System;
using System.Collections.Generic;
using COI.DAL;
using COI.Util;

namespace COI.WebUI.Investigator
{
    public partial class AddLinksFromSecCrawler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            var dal = new InvestigationManager();
            var table = dal.GetInvestigations();
            DropInvestigations.DataSource = table;
            DropInvestigations.DataTextField = table.investigation_titleColumn.ColumnName;
            DropInvestigations.DataValueField = table.investigation_idColumn.ColumnName;
            DropInvestigations.DataBind();
        }

        protected void ButtonSerachMentions_Click(object sender, EventArgs e)
        {
            if (TextBoxPrimaryName.Text.Trim()==string.Empty)
            {
                LabelAlarm.Text = "Please specify at least the primary name";
                return;
            }
            TextBoxPrimaryName.Text = NameAnalyzer.NameCapitalizer(TextBoxPrimaryName.Text);
            if (TextBoxAliases.Text.Trim() == string.Empty) TextBoxAliases.Text = TextBoxPrimaryName.Text;
            var aliases= TextBoxAliases.Text.Replace("\r\n", "\n").Split('\n');
            var aliasesList = new List<string> ();
            foreach (var s in aliases)
            {
                if (!aliasesList.Contains(s))
                    aliasesList.Add(s);
            }
            foreach (var s in aliasesList)
            {
                try
                {
                    var dal = new IndividualManager();
                    dal.ImportIndividualFromSrc10(TextBoxPrimaryName.Text, s);
                }
                catch(Exception ex)
                {
                    LabelAlarm.Text += ex.Message + "<br />";
                    return;
                }
                
            }
            if (CheckUseInvestigation.Checked)
            {
                var dali = new InvestigationManager();
                var invID = int.Parse(DropInvestigations.SelectedValue);
                var table=dali.GetInvestigatedIndividuals(invID);
                var row = table.FindByindividual_nameinvestigation_id(TextBoxPrimaryName.Text, invID);
                if (row == null)
                    table.Addinvestigated_individualRow(TextBoxPrimaryName.Text, invID);
                dali.Save(table);
            }
            LabelAlarm.Text = "processed with no errors";
        }

        protected void ButtonFormerNames_Click(object sender, EventArgs e)
        {

        }
    }
}
