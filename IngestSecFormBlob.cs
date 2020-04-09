using System;
using SECCrawler.DAL;
using System.IO;
using System.Text;
namespace SECCrawler.IngestLib
{
    public class IngestSecFormBlob
    {
        private readonly bool _useConsole;
        private readonly string _basePath;
        public IngestSecFormBlob(bool useConsole)
        {
            _useConsole = useConsole;
            var configReader = new System.Configuration.AppSettingsReader();
            _basePath = (string)configReader.GetValue("DataStorePath", string.Empty.GetType());
        }
        protected void Log(string note)
        {
            if (_useConsole)
                Console.WriteLine(note);
        }
        public int DoIngest()
        {
            int rowCount=0;
            var manager = new SECCrawler.DAL.SECFormsManager();
            secCrawlerData dtst;
            try
            {
                Log("Retrieving a new dataset..");
                dtst = manager.GetFormIngestQueue();
                if (dtst.tblSEC_Forms.Rows.Count == 0)
                {
                    return 0;
                }
                else
                {
                    Log("Files queued to be ingested: " + dtst.tblSEC_Forms.Rows.Count);
                }
            }
            catch(System.Data.SqlClient.SqlException)
            {
                Log("Timeout while retrieving rows. Server must be busy. Client will sleep for a few secconds" );
                System.Threading.Thread.Sleep(10000);
                return 0;
            }
            foreach (var row in dtst.tblSEC_Forms)
            {
                if (row.IsLastIngestedNull())
                {
                    var dtstBlob = manager.GetFormBlob(row.FormID);
                    var rowBlob = dtstBlob.tblSEC_FormsLocalBLOB.FindByFormID(row.FormID);
                    if (rowBlob == null)
                    {
                        rowBlob = dtstBlob.tblSEC_FormsLocalBLOB.NewtblSEC_FormsLocalBLOBRow();
                        rowBlob.FormID = row.FormID;
                        rowBlob.Text = string.Empty;
                        dtstBlob.tblSEC_FormsLocalBLOB.AddtblSEC_FormsLocalBLOBRow(rowBlob);
                    }
                    if (ReadFile(row, rowBlob))
                    {
                        manager.Save(dtstBlob);
                        manager.Save(dtst);
                        rowCount++;
                    }
                }
                if (_useConsole && Console.KeyAvailable) break;

            }
            Log("Saving any pending changes to the last dataset.");
            manager.Save(dtst);
            return rowCount;
        }
        public bool ReadFile(secCrawlerData.tblSEC_FormsRow form,secCrawlerData.tblSEC_FormsLocalBLOBRow blob)
        {
            string fullPath = Path.Combine(_basePath, form.FormPartialURL.Replace(@"/", @"\"));
            if (File.Exists(fullPath))
            {
                try
                {
                    Log("Reading file:" + fullPath);
                    blob.Text = File.ReadAllText(fullPath);
                    form.LastIngested = DateTime.Now;
                    return true;

                }
                catch (Exception)
                {
                    Log("File was in use, skipping:"+fullPath);
                    form.SetLastIngestedNull();
                }
            }
            else
            {
                Log("File not found:" + fullPath);
                if (( DateTime.Now - form.LastDownloaded).TotalMinutes >= 20)
                {
                    Log("File has been sent to download queue.");
                    form.SetLastDownloadedNull();
                }
                else
                {
                    Log("The last download attempt is " + ( DateTime.Now-form.LastDownloaded ).TotalMinutes.ToString() + " minutes old. Not retrying yet.");
                }
                form.SetLastIngestedNull();
            }
            return false;
        }
    }
}
