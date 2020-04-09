using COI.DAL.DtstCOITableAdapters;
namespace COI.DAL
{
    public class SharesManager : ManagerBase
    {
        #region declarationsAndProperties
        private individual_company_sharesTableAdapter _adapter;
        protected individual_company_sharesTableAdapter Adapter
        {get{ if (_adapter == null)_adapter=new individual_company_sharesTableAdapter();
            return _adapter;}}
        private company_shares_tradedTableAdapter _sharesAdapter;
        protected company_shares_tradedTableAdapter SharesAdapter
        { get{if (_sharesAdapter==null)_sharesAdapter=new company_shares_tradedTableAdapter();
            return _sharesAdapter;}}
        #endregion
        #region SharesTradedByIndividual
        public DtstCOI.individual_company_sharesDataTable GetIndividualSharesTraded()
        {
            return Adapter.GetData();
        }
        public DtstCOI.individual_company_sharesDataTable GetIndividualSharesTradedByCo(string companyName)
        {
            return Adapter.GetDataByCoName(companyName);
        }
        public void Save(DtstCOI.individual_company_sharesDataTable table)
        {
            Adapter.Update(table);
        }
        #endregion
        #region SharesTradedByCO
        public DtstCOI.company_shares_tradedDataTable GetSharesTraded()
        {return SharesAdapter.GetData();}
        public DtstCOI.company_shares_tradedDataTable GetSharesTraded(string companyName, int infoSource)
        { return SharesAdapter.GetDataByCoNameAndSource(companyName, infoSource); }
        public DtstCOI.company_shares_tradedDataTable GetSharesTraded(string companyName, int infoSource,int intervalDays)
        { return SharesAdapter.GetDataByCoNameInfoSourceIntervalDays(companyName, infoSource,intervalDays); }
        public void Save(DtstCOI.company_shares_tradedDataTable table)
        {SharesAdapter.Update(table); }
        #endregion
    }
}
