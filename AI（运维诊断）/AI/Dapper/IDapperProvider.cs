using System.Data;
using MySql.Data.MySqlClient;

namespace UCFAR.Util.Dapper
{
    public interface IDapperProvider
    {

        MySqlConnection Mysql { get; }
    }
}
