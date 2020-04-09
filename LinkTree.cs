using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using COI.DAL;
namespace COI.BLL
{
    public class LinkTree
    {
        #region publicMembers
        public Node Root;
        public int Count { get { return Icount; } }
        public LinkTree(string name, SubjectType type)
        {
            Dal = new LinkManager();
            CompaniesCache = new Dictionary<string, DtstCOIsprocs.sp_GetLinksBySubjectDataTable>();
            PeopleCache = new Dictionary<string, DtstCOIsprocs.sp_GetLinksBySubjectDataTable>();
            Root=Node.CreateRoot(name,type,this);
        }
        public void ToXml(TextWriter s)
        {Root.RootToXML(s);}
        public void Populate(int maxDept,int maxNodes)
        {
            Icount = 0;
            MaxCount = maxNodes;
            Root.PopulateRecursive(maxDept);
        }
        public enum SubjectType
        { Individual = 1, Company = 0, }
        #endregion
        #region internalMembers
        internal int Icount;
        internal int MaxCount;
        internal readonly LinkManager Dal;
        internal readonly Dictionary<string, DtstCOIsprocs.sp_GetLinksBySubjectDataTable> CompaniesCache;
        internal readonly Dictionary<string, DtstCOIsprocs.sp_GetLinksBySubjectDataTable> PeopleCache;
        #endregion
        #region ToString
        public void ToStringTabSepparatedTable(TextWriter sw)
        {
            var links =new DtstCOIsprocs.sp_GetLinksBySubjectDataTable();
            foreach (var cache in PeopleCache.Values)
            {
                foreach (var link in cache)
                {
                    if (null!=links.FindBySubject_TypeSubjectSubject_Type2Subject2(
                        link.Subject_Type,link.Subject,link.Subject_Type2,link.Subject2)
                        || null != links.FindBySubject_TypeSubjectSubject_Type2Subject2(
                        link.Subject_Type2, link.Subject2, link.Subject_Type, link.Subject)) continue;
                    links.Addsp_GetLinksBySubjectRow(link.Subject_Type, link.Subject, link.Subject_Type2, link.Subject2);
                }
            }
            foreach (var cache in CompaniesCache.Values)
            {
                foreach (var link in cache)
                {
                    if (null != links.FindBySubject_TypeSubjectSubject_Type2Subject2(
                        link.Subject_Type, link.Subject, link.Subject_Type2, link.Subject2)
                        || null != links.FindBySubject_TypeSubjectSubject_Type2Subject2(
                        link.Subject_Type2, link.Subject2, link.Subject_Type, link.Subject)) continue;
                    links.Addsp_GetLinksBySubjectRow(link.Subject_Type,link.Subject,link.Subject_Type2,link.Subject2);
                }
            }
            sw.WriteLine("subject1\tsubject2");
            foreach (var link in links)
            {
                sw.WriteLine("{0}\t{1}",link.Subject,link.Subject2);
            }
        }
        #endregion
        public class Node
        {
            #region declares
            public string Name;
            public SubjectType Type;
            public List<Node> Children;
            public readonly Node Parent;
            public readonly int Depth;
            public readonly LinkTree Tree;
            #endregion
            #region constructs
            internal static Node CreateRoot(string name, SubjectType type,LinkTree tree)
            {
                var r = new Node(name, type,tree);
                return r;
            }
            private Node(string name, SubjectType type, Node parent)
            {
                Name = name;
                Type = type;
                Depth = parent.Depth + 1;
                Parent = parent;
                Tree = parent.Tree;
                Children = new List<Node>();
            }
            private Node(string name, SubjectType type,LinkTree tree)
            {
                Name = name;
                Type = type;
                Depth = 0;
                Parent = null;
                Children = new List<Node>();
                Tree = tree;
            }
            #endregion
            #region buildTree
            internal void PopulateRecursive(int depth)
            {
                PopulateNodes();
                if (depth > 6) depth = 6;
                if (depth > 1)
                    foreach (var child in Children)
                        child.PopulateRecursive(depth - 1);
            }
            private void PopulateNodes()
            {
                if (Tree.Icount >= Tree.MaxCount) return;
                DtstCOIsprocs.sp_GetLinksBySubjectDataTable links;
                if (Type == SubjectType.Company && Tree.CompaniesCache.ContainsKey(Name))
                    links = Tree.CompaniesCache[Name];

                else if (Type == SubjectType.Individual && Tree.PeopleCache.ContainsKey(Name))
                    links = Tree.PeopleCache[Name];
                else
                {
                    links = Tree.Dal.GetLinksBySubject(Name, (int)Type);
                    if (Type == SubjectType.Company) Tree.CompaniesCache.Add(Name, links);
                    if (Type == SubjectType.Individual) Tree.PeopleCache.Add(Name, links);
                }
                foreach (var link in links)
                    AddChild(link.Subject2, (SubjectType)link.Subject_Type2);
            }
            public bool IsRoot { get { return Depth == 0; } }
            public void AddChild(string name, SubjectType type)
            {
                if (Tree.Icount>=Tree.MaxCount) return;
                if (HasParent(name)) return;
                if (HasChild(name)) return;
                var child = new Node(name, type, this);
                Children.Add(child);
                Tree.Icount++;
            }
            public bool HasChild(string name)
            {
                var hasChild = false;
                foreach (var child in Children)
                {
                    if (child.Name != name) continue;
                    hasChild = true; break;
                }
                return hasChild;
            }
            public List<Node> Parents
            {
                get
                {
                    var parents = new List<Node>();
                    var n = this;
                    while (!n.IsRoot)
                    { n = n.Parent; parents.Add(n); }
                    return parents;
                }
            }
            public bool HasParent(string name)
            {
                var isParent = false;
                foreach (var parent in Parents)
                {
                    if (parent.Name != name) continue;
                    isParent = true; break;
                }
                return isParent;
            }
            #endregion
            #region tostring
            public string ToStringJSON()
            {
                var sw = new StringWriter();
                var s = string.Format("[{1}][{2}]{0}", Name, Type, Depth);
                foreach (var parent in Parents)
                    s = string.Format("[{2}]{0} -> {1}", parent.Name, s, parent.Type);
                sw.WriteLine(s);
                foreach (var child in Children)
                    sw.WriteLine(child.ToString());
                return sw.ToString();
            }
            public void NodeToXML(XmlWriter xw)
            {
                xw.WriteStartElement(Type.ToString());
                xw.WriteAttributeString("name", Name);
                xw.WriteAttributeString("depth", Depth.ToString());
                foreach (var child in Children) child.NodeToXML(xw);
                xw.WriteEndElement();
            }
            public void RootToXML(TextWriter s)
            {
                var xo = new XmlWriterSettings { Indent = true, NewLineOnAttributes = true, OmitXmlDeclaration = false };
                var x = XmlWriter.Create(s, xo);
                if (x == null) return;
                NodeToXML(x);
                x.Flush();
                x.Close();
            }
            #endregion
        }
    }
}
