using System;

namespace SECCrawler.Controller
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.Label1.Text = Request.QueryString["msg"];
            }
            catch {
            }
            Label1.Visible = (Label1.Text != string.Empty);
        }
    }
}
