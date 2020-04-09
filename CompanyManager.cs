using System;
using COI.DAL.DtstCOITableAdapters;
namespace COI.DAL
{
    public class CompanyManager : ManagerBase
    {
        #region events
        private readonly company_eventTableAdapter _eventAdapter = new company_eventTableAdapter();
        public DtstCOI.company_eventDataTable GetEvents()
        {
            return _eventAdapter.GetData();
        }
        public DtstCOI.company_eventDataTable GetEventsByCo(string companyName)
        {
            return _eventAdapter.GetDataByCompanyName(companyName);
        }
        public void SaveEvents(DtstCOI.company_eventDataTable table)
        {
            _eventAdapter.Update(table);
        }
        public void CreateCompanyEvent(string companyName, string eventType, string eventDescription, DateTime eventDate, Guid formID, string keywords, string snippet)
        {
            var adap = new DtstCOIsprocsTableAdapters.QueriesTableAdapter();
            adap.SP_createCompanyEvent(companyName, eventType, eventDescription, eventDate, formID, keywords, snippet);
        }
        public DtstCOIsprocs.SP_getEventTypesDataTable GetEventTypes()
        {
            var sprocAdapter = new DtstCOIsprocsTableAdapters.SP_getEventTypesTableAdapter();
            return sprocAdapter.GetData();
        }

        #endregion
        #region company
        private readonly companyTableAdapter _adapter = new companyTableAdapter();
        public DtstCOI.companyDataTable GetCompanies()
        {
            return _adapter.GetData();
        }
        public DtstCOI.companyDataTable SearchCompanies(string searchPattern)
        {
            return _adapter.GetDataByCompanyNameSearch('%' + searchPattern + '%');
        }
        public void Save(DtstCOI.companyDataTable table)
        { _adapter.Update(table); }
        public DtstCOIsprocs.vwCompaniesKnownDataTable GetCompaniesKnown()
        {
            var adap = new DtstCOIsprocsTableAdapters.vwCompaniesKnownTableAdapter();
            return adap.GetData();
        }
        #endregion
        #region ids
        private company_idTableAdapter _adapIDs;
        private company_idTableAdapter AdapIDs
        {get{if (_adapIDs==null) _adapIDs=new company_idTableAdapter();
            return _adapIDs;}}
        public DtstCOI.company_idDataTable GetCompanyIDs()
        { return AdapIDs.GetData(); }
        public DtstCOI.company_idDataTable GetCompanyIDsByType(string identifierType)
        { return AdapIDs.GetDataByIdentifierType(identifierType); }
        public DtstCOI.company_idDataTable GetCompanyIDs(string companyName)
        {return AdapIDs.GetDataByCoName(companyName);}
        public DtstCOI.company_idDataTable GetCompanyIDs(int infoSource,string identifier)
        { return AdapIDs.GetDataByIdentifier(infoSource,identifier); }
        public DtstCOI.company_idDataTable GetCompanyIdsByQ()
        {return AdapIDs.GetDataByQueue();}        
        public void Save(DtstCOI.company_idDataTable table)
        {AdapIDs.Update(table); }
        public DtstCOI.company_idRow GetOrCreateIdentifier(string companyName, string idType, string iden, int srcID)
        {
            var tble = AdapIDs.GetDataByCoName(companyName);
            var row = tble.FindBycompany_nameidentifier_typeidentifier(companyName, idType, iden);
            if (row==null)
            {
                row = tble.Addcompany_idRow(companyName, srcID, idType, iden);
                AdapIDs.Update(tble);
            }
            return row;
        }
        #endregion
        #region auxiliarsprocs
        public void ConvertIndividualToCompany(string individualName, string coName)
        {
            if (string.IsNullOrEmpty(coName))
                coName = Util.Dbo.FilterName(individualName);
            var adap = new DtstCOIsprocsTableAdapters.QueriesTableAdapter();
            adap.SP_convertIndividual2Company(individualName, coName);
        }
        #endregion
    }
}
