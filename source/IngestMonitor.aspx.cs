using System;
using SECCrawler.DAL;

namespace SECCrawler.Controller.Ingest
{
    public partial class IngestMonitor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var manager = new CoiStatisicsManager();
            var table = manager.GetFormIngestQueue();
            this.GridView1.DataSource = table;
            this.GridView1.DataBind();
            if (this.GridView1.Columns.Count == 2)
            {
                this.GridView1.Columns[0].HeaderText = "Count";
                this.GridView1.Columns[1].HeaderText = "Description";
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }
    }
}
