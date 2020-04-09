using System;

namespace COI.WebUI.Diagrams
{
    public partial class alert : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Literal1.Text = Request.QueryString["a"];
            }catch{}
        }
    }
}
