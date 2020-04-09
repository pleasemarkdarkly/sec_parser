namespace COI.DAL
{
    public class InvestigationManager:ManagerBase
    {
        private readonly DtstCOITableAdapters.investigationTableAdapter _adapter =
            new DtstCOITableAdapters.investigationTableAdapter();
        private readonly DtstCOITableAdapters.investigated_individualTableAdapter _adapterII =
            new DtstCOITableAdapters.investigated_individualTableAdapter();
        private readonly DtstCOITableAdapters.investigated_companyTableAdapter _adapterIC =
            new DtstCOITableAdapters.investigated_companyTableAdapter();
        public DtstCOI.investigationDataTable GetInvestigations()
        {var table = _adapter.GetData();return table;}
        public DtstCOI.investigated_individualDataTable GetInvestigatedIndividuals()
        {return _adapterII.GetData();}
        public DtstCOI.investigated_individualDataTable GetInvestigatedIndividuals(int investigationId)
        {return _adapterII.GetDataByInvestigationId(investigationId);}
        public DtstCOI.investigated_companyDataTable GetInvestigatedCompanies()
        {return _adapterIC.GetData();}
        public DtstCOI.investigated_companyDataTable GetInvestigatedCompanies(int investigationId)
        {return _adapterIC.GetDataByinvestigationId(investigationId);}
        public void Save(DtstCOI.investigationDataTable table)
        {_adapter.Update(table);}
        public void Save(DtstCOI.investigated_individualDataTable table)
        {_adapterII.Update(table);}
        public void Save(DtstCOI.investigated_companyDataTable table)
        { _adapterIC.Update(table); }
        #region sprocs
        public int PopulateInvestigatedCompaniesWith2OrMoreIndividualsByInvestigationID(int investigationId)
        {var dal = new DtstCOIsprocsTableAdapters.QueriesTableAdapter();
         return dal.SP_populate_investigated_companies_by2ormore_individuals(investigationId);}
        public void PopulateInvestigatedCompaniesByIndividuals(int investigationID)
        {var dal = new DtstCOIsprocsTableAdapters.QueriesTableAdapter();
            dal.SP_populate_investigated_companies_by_individuals(investigationID);}
        #endregion
    }
}
