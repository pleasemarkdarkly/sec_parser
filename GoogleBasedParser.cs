using System;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Web;
using System.Xml.Serialization;
using SECCrawler.DAL;
using System.Diagnostics;
using Microsoft.Win32;
using System.Collections.Generic;
using COI.Util;
namespace FileList
{
    public partial class GoogleBasedParser : Form
    {
        private List<string> _summary=new List<string>();
        public GoogleBasedParser()
        {
            InitializeComponent();
        }
        private enum SearchType{ cat_files=1,email=2,SEC=3}
        private DateTime _lastRefresh = DateTime.Now;
        protected string GetFullDirNameSafe(DirectoryInfo path)
        {
            string dirName = path.Name;
            DirectoryInfo path1 = path;
            while (path1.Parent != null && path1.Parent.Name != path1.Name)
            {
                path1 = path1.Parent;
                if (!path1.Name.EndsWith("\\"))
                    dirName = path1.Name + "\\" + dirName;
                else
                    dirName = path1.Name + dirName;
            }
            return dirName;
        }
        protected string GetBaseQueryURL()
        {
            RegistryKey currentUser = RegistryKey.OpenRemoteBaseKey(Microsoft.Win32.RegistryHive.CurrentUser, "");
            RegistryKey searchUrl = currentUser.OpenSubKey("Software\\Google\\Google Desktop\\API");
            if (searchUrl != null)
            {
                var key = searchUrl.GetValue("search_url");
                return key.ToString();
            }
            return string.Empty;
        }
        private results QueryGoogoleDestop(DirectoryInfo dir, int position, bool subfolder, SearchType searchType)
        {
            var baseQuery = GetBaseQueryURL();
            baseQuery += "?ie=UTF-8&adv=1&type=" + searchType + "&ot=&file=&ext=";
            baseQuery += "&to=&from=&domain=&no=&within=86400&day=&format=xml";
            baseQuery += "&start=" + position.ToString();
            baseQuery += "&has=" + HttpUtility.UrlEncode(this.textCriteria.Text);
            if (searchType == SearchType.cat_files)
            {
                if (subfolder) baseQuery += "&under=on";
                baseQuery += "&in=" + HttpUtility.UrlEncode(GetFullDirNameSafe(dir));
            }
            else
                baseQuery += "&in=";
            var downloadRequest = (HttpWebRequest)WebRequest.Create(baseQuery);
            var downloadResponse = (HttpWebResponse)downloadRequest.GetResponse();
            results res;
            try
            {
                var xSerializer = new XmlSerializer(typeof(results));
                res = (results)xSerializer.Deserialize(downloadResponse.GetResponseStream());
            }
            catch 
            {
                this.textOutPut.AppendText("unable to deserialize: \r\n" + baseQuery + "\r\n");
                this.textOutPut.AppendText("\r\n==start link=======================\r\n");
                this.textOutPut.AppendText(baseQuery);
                this.textOutPut.AppendText("\r\n==end   link=======================\r\n");
                return null;
            }
            return res;
        }
        private int QuerySecDB(dtstResults dtst)
        {
            var rows = 0;
            var manager = new SECFormsManager();
            var table = manager.GetFormsByFullTextSearchAndFormType
                (this.textCriteria.Text, "(all)",LinkResponseType.NotClickableWithModifiedPath, "");
            rows = table.Rows.Count;
            var i = 0;
            foreach (var row1 in table)
            {
                i--;
               dtst.Result.AddResultRow(row1.FormDate, "SEC filing",row1.FormPartialURL, row1.FormPartialURL, "--No Preview--",
                       row1.CompanyName, string.Empty, string.Empty,i);
            }
            return rows;
        }
        private int DownloadOneFolderToDataset(DirectoryInfo dir, dtstResults dtst,bool subFolders,SearchType searchType)
        {
            var position = 0;
            while (true)
            {
                var res = QueryGoogoleDestop(dir, position, subFolders, searchType);
                if (res==null || res.result == null) break;
                if (position == 0 && res.count > 0 && !subFolders)
                try
                    {
                        this.textOutPut.AppendText("Path: " + dir.Name + " .. items: " + res.count + " ... ");
                    }
                catch
                    {
                        this.textOutPut.AppendText("Path: " + this.GetFullDirNameSafe(dir) + " .. items: " + res.count + " ... ");
                    }
                else if (subFolders)
                    this.textOutPut.AppendText("items: " + this.progressBar1.Value.ToString() + "\r\n");
                foreach (var result1 in res.result)
                {
                    DateTime d;
                    var snippet = result1.snippet;
                    var shortURL = searchType == SearchType.cat_files ? result1.url.ToLower().Replace(this.textFolder.Text, string.Empty) : result1.title;
                    try
                    {
                        var fi = new FileInfo(result1.url);
                        d = fi.LastWriteTime;
                    }
                    catch
                    {
                        d = DateTime.FromFileTime(result1.time);
                    }
                    if (string.IsNullOrEmpty(snippet))
                        snippet = " - no preview - ";
                    else
                    {
                        snippet = HttpUtility.HtmlDecode(snippet);
                        snippet = SECCrawler.BLL.SECFormsDocAnalyzer.StripHTML(snippet);
                    }
                    if (dtst.Result.FindByrsultId(result1.doc_id) == null)
                        dtst.Result.AddResultRow(d, result1.category, result1.url, shortURL, snippet,
                            result1.from,string.Empty,string.Empty, result1.doc_id);
                    position += 1;
                }
                if (this.progressBar1.Value + res.result.Length <= progressBar1.Maximum)
                {
                    this.progressBar1.Value += res.result.Length;
                    if ((DateTime.Now - _lastRefresh).TotalSeconds >= 3)
                    {
                        this.Refresh();
                        System.Threading.Thread.Sleep(500);
                        _lastRefresh = DateTime.Now;
                    }
                }
                if (position >= (res.count)) break;
            }
            return position;
        }
        private int GetEsimateCountofDocs(DirectoryInfo dir, SearchType searchType)
        {
            var res1 = QueryGoogoleDestop(dir, 0, true,searchType);
            if (null == res1) return 0;
            return (res1.count);
        }
        private bool Validation()
        {
            this.textOutPut.Clear();
            this.textFolder.Text = this.textFolder.Text.Trim().ToLower();
            if (!this.textFolder.Text.EndsWith(@"\")) this.textFolder.AppendText(@"\");
            if (checkDocs.Checked)
            {
                if (!Directory.Exists(this.textFolder.Text))
                {
                    this.textOutPut.AppendText("The folder does not seem to exist\r\n");
                    return false;
                }
            }
            if (checkEmail.Checked)
            {
                if (this.textCriteria.Text.Trim() == string.Empty)
                {
                    this.textOutPut.AppendText("Email search cannot be performed without some criteria\r\n");
                    return false;
                }
            }
            return true;
        }
        private void IterateFolders(DirectoryInfo dir, dtstResults dtst)
        {
            int i=DownloadOneFolderToDataset(dir, dtst,false,SearchType.cat_files);
            foreach (var sDir in dir.GetDirectories())
            {
                IterateFolders(sDir, dtst);
            }
        }
        private void PublishDatasetToExcel(DataSet dtst)
        {
            var fileName2 = Path.Combine(this.textFolder.Text, "FileList" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xls");
            var fileS = File.OpenWrite(fileName2);
            ExcelEngine.Convert(dtst,fileS,2,2,_summary);
            fileS.Close();
            this.textOutPut.AppendText("Your file list has been saved to: " + fileName2 + "\r\n");
            var notePad = new Process {StartInfo = {FileName = fileName2}};
            try
            {
                notePad.Start();
            }catch//(Exception ex)
            {
                this.textOutPut.AppendText("Unable to launch Excel Doc\r\n" );
            }
        }
        private void SetEstimates(DirectoryInfo dir)
        {
            this.tabControl1.SelectedTab = tabPage1;
            var emailEstimate = 0;
            var docEstimate = 0;
            this.progressBar1.Value = 0;
            this.progressBar1.Maximum = 0;
            this.progressBar1.Visible = true;
            this.progressBar1.Minimum = 0;
            if (checkDocs.Checked)
            {
                docEstimate = GetEsimateCountofDocs(dir, SearchType.cat_files);
                this.textOutPut.AppendText("Total number of docs (estimate):" + docEstimate + "\r\n");
                _summary.Add("Found documents:" + docEstimate);
            }
            if (checkEmail.Checked)
            {
                emailEstimate = GetEsimateCountofDocs(null, SearchType.email);
                this.textOutPut.AppendText("Total number of emails (estimate):" + emailEstimate + "\r\n");
                _summary.Add("Found emails:" + emailEstimate);
            }
            progressBar1.Maximum = docEstimate + emailEstimate;
        }
        private void btnSetFolder_Click(object sender, EventArgs e)
        {
            if (textFolder.Text.Trim() != string.Empty)
                if (Directory.Exists(textFolder.Text))
                    folderBrowserDialog1.SelectedPath = textFolder.Text;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            this.textFolder.Text = folderBrowserDialog1.SelectedPath;

        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                textCriteria.Text = checkListSamples.CheckedItems[0].ToString();
            }catch{}
        }
        private void btnProcess_Click(object sender, EventArgs e)
        {
            if (!Validation()) return;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                _summary.Clear();
                _summary.Add("Searched for:" + HttpUtility.HtmlEncode( this.textCriteria.Text));
                _summary.Add("Search time:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
                var dir = new DirectoryInfo(this.textFolder.Text);
                SetEstimates(dir);
                var dtst = new dtstResults();
                if (checkSEC.Checked)
                    try
                    {
                        var rows = QuerySecDB(dtst);
                        _summary.Add("Found SEC filings:" + rows.ToString());
                        this.progressBar1.Maximum += rows;
                        this.progressBar1.Value += rows;
                        this.textOutPut.AppendText("\r\nSEC Filings found: " + rows.ToString() + "\r\n");
                    }
                    catch (Exception ex)
                    {
                        this.textOutPut.AppendText("\r\nUnable to Query SecDB:\r\n" + ex.Message);

                    }
                if (checkDocs.Checked)
                    if (this.checkIterate.Checked)
                        IterateFolders(dir, dtst);
                    else
                        DownloadOneFolderToDataset(dir, dtst, true, SearchType.cat_files);
                if (checkEmail.Checked)
                    DownloadOneFolderToDataset(null, dtst, true, SearchType.email);
                if (dtst.Result.Count > 0)
                {
                    this.textOutPut.AppendText("\r\nPublishing to XLS\r\n");
                    PublishDatasetToExcel(dtst);
                }
                else
                    this.textOutPut.AppendText("\r\nNo matches found\r\n");
            }
            catch (Exception ex) { this.textOutPut.AppendText(ex.ToString() + "\r\n"); }
            finally
            {
                this.progressBar1.Visible = false;
                this.Cursor = Cursors.Default;
            }
        }
    }
}