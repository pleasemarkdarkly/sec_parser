using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Invest : System.Web.UI.Page
{
    public string iFrameContent = string.Empty;
    public int current = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        try { current = int.Parse(Request.QueryString["current"]); }
        catch { current = 0; }
        int move;
        try { move = int.Parse(Request.QueryString["move"]); }
        catch { move = 0; }
        current = current + move;
        if (current < 0) current = 0;
        if (current > 51) current = 51;
        iFrameContent = string.Format("./slides/investment/showslide.aspx?content={0}.png", current.ToString("00"));
        HyperLink1.NavigateUrl = string.Format("./invest.aspx?current={0}&move=-1", current);
        HyperLink2.NavigateUrl = string.Format("./invest.aspx?current={0}&move=1", current);
    }
}
