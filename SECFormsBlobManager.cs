using System;
namespace SECCrawler.DAL
{
    public class SECFormsBlobManager:ManagerBase
    {
        public string GetFormBlob(Guid formId)
        {
            var adapter = new secCrawlerDataTableAdapters.tblSEC_FormsLocalBLOBTableAdapter();
            var table=adapter.GetData(formId);
            return table.Rows.Count == 1 ? table[0].Text : "not found";
        }

    }
}
