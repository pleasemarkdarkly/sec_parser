using System;
using System.IO;
using System.Collections.Generic;
using SECCrawler.DAL;
using System.Text.RegularExpressions;
namespace SECCrawler.BLL
{
    public class SECFormsDocAnalyzer
    {
        #region declares
        private readonly SECFormsBlobManager _dal = new SECFormsBlobManager();
        #endregion
        #region Split SEC doc
        public int GetFormSubdocCount(Guid formId)
        {
            const string tag = "PUBLIC DOCUMENT COUNT:";
            var header = _dal.GetFormBlob(formId);
            try
            {
                if (header.IndexOf(tag) > 0)
                {
                    header = header.Substring(header.IndexOf(tag) + tag.Length, 20);
                    header = header.Substring(0, header.IndexOf("\n")).Trim();
                    header = header.Replace("\t", "");
                    return int.Parse(header);
                }
            }
            catch {
                return 0; }
            return 0;
        }
        public string GetFormSubDoc(Guid formId, int expecedSequence, out string newFileName, out string type)
        {
            newFileName = string.Empty;
            type = string.Empty;
            if (expecedSequence == -1) return _dal.GetFormBlob(formId);
            if (expecedSequence==0)
            {
                const string tag = "</SEC-HEADER>";
                var header = _dal.GetFormBlob(formId);
                return header.IndexOf(tag) > 0 ? header.Substring(0, header.IndexOf(tag)+tag.Length) : string.Empty;
            }
            //^^seq 0 is the original doc, no extraction
            var sequence = 0;
            var reader = new StringReader(_dal.GetFormBlob(formId));
            var aLine = string.Empty;
            while (aLine != null && newFileName == string.Empty)
            {
                while (true)
                {
                    aLine = reader.ReadLine();
                    if (aLine == null || aLine.Trim() == "<DOCUMENT>") break;
                }
                while (aLine != null)
                {
                    aLine = reader.ReadLine();
                    if (aLine == null || aLine == "<TEXT>") break;
                    if (aLine.StartsWith("<SEQUENCE>")) sequence = int.Parse(aLine.Replace("<SEQUENCE>", string.Empty));
                    if (aLine.StartsWith("<FILENAME>")) newFileName = aLine.Replace("<FILENAME>", string.Empty);
                    if (aLine.StartsWith("<TYPE>")) type = aLine.Replace("<TYPE>", string.Empty);
                }
                if (aLine != null && expecedSequence == sequence)
                {
                    var outFile = new StringWriter();
                    while (true)
                    {
                        aLine = reader.ReadLine();
                        if (aLine == null) break;
                        if (aLine.Trim() == "</TEXT>") break;
                        outFile.WriteLine(aLine);
                    }
                    reader.Close();
                    var outFileS = outFile.ToString();
                    if (outFileS.Contains("<S>") & !outFileS.Contains("</S>")) outFileS = outFileS.Replace("<S>", "");
                    if (outFileS.Contains("<s>") & !outFileS.Contains("</s>")) outFileS = outFileS.Replace("<s>", "");
                    return outFileS;
                }
                newFileName = string.Empty;
            }
            reader.Close();
            newFileName = "EmptyFile.txt";
            return "document or subdocument not found";
        }
        public List<string> GetFormSubDocParagraphs(Guid formId, int expectedSequence)
        {
            var pars = new List<string>();
            var aLine = string.Empty;
            var pastEmptyLines = 0;
            var par = new StringWriter();
            string newFileName = null;
            string type = null;
            if (expectedSequence<0)
            {
                var count = GetFormSubdocCount(formId);
                for (var i=1;i<=count;i++)
                {
                    //doc += GetFormSubDoc(formId, i,out newFileName)+"\n";
                    var newlist = GetFormSubDocParagraphs(formId, i);
                    pars.AddRange(newlist);
                }
                return pars;
            }
            var doc = GetFormSubDoc(formId, expectedSequence, out newFileName,out type);
            if (type.ToLower() == "graphic")
            {pars.Add("Embedded graphic goes here .." + newFileName);
                return pars;
            }
            doc= (expectedSequence>0 && doc.ToUpper().Contains("<HTML>") ) ? StripHTML(doc):StripHTMLSimple(doc);
            var subDoc = new StringReader(doc);
            while (aLine != null)
            {
                aLine = subDoc.ReadLine(); if (aLine == null) break;
                if (!IsLineBreak(aLine))
                {
                    if (pastEmptyLines > 1)
                    {
                        pars.Add(par.ToString());
                        par = new StringWriter();
                        pastEmptyLines = 0;
                    }
                    par.WriteLine(aLine);
                }
                else
                    pastEmptyLines++;
            }
            pars.Add(par.ToString());
            return pars;
        }
        public List<string> GetFormSubDocParagraphsWithSearch(Guid formId, int expectedSequence, string term1, int includeAditionalParagraphs, bool multipleResults)
        {
            var paragraphs = GetFormSubDocParagraphs(formId, expectedSequence);
            var outParagraphs = new List<string>();
            var outIndexes = new List<bool>();
            var resultFound = false;
            foreach (var par in paragraphs)
            {
                outIndexes.Add(false);
            }
            for (var i1 = 0; i1 < paragraphs.Count & (multipleResults || !resultFound); i1++)
            {
                var par1 = paragraphs[i1];
                if (Contains(par1, term1))
                {
                    var lowIndex = i1 - includeAditionalParagraphs;
                    lowIndex = Math.Max(lowIndex, 0);
                    var highIndex = i1 + includeAditionalParagraphs;
                    highIndex = Math.Min(highIndex, paragraphs.Count - 1);
                    for (var index = lowIndex; index <= highIndex; index++)
                    {
                        outIndexes[index] = true;
                    }
                    resultFound = true;
                }
            }
            for (var index = 0; index < outIndexes.Count; index++)
            {
                if (outIndexes[index]) outParagraphs.Add(paragraphs[index]);
            }
            return outParagraphs;
        }
        public List<string> GetFormSubDocParagraphsWithProximity(Guid formId, int expectedSequence, int proximity, string term1, string term2, int includeAditionalParagraphs, bool multipleResults)
        {
            var paragraphs = GetFormSubDocParagraphs(formId, expectedSequence);
            var outParagraphs = new List<string>();
            var outIndexes=new List<bool>();
            var resultFound = false;
            foreach (var par in paragraphs)
            {
                outIndexes.Add(false);
            }
            for (var i1 = 0; i1 < paragraphs.Count & (multipleResults || !resultFound); i1++)
            {
                var par1 = paragraphs[i1];
                if (Contains(par1, term1))
                {
                    for (var i2 = i1-proximity; i2 <= i1+proximity & (multipleResults || !resultFound); i2++)
                    {
                        if (i2<0 || i2>=paragraphs.Count) continue;
                        var par2 = paragraphs[i2];
                        if (Contains(par2,term2))
                        {
                            var lowIndex = Math.Min(i2,i1)-includeAditionalParagraphs;
                            lowIndex = Math.Max(lowIndex, 0);
                            var highIndex = Math.Max(i1, i2) + includeAditionalParagraphs;
                            highIndex = Math.Min(highIndex, paragraphs.Count - 1);
                            for (var index = lowIndex; index <= highIndex; index++)
                            {
                                outIndexes[index] = true;
                            }
                            resultFound = true;
                        }
                    }
                }
            }
            for (var index = 0; index < outIndexes.Count;index++ )
            {
                if (outIndexes[index]) outParagraphs.Add(paragraphs[index]);
            }
            return outParagraphs;
        }
        public List<List<string >> GetTable(Guid formID,int subDocIndex,string textCriteria)
        {
            var s = string.Empty;
            var t = string.Empty;
            var doc = GetFormSubDoc(formID,subDocIndex,out s,out t);
            return GetTable(doc, textCriteria);
            
        }
        public List<List<string >> GetTable(string htmlDoc,string textCriteria)
        {
            const string s0 = "<table";
            const string s1 = "</table>";
            var t = new List<List<string>>();
            var htmlDocLower = htmlDoc.ToLower();
            var textCriteriaLower = textCriteria.ToLower();
            var index = htmlDocLower.IndexOf(textCriteriaLower);
            if (index>=0)
            {
                htmlDoc = htmlDoc.Substring(index);
                htmlDoc = GetSubDoc(htmlDoc, s0, s1);
                var rows = GetRows(htmlDoc);
                foreach (var row in rows)
                {
                    t.Add(GetCells(row,true));
                }
            }
            return t;
        }
        protected string GetSubDoc(string doc, string opener, string closer)
        {
            try
            {
                var s0 = opener.ToLower();
                var s1 = closer.ToLower();
                var lowdoc = doc.ToLower();
                var index0 = lowdoc.IndexOf(s0);
                var index1 = lowdoc.IndexOf(s1, index0) + s1.Length;
                if (index0 == -1 || index1 == -1)
                    return "invalid document\r\n";
                var subdoc = doc.Substring(index0, index1 - index0);
                return subdoc;
            }
            catch
            {
                return string.Empty;
            }
        }
        protected List<string> GetRows(string htmlTable)
        {
            try
            {
                var lowTable = htmlTable.ToLower();
                var rows = new List<string>();
                const string s0 = "<tr";
                const string s1 = "</tr>";
                while (lowTable.IndexOf(s0) > -1)
                {
                    var index0 = lowTable.IndexOf(s0);
                    var index1 = lowTable.IndexOf(s1, index0) + s1.Length;
                    if (lowTable.IndexOf(s1, index0) == -1) break;
                    var row = htmlTable.Substring(index0, index1 - index0);
                    rows.Add(row);
                    htmlTable = htmlTable.Substring(index1);
                    lowTable = lowTable.Substring(index1);
                }
                return rows;
            }
            catch (Exception e)
            {
                return new List<string> { e.Message };
            }
        }
        protected List<string> GetCells(string htmlRow)
        {return GetCells(htmlRow, false); }
        protected List<string> GetCells(string htmlRow,bool stripHTML)
        {
            try
            {
                var lowRow = htmlRow.ToLower();
                var cells = new List<string>();
                const string s0 = "<td";
                const string s1 = "</td>";
                while (lowRow.IndexOf(s0) > -1)
                {
                    var index0 = lowRow.IndexOf(s0);
                    index0 = lowRow.IndexOf(">", index0) + 1;
                    var index1 = lowRow.IndexOf(s1, index0);
                    var cell = htmlRow.Substring(index0, index1 - index0);
                    if (stripHTML) cell = StripHTML(cell);
                    cells.Add(cell);
                    htmlRow = htmlRow.Substring(index1);
                    lowRow = lowRow.Substring(index1);
                }
                return cells;
            }
            catch (Exception e)
            {
                return new List<string> { e.Message };
            }
        }
        #endregion
        #region HTML to plain text
        public static string StripHTMLSimple(string source)
        {
            try
            {
                var result = Regex.Replace(source, "<table>", "", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "</table>", "", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "<S>", "", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "<caption>", "", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "<C>", "", RegexOptions.IgnoreCase);
                return result;
            }
            catch
            {
                return source;
            }
        }
        public static string StripHTML(string source)
        {
            try
            {
                var result = source.Replace("\r", " ");
                result = result.Replace("\n", " ");
                result = result.Replace("\t", string.Empty);
                result = Regex.Replace(result, @"( )+", " ");
                result = Regex.Replace(result, @"<( )*head([^>])*>", "<head>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"(<( )*(/)( )*head( )*>)", "</head>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(<head>).*(</head>)", string.Empty, RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*script([^>])*>", "<script>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"(<( )*(/)( )*script( )*>)", "</script>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"(<script>).*(</script>)", string.Empty, RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*style([^>])*>", "<style>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"(<( )*(/)( )*style( )*>)", "</style>", RegexOptions.IgnoreCase);
                result = Regex.Replace(result, "(<style>).*(</style>)", string.Empty, RegexOptions.IgnoreCase);
                result = Regex.Replace(result, @"<( )*td([^>])*>", "\t", RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"<( )*br( )*>", "\r",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"<( )*li( )*>", "\r",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"<( )*div([^>])*>", "\r\r",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"<( )*tr([^>])*>", "\r\r",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"<( )*p([^>])*>", "\r\r",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"<[^>]*>", string.Empty,
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @" ", " ",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&bull;", " * ",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&lsaquo;", "<",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&rsaquo;", ">",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&trade;", "(tm)",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&frasl;", "/",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&lt;", "<",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&gt;", ">",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&copy;", "(c)",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"<strike>", " ",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"</strike>", " ",
                         RegexOptions.IgnoreCase); 
                result = Regex.Replace(result,
                         @"&reg;", "(r)",
                         RegexOptions.IgnoreCase); 
                result = Regex.Replace(result,
                         @"&(.{2,6});", string.Empty,
                         RegexOptions.IgnoreCase);
                result = result.Replace("\n", "\r");
                result = Regex.Replace(result,
                         "(\r)( )+(\r)", "\r\r",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         "(\t)( )+(\t)", "\t\t",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         "(\t)( )+(\r)", "\t\r",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         "(\r)( )+(\t)", "\r\t",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         "(\r)(\t)+(\r)", "\r\r",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         "(\r)(\t)+", "\r\t",
                         RegexOptions.IgnoreCase);
                while (result.Contains("\r\r\r"))
                    result = result.Replace("\r\r\r", "\r\r");
                while (result.Contains("\t\t\t\t\t"))
                    result = result.Replace("\t\t\t\t\t", "\t\t\t\t");
                return result;
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        #endregion
        #region aux
        private static bool Contains(string text, string keyword)
        {return text.ToLower().IndexOf(keyword.Trim().ToLower()) >= 0;}
        private static bool IsLineBreak(string s)
        {
            if (s == null) return true;
            if (s == string.Empty) return true;
            return s.Trim() == string.Empty;
        }
        #endregion
        #region snippets
        public string GetFormSnippet(Guid formId,int subdoc)
        {
            var snippet = string.Empty;
            var pars = GetFormSubDocParagraphs(formId, subdoc);
            while (snippet.Length<500 && pars.Count>0)
            {
                snippet = snippet + pars[0] + "\r\n";
                pars.RemoveAt(0);
            }
            if (snippet.Length > 500) snippet = snippet.Substring(0, 500);
            return snippet;
        }
        public string GetFormSnippet(Guid formId, int subdoc,string keyPhrase)
        {
            var snippet = string.Empty;
            var pars = GetFormSubDocParagraphs(formId, subdoc);
            var index=pars.Count;
            for (var i = 0; i < pars.Count;i++ )
            {
                var par = pars[i];
                if (!par.ToLower().Contains(keyPhrase.ToLower())) continue;
                snippet = par;
                index = i;
                break;
            }
            while (snippet.Length<500 && index+1<pars.Count)
            {
                index++;
                snippet += "\r\n" + pars[index];
            }
            if (snippet.Length > 500) snippet = snippet.Substring(0, 500);
            return snippet;
        }
        #endregion
    }
}
