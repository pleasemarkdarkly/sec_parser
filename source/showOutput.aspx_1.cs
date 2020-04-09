using System;
using System.Linq;
using COI.DAL;

namespace COI.WebUI.Investigator.Batch
{
    public partial class showOutput : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.AllKeys.Contains("batchTypeId"))
            {
                try
                {
                    var dal = new BatchManager();
                    var name = BatchManager.BatchNames[int.Parse(Request.QueryString["batchTypeId"])];
                    var table = dal.GetLatestData(name);
                    Literal1.Text = table[0].output_text.Replace("\r\n","<br />\r\n");
                }
                catch (Exception ex)
                {
                    Literal1.Text = ex.ToString();
                }
            }
        }
    }
}
