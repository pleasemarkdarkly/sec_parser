using System;
using System.Web;
using COI.DAL.DtstCOIsprocsTableAdapters;
using COI.DAL.DtstCOITableAdapters;
namespace COI.DAL
{
    public class LinkManager:ManagerBase
    {
        private DtstCOIsprocsTableAdapters.QueriesTableAdapter _sprocAdapter;
        private DtstCOIsprocsTableAdapters.QueriesTableAdapter SprocAdapter{
            get{if (_sprocAdapter==null) _sprocAdapter=new QueriesTableAdapter();
                return _sprocAdapter;}}
        #region companyCompanyLinks
        private company_company_linkTableAdapter CoCoAdapter
        {get{if (_coCoAdapter ==null)_coCoAdapter = new company_company_linkTableAdapter();
             return _coCoAdapter;}}
        private company_company_linkTableAdapter _coCoAdapter;
        public DtstCOI.company_company_linkDataTable GetCoCoLinks()
        { return CoCoAdapter.GetData(); }
        public DtstCOI.company_company_linkDataTable GetCoCoLinks(string companyName)
        { return CoCoAdapter.GetDataByCompanyName(companyName); }
        public DtstCOI.company_company_linkDataTable GetCoCoLinks(int investigationID)
        { return CoCoAdapter.GetDataByInvestigation(investigationID); }
        public DtstCOI.company_company_linkDataTable GetCoCoLinks(string companyName, string linkType)
        { return CoCoAdapter.GetDataByCoNameAndLinkType(companyName,linkType); }
        public DtstCOI.company_company_linkDataTable GetLinksByCoDateAndLinkType(string companyName1, string companyName2, int docId, string linkType)
        { return CoCoAdapter.GetDataByNamesTypeAndDocId(companyName1, companyName2, docId, linkType); }
        public void Save(DtstCOI.company_company_linkDataTable table)
        { CoCoAdapter.Update(table); }
        public string GetPreviousLinks(string name, bool invertDirection, string linkBase)
        {
            var s = string.Empty;
            var table = GetCoCoLinks(name, "previous");
            foreach (var coCoLink in table)
            {
                if ((coCoLink.company_name == name & invertDirection) || (coCoLink.company_name2 == name & !invertDirection))
                {
                    var name2 = invertDirection ? coCoLink.company_name2 : coCoLink.company_name;
                    var url = linkBase + HttpUtility.UrlEncode(name2);
                    var s1= string.Format("<a href=\"{1}\">{0}</a><br />", name2, url);
                    if (!s.Contains(s1)) s += s1;
                }
            }
            return s;
        }
        public void CreateOrUpdateCoCoLink(string companyName, string companyName2, int? sourceID, int? supportingDocID, string linkType, string linkTitle, DateTime? linkDate)
        {SprocAdapter.SP_createOrUpdateCoCoLink(companyName, companyName2, sourceID, supportingDocID, linkType,linkTitle, linkDate);}
        #endregion
        #region individualCompanyLinks
        private readonly individual_company_linkTableAdapter _adapter=
            new individual_company_linkTableAdapter();
        public DtstCOI.individual_company_linkDataTable GetIndividualCompanyLink()
        {return _adapter.GetData();}
        public DtstCOI.individual_company_linkDataTable GetIndividualCompanyLink(int investigationID)
        {return _adapter.GetDataByInvestigationId(investigationID);}
        public DtstCOI.individual_company_linkDataTable GetIndividualCompanyLink(string companyName)
        {return _adapter.GetDataByCompanyName(companyName);}
        public DtstCOI.individual_company_linkDataTable GetIndividualCompanyLinkByIndividual(string individualName)
        { return _adapter.GetDataByIndividualName(individualName); }
        public DtstCOI.individual_company_linkDataTable CreateOrUpdate(int investigationID, string companyName, string individualName, string individualAlias, int sourceID, int docID, string linkType, DateTime linkDate, string position, DateTime linkDateEnd)
        {return _adapter.GetDataByCreateOrUpdteLink(investigationID, companyName, individualName,individualAlias, sourceID,docID,linkType,linkDate,position,linkDateEnd);}
        public DtstCOI.individual_company_linkDataTable CreateOrUpdate(int investigationID, string companyName, string individualName,string individualAlias, int sourceID, int docID, string linkType, DateTime linkDate, string position)
        {return _adapter.GetDataByCreateOrUpdteLink(investigationID, companyName, individualName,individualAlias, sourceID, docID, linkType, linkDate, position, null);}
        public void  Save(DtstCOI.individual_company_linkDataTable table)
        {_adapter.Update(table); }
        #endregion
        #region individualIndividualLinks
        private individual_individual_linkTableAdapter Iil
        {get{if (_iil==null) _iil=new individual_individual_linkTableAdapter();
             return _iil;}}
        private individual_individual_linkTableAdapter _iil;
        public DtstCOI.individual_individual_linkDataTable GetIILByName(string name)
        {return Iil.GetDataByIndividualName(name);}
        public DtstCOI.individual_individual_linkDataTable GetIILByAliasSearch(string searchPattern)
        {return Iil.GetDataByAliasSearch('%' + searchPattern + '%'); }
        public void Save(DtstCOI.individual_individual_linkDataTable table){Iil.Update(table);}
        #endregion
        #region others
        public DtstCOIsprocs.Vw_link_typesDataTable GetAllLinkTypes()
        {
            var adap=new DtstCOIsprocsTableAdapters.Vw_link_typesTableAdapter();
            return adap.GetData();
        }
        public DtstCOIsprocs.Vw_link_titlesDataTable GetAllPositionsOrTitles()
        {
            var dal = new DtstCOIsprocsTableAdapters.Vw_link_titlesTableAdapter();
            return dal.GetData();
        }
        #endregion
        #region mergedLinks
        private DtstCOIsprocsTableAdapters.sp_GetLinksBySubjectTableAdapter _adapSpLinks;
        private DtstCOIsprocsTableAdapters.sp_GetLinksBySubjectTableAdapter AdapSpLinks
        {get{if (_adapSpLinks==null) _adapSpLinks=new sp_GetLinksBySubjectTableAdapter();
            return _adapSpLinks;}}
        public DtstCOIsprocs.sp_GetLinksBySubjectDataTable GetLinksBySubject(string subject, int type)
        {return AdapSpLinks.GetData(subject, type);}
        public DtstCOIsprocs.sp_GetLinksBySubjectDataTable GetLinksBySubject(string subject, int type,int investigationID)
        { return AdapSpLinks.GetDataBySubjectAndInvestigation(subject, type,investigationID); }
        public DtstCOIsprocs.sp_GetLinksBySubjectWithDetailsDataTable GetLinksBySubjectWithDetails(string subject, int type)
        { return new DtstCOIsprocsTableAdapters.sp_GetLinksBySubjectWithDetailsTableAdapter().GetData(subject, type); }
        #endregion
    }
}
