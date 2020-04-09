 using System;
using COI.DAL;

namespace COI.WebUI.Investigator
{
    public partial class BatchProcess : System.Web.UI.Page
    {
        protected static BatchManager.BatchNamesEnum OTCBB1 = BatchManager.BatchNamesEnum.OTCBBSystemChange;
        protected static string OTCBB1Name = BatchManager.BatchNames[(int) OTCBB1];
        public string FrameLink = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            var dal = new BatchManager();
            var table = dal.GetLatestData();
            foreach (var batchRun in table)
            {
                if (batchRun.batch_name == OTCBB1Name)
                    LiteralLastOTCBB.Text = batchRun.run_date.ToString("yyyy-MM-dd HH:mm");
                else if (batchRun.batch_name==BatchManager.BatchNames[(int)BatchManager.BatchNamesEnum.SECOwnership])
                    LiteralLastSEC1.Text = batchRun.run_date.ToString("yyyy-MM-dd HH:mm");
                else if (batchRun.batch_name==BatchManager.BatchNames[(int)BatchManager.BatchNamesEnum.NasdaqHistoricAllSymbols])
                    Label2.Text = batchRun.run_date.ToString("yyyy-MM-dd HH:mm");
                else if (batchRun.batch_name == BatchManager.BatchNames[(int)BatchManager.BatchNamesEnum.NameChangesFromSECCrawler])
                    Label3.Text = batchRun.run_date.ToString("yyyy-MM-dd HH:mm");
                else if (batchRun.batch_name == BatchManager.BatchNames[(int)BatchManager.BatchNamesEnum.SECImportCIK])
                    Label4.Text = batchRun.run_date.ToString("yyyy-MM-dd HH:mm");
                else if (batchRun.batch_name == BatchManager.BatchNames[(int)BatchManager.BatchNamesEnum.SECLookupIndividuals])
                    Label5.Text = batchRun.run_date.ToString("yyyy-MM-dd HH:mm");
                else if (batchRun.batch_name == BatchManager.BatchNames[(int)BatchManager.BatchNamesEnum.ParseHistoricShareValuesFrom10K])
                    Label6.Text = batchRun.run_date.ToString("yyyy-MM-dd HH:mm");
                else if (batchRun.batch_name == BatchManager.BatchNames[(int)BatchManager.BatchNamesEnum.ParseLitigations])
                    Label7.Text = batchRun.run_date.ToString("yyyy-MM-dd HH:mm");
            }
        }
        protected void ButtonOTCBB1_Click(object sender, EventArgs e)
        {FrameLink = "./Batch/SystemChanges.aspx";}
        protected void ButtonSECOwnership_Click(object sender, EventArgs e)
        {FrameLink = "./Batch/SECOwnershipParse.aspx";}
        protected void ButtonOutput_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {try
            {
                FrameLink = "./Batch/showOutput.aspx?batchTypeId=" + e.CommandArgument.ToString();
            }
            catch (Exception ex)
            {
                LabelOutput.Text = ex.ToString();
            }
        }
        protected void ButtonDoBatch(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {if (e.CommandName != "DoBatch") return;
            const string querystring = "batchType={0}";
            FrameLink = "./Batch/DoBatchByID.aspx?" + string.Format(querystring, e.CommandArgument);
        }

    }
}
