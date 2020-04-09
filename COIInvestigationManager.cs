namespace SECCrawler.DAL
{
    public class COIInvestigationManager:ManagerBase
    {
        private readonly secCrawlerDataTableAdapters.tblCOI_InvestigationCompanyTableAdapter
            _adapterIc = new secCrawlerDataTableAdapters.tblCOI_InvestigationCompanyTableAdapter();
        private readonly secCrawlerDataTableAdapters.tblCOI_InvestigationTableAdapter
            _adapterI = new secCrawlerDataTableAdapters.tblCOI_InvestigationTableAdapter();
        public secCrawlerData.tblCOI_InvestigationDataTable GetInvestigations()
        {
            var table = _adapterI.GetData();
            return table;
        }
        public secCrawlerData.tblCOI_InvestigationCompanyDataTable GetCompaniesByInvestigation(int investigationID)
        {
            var table = _adapterIc.GetDataByInvestigation(investigationID);
            return table;
        }
        public secCrawlerData.tblCOI_InvestigationCompanyDataTable GetCompaniesInvestigated()
        {
            var table = _adapterIc.GetData();
            return table;
        }
        public void Save(secCrawlerData.tblCOI_InvestigationCompanyDataTable table)
        {
            _adapterIc.Update(table);
        }
    }
}
