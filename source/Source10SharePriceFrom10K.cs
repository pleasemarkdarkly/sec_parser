using System;
using System.Collections.Generic;
using System.IO;
using COI.DAL;
namespace COI.BLL.Parsers
{
    public class Source10SharePriceFrom10K:ParserBase
    {
        private readonly System.Guid _formId;
        private readonly string _coName;
        public DtstCOI.documentRow DbDoc;
        private const string TextCriteria = "market information";
        private const string SnippetType = "10-K market info";
        public Source10SharePriceFrom10K(System.Guid formID,string coName)
        {
            _formId = formID;
            _coName = coName;
        }
        private const int SubDocIndex = 1;
        public List<List<string>> GetTable()
        {
            return GetTable(_formId);
        }
        public string GenerateSnippet()
        {
            var s = new StringWriter();
            var t= GetTable(_formId);
            if (t.Count < 1) return string.Empty;
            var table = DbDocManager.GetDocument(_formId);
            DbDoc = table[0];
            s.Write("<table border=1 bordercolor=#000000 cellspacing=0 cellpadding=1>");
            foreach (var row in t)
            {
                var line = string.Empty;
                var isEmpty = true;
                foreach (var cell in row)
                {   if (cell.Trim() != string.Empty) isEmpty = false;
                    var c = (cell.Trim() == string.Empty) ? "-" : cell.Trim();
                    line+=("<td>"+ c + "</td>");}
                if (isEmpty) continue;
                s.Write("<tr>");
                s.Write(line);
                s.Write("</tr>");
            }
            s.Write("</table>");
            var snippet= s.ToString();
            var snippetsTable = DbDocManager.GetSnippets(DbDoc.document_id);
            var dbsnippet = snippetsTable.FindBydocument_idsnippet_type(DbDoc.document_id, SnippetType);
            if (dbsnippet == null)
                snippetsTable.AddsnippetRow(DbDoc.document_id, SnippetType, snippet,string.Empty,_coName);
            else
                dbsnippet.snippet = snippet;
            DbDocManager.Save(snippetsTable);
            return snippet;
        }
        private List<List<string>> GetTable(System.Guid formID)
        {
            Url = string.Format("http://www.cornerofficellc.com/crawler/FormsBrowser/FetchDocumentFromDB.aspx?FormID={0}&seq={1}",formID,SubDocIndex);
            var higFound = false;
            var lowFound = false;
            var doc = GetDoc();
            var t= GetTable(doc);
            foreach (var row in t)
            {
                foreach (var cell in row)
                {
                    if (cell.ToLower().Contains("high")) higFound = true;
                    if (cell.ToLower().Contains("low")) lowFound = true;
                }
            }
            if (higFound & lowFound)
                return t;
            return new List<List<string>>();
        }
        private List<List<string>> GetTable(string htmlDoc)
        {
            const string s0 = "<table";
            const string s1 = "</table>";
            var t = new List<List<string>>();
            var htmlDocLower = htmlDoc.ToLower();
            var textCriteriaLower = TextCriteria.ToLower();
            var index = htmlDocLower.IndexOf(textCriteriaLower);
            if (index >= 0)
            {
                htmlDoc = htmlDoc.Substring(index);
                htmlDoc = GetSubDoc(htmlDoc, s0, s1);
                var rows = GetRows(htmlDoc);
                foreach (var row in rows) t.Add(GetCells(row, true));
            }
            return t;
        }

    }
}
