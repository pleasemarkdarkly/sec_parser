using System.Data;
using System.Data.SqlClient;
using System;
namespace COI.DAL
{
    public class XMLManager:ManagerBase
    {
        public string GetLinks(int investigationID)
        {
            var connection = GetOpenConnection();
            var command = new SqlCommand("[SP_export_XML] @investigation_id", connection);
            var parameter=command.Parameters.Add(new SqlParameter("@investigation_id", SqlDbType.Int));
            parameter.Value = investigationID;
            command.Prepare();
            var o=command.ExecuteScalar();
            if (o!= DBNull.Value)
                return (string) o;
            return string.Empty;
        }
    }
}
