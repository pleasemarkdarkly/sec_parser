using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using COI.DAL;
namespace COI.BLL.Parsers
{
    public class ParserBase
    {
        #region declares
        protected int SourceId;
        protected int DocSizeKb=0;
        protected string Url{ get;set;}
        protected int InvestigationId { get; set; }
        protected string DocType = "external link";
        protected bool ReadOnly = false;
        public ParserBase()
        {
        }
        #endregion
        #region downloader
        protected string GetDoc(){return GetDoc(new StringWriter());}
        protected string GetDoc(TextWriter log){return GetDoc(log, Url);}
        protected string GetDoc(TextWriter log,string url1)
        {
            var s = new StringWriter();
            try
            {
                log.WriteLine("Attempting to retrieve web doc: {0}",url1);
                var downloadRequest = (HttpWebRequest)WebRequest.Create(url1);
                var downloadResponse = (HttpWebResponse)downloadRequest.GetResponse();
                var responseStream = downloadResponse.GetResponseStream();
                var buffer = new Char[1024];
                var totalBytes = 0;
                var encode = Encoding.UTF8;
                var readStream = new StreamReader(responseStream, encode);
                while (true)
                {
                    var bytesRead = readStream.Read(buffer, 0, buffer.Length);
                    totalBytes += bytesRead;
                    if (bytesRead == 0)
                        break;
                    s.Write(buffer, 0, bytesRead);
                }
                responseStream.Close();
                readStream.Close();
                log.WriteLine("Retrieved web doc with {0} bytes", totalBytes);
                DocSizeKb = totalBytes/1024;
            }
            catch (Exception ex)
            {
                log.WriteLine();
                log.Write(ex.ToString());
            }
            return s.ToString();
        }
        #endregion
        #region DB Doc management
        private DocumentManager _docManager;
        public DocumentManager DbDocManager{get{if (_docManager == null) _docManager = new DocumentManager();return _docManager;}}
        protected DtstCOI.documentDataTable CreateOrGetDoc()
        {return DbDocManager.CreateOrGetDocumentByURL(Url, SourceId,DateTime.Now,DocSizeKb,string.Empty,DocType);}
        protected DtstCOI.documentDataTable CreateOrGetDoc(string url1)
        { return DbDocManager.CreateOrGetDocumentByURL(url1, SourceId, DateTime.Now, DocSizeKb, string.Empty, DocType); }
        public DtstCOI.documentRow GetOrCreateDbDoc()
        {
            var dbDoc = ReadOnly ? new DtstCOI.documentDataTable() : CreateOrGetDoc();
            if (dbDoc.Rows.Count == 0)
                dbDoc.AdddocumentRow(SourceId, "link", Url, string.Empty, System.Guid.NewGuid(), DateTime.Now, 0, string.Empty, DocType);
            return dbDoc[0];
        }
        public DtstCOI.documentRow GetOrCreateDbDoc(string url1)
        {return GetOrCreateDbDoc(url1, "link", DateTime.Now, DocType); }
        public DtstCOI.documentRow GetOrCreateDbDoc(string url1, string fullOrExcerpt, DateTime docDate, string docType)
        {
            var dbDoc = ReadOnly ? new DtstCOI.documentDataTable() : CreateOrGetDoc(url1);
            if (dbDoc.Rows.Count == 0)
                dbDoc.AdddocumentRow(SourceId,fullOrExcerpt, url1, string.Empty,Guid.Empty, docDate,0, string.Empty, docType);
            return dbDoc[0];
        }
        #endregion
        #region text manipulation
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
        { return GetCells(htmlRow, false); }
        protected List<string> GetCells(string htmlRow, bool stripHTML)
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
        protected string GetSubDoc(string doc, string opener, string closer)
        {
            try
            {
                var s0 = opener.ToLower();
                var s1 = closer.ToLower();
                var lowdoc = doc.ToLower();
                var index0 = lowdoc.IndexOf(s0);
                var index1 = lowdoc.IndexOf(s1, index0) + s1.Length;
                if (index0 == -1 || index1 == -1) return "invalid document\r\n";
                var subdoc = doc.Substring(index0, index1 - index0);
                return subdoc;
            }
            catch
            {
                return string.Empty;
            }
        }
        public List<List<string>>GetCellFromCSV(string doc,char sepparator)
        {
            var data = new List<List<string>>();
            doc = doc.Replace("\r\n", "\n" );
            var rows = doc.Split('\n');
            foreach (var row in rows)
            {
                var cells = row.Split(sepparator);
                var rowData = new List<string>();
                foreach (var cell in cells)
                {
                    rowData.Add(cell);
                }
                data.Add(rowData);
            }
            return data;
        }
        public List<List<string>> GetCellFromCSV(string doc)
        {
            return GetCellFromCSV(doc, ',');
        }
        public List<List<string>> GetCellFromTSV(string doc)
        {
            return GetCellFromCSV(doc, '\t');
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
        public string GetLinkText(string aTag)
        {
            var index1 = aTag.IndexOf('>') + 1;
            var index2 = aTag.IndexOf('<', index1);
            var text = aTag.Substring(index1, index2 - index1);
            return text;
        }
        #endregion
    }
}
