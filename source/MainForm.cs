using System;
using System.Windows.Forms;
using System.IO;
namespace SECcrawler.UI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'secCrawlerDataDataSet.tblSEC_Forms' table. You can move, or remove it, as needed.
            //this.tblSEC_FormsTableAdapter.Fill(this.secCrawlerDataDataSet.tblSEC_Forms);
            // TODO: This line of code loads data into the 'secCrawlerDataDataSet.tblSEC_Company' table. You can move, or remove it, as needed.
            this.tblSEC_CompanyTableAdapter.Fill(this.secCrawlerDataDataSet.tblSEC_Company);
            // TODO: This line of code loads data into the 'secCrawlerDataDataSet.tblFiles_files' table. You can move, or remove it, as needed.
            this.tblFiles_filesTableAdapter.Fill(this.secCrawlerDataDataSet.tblFiles_files);

        }
        private void toolPopulateIndexList_Click(object sender, EventArgs e)
        {
            //navigate filesystems and identify files that are not already in the database
            int filesBefore = this.secCrawlerDataDataSet.tblFiles_files.Rows.Count;
            var configReader = new System.Configuration.AppSettingsReader();
            string fileFilter = (string)configReader.GetValue("IncludeFiles", string.Empty.GetType());
            string basePath = (string)configReader.GetValue("DataStoreIndexPath", string.Empty.GetType());
            if (!basePath.EndsWith(@"\")) basePath=basePath+@"\";
            string[] files = Directory.GetFiles(basePath, fileFilter,System.IO.SearchOption.AllDirectories);
            foreach(var f in files)
            {
                var relativePath = f.Replace(basePath, string.Empty);
                var fileRow =this.secCrawlerDataDataSet.tblFiles_files.FindByRelativePath(relativePath);
                if (fileRow == null)
                {
                    fileRow = secCrawlerDataDataSet.tblFiles_files.NewtblFiles_filesRow();
                    fileRow.RelativePath = relativePath;
                    this.secCrawlerDataDataSet.tblFiles_files.AddtblFiles_filesRow(fileRow);
                }
            }
            this.tblFiles_filesTableAdapter.Update(this.secCrawlerDataDataSet.tblFiles_files);
            int filesAfter = this.secCrawlerDataDataSet.tblFiles_files.Rows.Count;
            this.toolStripStatusLabel1.Text = "Index files before:" + filesBefore.ToString();
            this.toolStripStatusLabel1.Text += " | Index files after: " + filesAfter.ToString();
        }
        private void toolStripButtonProcessIndexes_Click(object sender, EventArgs e)
        {
            var configReader = new System.Configuration.AppSettingsReader();
            string basePath = (string)configReader.GetValue("DataStoreIndexPath", string.Empty.GetType());
            int totalIndexFiles = secCrawlerDataDataSet.tblFiles_files.Rows.Count;
            int unprocessedIndexFiles = 0;
            int totalRows = 0;
            int newForms = 0;
            int errors = 0;
            int newUnsavedForms = 0;
            this.toolStripStatusLabel1.Text = string.Empty;
            this.toolStripProgressBar1.Visible = true;
            toolStripProgressBar1.Minimum=0;
            this.toolStripProgressBar1.Value = toolStripProgressBar1.Minimum;
            toolStripProgressBar1.Maximum=totalIndexFiles+1;
            foreach (var index in this.secCrawlerDataDataSet.tblFiles_files)
            {
                this.toolStripProgressBar1.Value++;
                if (index.IsLastProcessedNull())
                {
                    unprocessedIndexFiles++;
                    var path = Path.Combine(basePath, index.RelativePath);
                    if (File.Exists(path))
                    {
                        try 
                        {
                            string[] rows = System.IO.File.ReadAllLines(path);
                            totalRows += rows.Length;
                            foreach (var row in rows)
                            { 
                                if (row!=null)
                                    if (row != string.Empty)
                                    {
                                        var elements = row.Split('|');
                                        if (elements.Length == 5)
                                        {
                                            try
                                            {
                                                decimal companyCIK = 0;
                                                try
                                                {
                                                    companyCIK = decimal.Parse(elements[0]);
                                                }
                                                catch
                                                {
                                                    continue;
                                                }
                                                string companyName = elements[1];
                                                string formType = elements[2];
                                                DateTime formDate =DateTime.Parse( elements[3]);
                                                string formPartialURL = elements[4];
                                                SecCrawlerDataDataSet.tblSEC_FormsRow formRow=null;// = this.secCrawlerDataDataSet.tblSEC_Forms.FindByFormPartialURL(formPartialURL);
                                                if (formRow == null)
                                                {
                                                    newForms++;
                                                    newUnsavedForms++;
                                                    this.secCrawlerDataDataSet.tblSEC_Forms.AddtblSEC_FormsRow(
                                                        companyCIK, companyName, formType, formDate, formPartialURL);
                                                    if (newUnsavedForms>=1000)
                                                    {
                                                        this.tblSEC_FormsTableAdapter.Update(this.secCrawlerDataDataSet.tblSEC_Forms);
                                                        newUnsavedForms = 0;
                                                        this.Refresh();
                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                System.Windows.Forms.Clipboard.SetText(ex.ToString());
                                                errors++;
                                                this.toolStripStatusLabel1.Text = "Errors:" + errors.ToString();
                                                this.Refresh();
                                            }
                                        }
                                    }
                            }
                            index.LastProcessed = DateTime.Now;
                        }
                        catch { errors++; }
                        this.tblFiles_filesTableAdapter.Update(this.secCrawlerDataDataSet.tblFiles_files);
                        this.tblSEC_FormsTableAdapter.Update(this.secCrawlerDataDataSet.tblSEC_Forms);
                        newUnsavedForms = 0;
                        this.secCrawlerDataDataSet.tblSEC_Forms.Clear();
                        this.Refresh();
                    }
                    else errors++;
                }
            }
            this.tblSEC_FormsTableAdapter.Update(this.secCrawlerDataDataSet.tblSEC_Forms);
            this.tblFiles_filesTableAdapter.Update(this.secCrawlerDataDataSet.tblFiles_files);
            this.toolStripProgressBar1.Visible = false;
            this.toolStripStatusLabel1.Text = "Index files:" + totalIndexFiles.ToString();
            this.toolStripStatusLabel1.Text += " | unprocessed: " + unprocessedIndexFiles.ToString();
            this.toolStripStatusLabel1.Text += " | Rows: " + totalRows.ToString();
            this.toolStripStatusLabel1.Text += " | New forms: " + newForms.ToString();
            this.toolStripStatusLabel1.Text += " | Errors: " + errors.ToString();
        }
    }
}
