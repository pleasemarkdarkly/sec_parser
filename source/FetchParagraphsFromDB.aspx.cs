using System;
using System.Collections.Generic;
using SECCrawler.BLL;
using System.Web.UI.WebControls;
using System.Web;
namespace SECCrawler.Controller.FormsBrowser
{
    public partial class FetchParagraphsFromDB : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RenderRows( GetRows());
        }
        private void RenderRows(IEnumerable<string> paragraphs)
        {
            var i = 0;
            foreach (var par in paragraphs)
            {
                i++;
                var par1 = par;
                if (par1.Contains("<S>") & !par1.Contains("</S>")) par1 = par1.Replace("<S>", "");
                if (par1.Contains("<s>") & !par1.Contains("</s>")) par1 = par1.Replace("<s>", "");
                par1 = HttpUtility.HtmlEncode(par1).Replace("\r\n", "<br/>");
                var row1 = new TableRow();
                var cell1 = new TableCell
                {
                    Text = i.ToString(),
                    VerticalAlign = VerticalAlign.Top
                };
                var cell2 = new TableCell
                {
                    Text = par1,
                    BorderWidth = 1,
                    BorderStyle = BorderStyle.Solid,
                    BorderColor = System.Drawing.Color.Black
                };
                row1.Cells.Add(cell1);
                row1.Cells.Add(cell2);
                Table1.Rows.Add(row1);
            }
        }
        private List<string >GetRows()
        {
            try
            {
                var formID = new Guid(Request.QueryString["formID"]);
                var seq = int.Parse(Request.QueryString["Seq"]);
                var manager = new SECFormsDocAnalyzer();
                if (Request.QueryString["proximity"]=="on")
                {
                    var term1 = Request.QueryString["proximityTerm1"];
                    var term2 = Request.QueryString["proximityTerm2"];
                    var proximityRange = int.Parse(Request.QueryString["proximityRange"]);
                    var additionalRange = int.Parse(Request.QueryString["additionalRange"]);
                    var paragraphs1 = manager.GetFormSubDocParagraphsWithProximity(formID, seq, proximityRange, term1, term2, additionalRange, true);
                    return paragraphs1;
                }
                if (Request.QueryString["search"] == "on")
                {
                    var term1 = Request.QueryString["proximityTerm1"];
                    var additionalRange = int.Parse(Request.QueryString["additionalRange"]);
                    var paragraphs1 = manager.GetFormSubDocParagraphsWithSearch(formID, seq,  term1, additionalRange,true);
                    return paragraphs1;
                } var paragraphs = manager.GetFormSubDocParagraphs(formID, seq);
                return paragraphs;
            }
            catch (Exception ex)
            {   var paragraphs = new List<string>{"No document or invalid parameters", ex.Message, ex.ToString()};
                return paragraphs;}
        }
    }
}
