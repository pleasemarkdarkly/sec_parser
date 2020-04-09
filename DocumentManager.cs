using System;
using COI.DAL.DtstCOITableAdapters;

namespace COI.DAL
{
    public class DocumentManager:ManagerBase
    {
        private readonly documentTableAdapter _adapter=new documentTableAdapter();
        public  DtstCOI.documentDataTable GetDocuments(){return _adapter.GetData();}
        public DtstCOI.documentDataTable GetDocument(int id){return _adapter.GetDataByID(id);}
        public DtstCOI.documentDataTable GetDocument(Guid formId) { return _adapter.GetDataByExternalGuid(formId); }
        public DtstCOI.documentDataTable CreateOrGetDocumentByURL(string url, int sourceId, DateTime date, int sizeKb, string keywrds, string type)
        {return _adapter.CreateAndGetDataByURL(sourceId, "link", url, date, sizeKb, keywrds, type);}
        public void Save (DtstCOI.documentDataTable table){_adapter.Update(table);}
        #region snippets
        //snippets-external
        public DtstCOIviews.vwSEC_SnippetDataTable GetSECSnippetsByType(string criteria)
        {return new DtstCOIviewsTableAdapters.vwSEC_SnippetTableAdapter().GetDataByCriteria(criteria);}
        //snippets-internal
        private DtstCOITableAdapters.snippetTableAdapter _dalS;
        private DtstCOITableAdapters.snippetTableAdapter DalS{get
        {if (_dalS==null) _dalS=new snippetTableAdapter();
            return _dalS;}}
        public DtstCOI.snippetDataTable GetSnippets(int docID)
        {return DalS.GetDataByDocID(docID);}
        public DtstCOI.snippetDataTable GetSnippets(string coName)
        { return DalS.GetDataByCoName(coName); }
        public void Save(DtstCOI.snippetDataTable snippetTable)
        {DalS.Update(snippetTable);}
        #endregion
    }
}
