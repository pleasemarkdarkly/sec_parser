using System.Configuration;
using System.Data.SqlClient;
namespace COI.DAL
{
    public class ManagerBase
    {
        protected AppSettingsReader ConfigReader = new AppSettingsReader();
        public SqlConnection GetOpenConnection()
        {
            var cs = GetConnectionString();
            var cn = new SqlConnection(cs);
            cn.Open();
            return cn;
        }
        public string GetConnectionString()
        {
            var cs = (string)ConfigReader.GetValue("CornerOfficeInvestigationsConnectionString", string.Empty.GetType());
            return cs;
        }

        public ManagerBase()
        {

        }
    }
}
