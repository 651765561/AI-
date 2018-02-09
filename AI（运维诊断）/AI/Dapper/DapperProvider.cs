using System;
using System.Data;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace UCFAR.Util.Dapper
{
    public class DapperProvider :IDapperProvider
    {
        /// <summary>
        /// Rediskit options
        /// </summary>
        private readonly DapperOptions _dapperOptions;


        #region ctor
        public DapperProvider(DapperOptions options)
        {
            _dapperOptions = options;
        }

        #endregion
        public MySqlConnection OpenConnection()
        {
            MySqlConnection conn = new MySqlConnection(_dapperOptions.ConnectionString);
            //conn.Open();
            return conn;
        }

        public MySqlConnection Mysql => OpenConnection();
    }
}
