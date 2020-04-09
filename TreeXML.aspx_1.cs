using System;
using COI.BLL;

namespace COI.WebUI.Diagrams
{
    public partial class TreeXML : System.Web.UI.Page
    {
        private string _subject;
        private int _depth;
        private string _view;
        private LinkTree.SubjectType _subjectType;
        private int _maxNodes;
        private bool ReadQueryString()
        {
            try
            { _depth = int.Parse(Request.QueryString["depth"]); }
            catch (Exception)
            { return false; }
            try
            { _maxNodes = int.Parse(Request.QueryString["maxNodes"]); }
            catch (Exception)
            { return false; }
            try
            { _view = (Request.QueryString["view"]); }
            catch (Exception)
            {_view = "xml"; }
            try
            { _subject = Request.QueryString["subject"]; }
            catch (Exception)
            { return false; }
            try
            { _subjectType = (LinkTree.SubjectType)int.Parse(Request.QueryString["type"]); }
            catch (Exception)
            { return false; }
            return true;
        }
        protected void Page_Load(object sender, EventArgs e)
        {   if (!ReadQueryString()) return;
            var tree = new LinkTree(_subject, _subjectType);
            tree.Populate(_depth,_maxNodes);
            Response.ClearContent();
            Response.ClearHeaders();
            switch (_view)
            {
                case "text":
                    Response.ContentType = "text/plain";
                    tree.ToStringTabSepparatedTable(Response.Output);
                    break;
                case "xml":
                    Response.ContentType = "text/xml";
                    tree.ToXml(Response.Output);
                    break;
            }
        }

    }
}
