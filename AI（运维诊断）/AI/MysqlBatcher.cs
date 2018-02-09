using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using UCFAR.Util.Dapper;

namespace AI
{
   public class MysqlBatcher
    {
        private readonly IDapperProvider _db;

        public MysqlBatcher(IDapperProvider db)
        {
            this._db = db;
        }

        private MySqlConnection Connection => _db.Mysql;
        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="sqlStringList">多条SQL语句</param>
        public  void ExecuteSqlTran(List<string> sqlStringList)
        {
          using (MySqlConnection conn = Connection)
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand {Connection = conn};
                MySqlTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < sqlStringList.Count; n++)
                    {
                        string strsql = sqlStringList[n];
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                        }
                        if (n > 0 && (n % 500 == 0 || n == sqlStringList.Count - 1))
                        {
                            tx.Commit();
                            tx = conn.BeginTransaction();
                            //原本是直接下面统一提交，听从sp1234的意见，就在这里重启事务，不知道这样写会不会/好，不过我这些写运行起来好像没问题。
                        }
                    }
                    //tx.Commit();
                }
                catch (MySqlException e)
                {
                    tx.Rollback();
                    throw new Exception(e.Message);
                }
            }
        }
    }
}
