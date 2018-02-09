using System;
using Dapper;

namespace AiWin_API
{
    /// <summary>
    ///     描 述：系统日志
    /// </summary>
    public class LogService
    {

        /// <summary>
        ///     写日志
        /// </summary>
        /// <param name="logEntity">对象</param>
        public static void WriteLog(MBaseLog logEntity)
        {
            logEntity.OperateTime = DateTime.Now;
            logEntity.DeleteMark = false;

            logEntity.IpAddressName = "内网";

            using (var conn = DbContext.Open())
            {
                conn.Execute(@"INSERT INTO Base_Log
        (OperateTime,OperateAccount,OperateTypeId,OperateType,IPAddress,IPAddressName,Browser,ExecuteResult,ExecuteResultJson,DeleteMark)
    VALUES
        (@OperateTime,@OperateAccount,@OperateTypeId,@OperateType,@IPAddress,@IPAddressName,@Browser,@ExecuteResult,@ExecuteResultJson,@DeleteMark)",
                    logEntity);
                conn.Close();
                conn.Dispose();

            }
        }
    }
}