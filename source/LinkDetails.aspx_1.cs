using System;
using System.IO;
using COI.BLL;
using COI.DAL;

namespace COI.WebUI.Diagrams
{
    public partial class LinkDetails : System.Web.UI.Page
    {
        public string Subject;
        public string Child;
        private LinkTree.SubjectType _subjectType;
        private bool ReadQueryString()
        {
            try
            { Subject = Request.QueryString["subject"]; }
            catch (Exception)
            { return false; }
            try
            { Child = Request.QueryString["child"]; }
            catch (Exception)
            { return false; }
            try
            { _subjectType = (LinkTree.SubjectType)int.Parse(Request.QueryString["type"]); }
            catch (Exception)
            { return false; }
            return true;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            const string indRow = "<tr><td>{0}</td><td>{1}</td><td>{2:yyyy-MM-dd}</td><td>{3}</td><td>{4}</td></tr>";
            const string indRowNoDate = "<tr><td>{0}</td><td>{1}</td><td>.</td><td><td>{3}</td><td>{4}</td></tr>";
            ReadQueryString();
            Label1.Text = Subject;
            Label2.Text = Child;
            var dal = new LinkManager();
            var links=dal.GetLinksBySubjectWithDetails(Subject, (int) _subjectType);
            var sw=new StringWriter();
            foreach (var link in links)
            {
                if (link.Subject2!=Child) continue;
                if (link.Isexternal_linkNull()) link.external_link = string.Empty;
                if (link.Isinternal_linkNull()) link.internal_link = string.Empty;
                if (link.Islink_dateNull())
                    sw.WriteLine(indRowNoDate, link.Link_Type, link.Link_Title, string.Empty,RenderLink(link.external_link),RenderLink(link.internal_link));
                else
                    sw.WriteLine(indRow, link.Link_Type, link.Link_Title, link.link_date, RenderLink(link.external_link), RenderLink(link.internal_link));
            }
            Literal1.Text = sw.ToString();
        }
        private static string RenderLink(string url)
        {
            return url.Trim() == string.Empty ? "&nbsp;" : string.Format("<a href=\"{0}\" target=blank>Link</a>",url);
        }
    }
}
