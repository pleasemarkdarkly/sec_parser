using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace COI.WebUI.Charts
{
    public partial class CompanyTimeLine2 : System.Web.UI.Page
    {
        private string _coName;
        private void DoChart()
        {
            var ct = new ChartTools(_coName, Chart1,70,1);
            ct.Resize(DropSize.SelectedValue,0.75D);
            ct.Reset(_coName + " timeline");
            ct.DoSharesVolume();
            ct.DoEvents();
        }
        #region events
        protected void Page_Load(object sender, EventArgs e)
        {
            _coName = Request.QueryString["coName"];
            LabelCoName.Text = _coName;
            if (_coName != string.Empty)
                DoChart();
        }
        protected void ButtonData_Click(object sender, EventArgs e)
        {
            Response.Redirect("./CompanyView.aspx?coName=" + HttpUtility.UrlEncode(_coName), true);
        }
        #endregion
    }
}
