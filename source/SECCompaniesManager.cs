using System.Data.SqlClient;
using System.Data;
namespace SECCrawler.DAL
{
    public class SECCompaniesManager:ManagerBase
    {
        public secCrawlerData.tblSEC_CompanyNamesDataTable GetCompaniesByNameSearc(string pattern)
        {
            var adapter = new secCrawlerDataTableAdapters.tblSEC_CompanyNamesTableAdapter();
            var table = adapter.GetDataByNameSearch(pattern);
            return table;
        }

        public int AddInvestigatedCompaniesByName(string name, int investigationID, string foundVia)
        {
            int added = 0;
            var coneX = new SqlConnection(this.GetConnectionString());
            var command = new SqlCommand("[spCOI_insertInvestigatedCompany] @name ,@investigationID,@foundVia", coneX);
            var nameParam = new SqlParameter("@name", SqlDbType.VarChar, 100) { Value = name };
            var iidParam = new SqlParameter("@investigationID", SqlDbType.Int) { Value = investigationID };
            var foundParam = new SqlParameter("@foundVia", SqlDbType.VarChar, 100) { Value = foundVia };
            command.Parameters.Add(nameParam);
            command.Parameters.Add(iidParam);
            command.Parameters.Add(foundParam);
            command.Prepare();
            added = (int)command.ExecuteScalar();
            return added;
        }
        public secCrawlerData.tblCOI_InvestigationCompanyDataTable GetInvestigatedCompanies()
        {
            var adapter = new secCrawlerDataTableAdapters.tblCOI_InvestigationCompanyTableAdapter();
            var table = adapter.GetData();
            return table;
        }
        public secCrawlerData.tblSEC_CompanyNamesDataTable GetInvestigatedCompanyNames()
        {
            var adapter = new secCrawlerDataTableAdapters.tblSEC_CompanyNamesTableAdapter();
            var table = adapter.GetDataByInvestigatedCompanies();
            return table;
        }
    }
}
