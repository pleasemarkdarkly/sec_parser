using SECCrawler.DAL.secCrawlerDataTableAdapters;

namespace SECCrawler.DAL
{
    public class SavedSearchManager : ManagerBase
    {
        readonly tblCOI_savedSearchTableAdapter _manager=new tblCOI_savedSearchTableAdapter();
        public secCrawlerData.tblCOI_savedSearchDataTable GetSearches()
        {
            var table = _manager.GetData();
            return table;
        }
        public secCrawlerData.tblCOI_savedSearchDataTable GetSearches(string type)
        {
            var table = _manager.GetDataByType(type);
            return table;
        }
        public void Save(secCrawlerData.tblCOI_savedSearchDataTable table)
        {
            _manager.Update(table);
        }
    }
}
