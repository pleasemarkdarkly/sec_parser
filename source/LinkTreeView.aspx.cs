using System;
using System.Web.UI.WebControls;
using COI.BLL;
namespace COI.WebUI.Diagrams
{
    public partial class LinkTreeView : System.Web.UI.Page
    {
        public string Subject;
        private int _depth;
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
            { Subject = Request.QueryString["subject"]; }
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
            ReadQueryString();
            var tree = new LinkTree(Subject, _subjectType);
            tree.Populate(_depth, _maxNodes);
            var r = new TreeNode(tree.Root.Name,tree.Root.Type.ToString(),string.Empty,string.Empty,string.Empty);
            TreeView1.Nodes.Add(r);
            AddNodesRecursive(r,tree.Root);
        }
        public void AddNodesRecursive(TreeNode tn,LinkTree.Node n)
        {
            foreach (var nChild in n.Children)
            {
                var label = string.Format("[{0}] {1}",nChild.Type,nChild.Name);
                var navigateURL = "javascript:open_win('./LinkDetails.aspx?child=" + Server.UrlEncode(nChild.Name);
                navigateURL += "&subject=" + Server.UrlEncode(n.Name);
                navigateURL += "&type=" + (int)n.Type+"');";
                var tnChild = new TreeNode(label,nChild.Name,string.Empty,navigateURL,"_self");
                
                tn.ChildNodes.Add(tnChild);
                AddNodesRecursive(tnChild,nChild);
            }
        }
    }
}
