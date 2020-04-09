using COI.DAL.DtstCOITableAdapters;
namespace COI.DAL
{
    public class InfoSourceManager:ManagerBase
    {
        private readonly information_sourceTableAdapter _adapter
            =new information_sourceTableAdapter();
        private readonly infoSource_pagesTableAdapter _pagesAdapter
            =new infoSource_pagesTableAdapter();
        public DtstCOI.information_sourceDataTable GetInfoSources()
        {
            return _adapter.GetData();
        }
        public DtstCOI.infoSource_pagesDataTable GetPages()
        {
            return _pagesAdapter.GetData();
        }
        public DtstCOI.infoSource_pagesDataTable GetPagesByInfoSourceId(int infoSourceId)
        {
            return _pagesAdapter.GetDataBySourceId(infoSourceId);
        }
    }
}
