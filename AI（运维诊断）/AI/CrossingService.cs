using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using UCFAR.Util.Dapper;

namespace AI
{
    public class CrossingService
    {
        private readonly IDapperProvider _db;
        public CrossingService(IDapperProvider db)
        {
            this._db = db;
        }
        private IDbConnection Connection => _db.Mysql;
        public  dynamic GetIP_IsOnline(string crsCode)
        {
          
            using (var conn = Connection)
            {
                conn.Open();
                var sql = @"SELECT CrsIp,CrsName FROM crossing where crsCode=@crsCode";
                var result = conn.QueryFirstOrDefault(sql, new { crsCode });
                conn.Close();
                conn.Dispose();
                return result;
            }



        }
  

    }
}
