using System.Collections.Generic;
using System;
using SECCrawler.DAL;
namespace SECCrawler.FTPClient
{
    public class Downloader
    {
        private readonly string _basePath;
        private readonly string _remotePath;
        private readonly bool _useConsole;
        public Downloader(bool useConsole)
        {
            var configReader = new System.Configuration.AppSettingsReader();
            _basePath = (string)configReader.GetValue("DataStorePath", string.Empty.GetType());
            _remotePath = (string)configReader.GetValue("FTPServerPath", string.Empty.GetType());
            _useConsole = useConsole;
        }
        protected void Log(string note)
        {
            if (_useConsole)
                Console.WriteLine(note);
        }
        public int ProcessQueue()
        {
            var itemsProcessed = 0;           
            try
            {
                var manager = new SECFormsManager();
                var dtst = manager.GetFormDownloadQueue();
                Log("Read" + dtst.tblSEC_Forms.Rows.Count);
                var ftpClient = new FtpClient(_remotePath,_basePath);
                //to prevent concurrency errors, mark as downloaded immediately:
                foreach (var form in dtst.tblSEC_Forms)
                {
                    form.LastDownloaded = DateTime.Now;
                }
                manager.Save(dtst);
                Log("Blocked rows from parallel processes");
                foreach (var form in dtst.tblSEC_Forms)
                {
                    string error;
                    Log("form type: " + form.FormType + " company name: " + form.CompanyName);
                    ftpClient.Download(form.FormPartialURL, _useConsole, out error);
                    if (error == null) error = string.Empty;
                    if (error == string.Empty)
                    {
                        form.LastDownloaded = DateTime.Now;
                        itemsProcessed ++;
                    }
                    else
                    {
                        Log(error);
                    }
                }
                manager.Save(dtst);
                Log("saved rows");
                dtst.Clear();
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
            return itemsProcessed;
        }
    }
    public class DownloadInitiator
    {
        public int ProcessTickConsoleVersion()
        {
            var downloader = new Downloader(true);
            var errors = downloader.ProcessQueue();
            return errors;
        }
    }
}