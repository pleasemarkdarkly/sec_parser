using System;
using SECCrawler.BLL;
namespace SECCrawler.Controller.FormsBrowser
{
    public partial class FetchDocumentFromDB : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.ClearContent();
                Response.ClearHeaders();
                var formID = new Guid(Request.QueryString["formID"]);
                var seq = int.Parse(Request.QueryString["Seq"]);
                var manager = new SECFormsDocAnalyzer();
                string fileName;
                string type;
                Response.Write(manager.GetFormSubDoc(formID,seq,out fileName,out type));
                if (fileName == string.Empty) fileName = "form.txt";
                Response.StatusCode = 200;
                Response.ContentType = "text/plain";
                if (fileName.ToLower().Trim().EndsWith(".html") || fileName.ToLower().Trim().EndsWith(".htm"))
                    Response.ContentType = "text/html";
                Response.AppendHeader("Content-disposition", "Inline; filename=" + fileName);
            }
            catch (Exception ex)
            {
                Response.ClearContent();
                Response.StatusCode = 400;
                Response.Write("No document or invalid document requested\r\n\r\n" + ex.ToString());
            }
        }
    }
}
