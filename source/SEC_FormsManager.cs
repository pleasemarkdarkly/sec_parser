namespace SECCrawler.DAL
{
    using System;
    public class SECFormsManager
    {
        public secCrawlerData GetFormDownloadQueue()
        {
            var dtst = new secCrawlerData();
            var adapter = new secCrawlerDataTableAdapters.tblSEC_FormsTableAdapter();
            adapter.Fill(dtst.tblSEC_Forms);
            return dtst;
        }
        public secCrawlerData GetFormIngestQueue()
        {
            var dtst = new secCrawlerData();
            var adapter = new secCrawlerDataTableAdapters.tblSEC_FormsToIngestTableAdapter();
            adapter.Fill(dtst.tblSEC_FormsToIngest);
            return dtst;
        }
        public secCrawlerData GetFormBlob(Guid formId)
        {
            var dtst = new secCrawlerData();
            var adapter = new secCrawlerDataTableAdapters.tblSEC_FormsLocalBLOBTableAdapter();
            adapter.Fill(dtst.tblSEC_FormsLocalBLOB, formId);
            return dtst;
        }
        public void Save(secCrawlerData dtst)
        {
            if (dtst.tblSEC_Forms.IsInitialized)
            {
                var adapter = new secCrawlerDataTableAdapters.tblSEC_FormsTableAdapter();
                adapter.Update(dtst.tblSEC_Forms);
            }
            if (dtst.tblSEC_FormsToIngest.IsInitialized)
            {
                var adapter = new secCrawlerDataTableAdapters.tblSEC_FormsToIngestTableAdapter();
                adapter.Update(dtst.tblSEC_FormsToIngest);
            }

        }
    }
}