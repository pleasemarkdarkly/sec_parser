namespace SECCrawler.DAL
{
    using System;
    public class SECFormsSnippetManager
    {
        private readonly secCrawlerDataTableAdapters.tblSEC_Forms_snippetTableAdapter 
            _adapter = new secCrawlerDataTableAdapters.tblSEC_Forms_snippetTableAdapter();
        public secCrawlerData.tblSEC_Forms_snippetDataTable GetFormSnippets(Guid formId,int sequence)
        {
            var table = _adapter.GetDataByFormIdAndSubDocSequence(formId,sequence);
            return table;
        }
        public void Save(secCrawlerData.tblSEC_Forms_snippetDataTable table)
        {
            _adapter.Update(table);
        }

    }
}
