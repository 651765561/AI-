using System;
using System.Data;
using System.Data.SqlClient;

namespace AiWin_New
{
    public class DbContext
    {
        public static string DefaultConnectionString => Config.DefaultConnectionString;
     
        public static string GetSubCode()
        {
            var subCode = Config.GetAppSetting("SubCode");
            return subCode;

        }
 
        public static SqlConnection Open(bool mars = false)
        {
            var cs = DefaultConnectionString;
            if (mars)
            {
                var scsb = new SqlConnectionStringBuilder(cs)
                {
                    MultipleActiveResultSets = true
                };
                cs = scsb.ConnectionString;
            }
            var connection = new SqlConnection(cs);
            connection.Open();
            return connection;
        }
        public SqlConnection GetClosedConnection()
        {
            var conn = new SqlConnection(DefaultConnectionString);
            if (conn.State != ConnectionState.Closed) throw new InvalidOperationException("should be closed!");
            return conn;
        }
   
    }
}
