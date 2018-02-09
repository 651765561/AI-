using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace AiWinLinux
{
    public class EleServices
    {
        public static List<dynamic> GetIpList(string subCode)
        {
            using (var conn = DbContext.Open())
            {
                var sql = "SELECT a.crsCode,a.acb,b.crsName,b.lat,b.lng,b.crsIp from crossingstatus a,crossing b WHERE a.acb=0 and (a.crsCode=b.crsCode) and b.subCode= '" + subCode + "'";

                var result = conn.Query<dynamic>(sql);
                conn.Close();
                conn.Dispose();
                return result.ToList();
            }



        }
        public static void InSertDb(List<dynamic> para)
        {

            // System.Threading.Tasks.Parallel.For - for 循环的并行运算
            Parallel.ForEach(para, new ParallelOptions() { MaxDegreeOfParallelism = 4 }, (i, parallelLoopStat) =>
            {
                
                    if (IsExistFault(i.crsCode))
                    {
                        UpdateOfflineFault(i);
                        // Console.WriteLine($"Ip={i.crsIp}&Msg=fault更新离线状态");
                    }
                    else
                    {
                        InsertOfflineFault(i);
                        // Console.WriteLine($"Ip={i.crsIp}&Msg=fault插入离线状态");
                    }
            });

        }
        public static bool IsExistFault(string faultCode)
        {
            using (var conn = DbContext.Open())
            {
                var a = conn.ExecuteScalar<int>(@"select count(*) from fault where faultCode=@faultCode and faultType=@faultType",
                            new { faultCode= faultCode, faultType=1 }) > 0;
                conn.Close();
                conn.Dispose();
                return a;
            }


        }
        public static void UpdateOfflineFault(dynamic i)
        {
            using (var conn = DbContext.Open())
            {
                conn.Execute(@"UPDATE fault SET devName=@devName,lat=@lat,lng=@lng,ip=@ip,memo=@memo,level=@level,faultType=@faultType  WHERE faultCode=@faultCode and faultType=@faultType",
                    new { faultCode = i.crsCode, devName = i.crsName, lat = i.lat, lng = i.lng, ip = i.crsIp, memo = "供电故障", level = 3,faultType=1 });
                conn.Close();
                conn.Dispose();
            }

        }
        public static void InsertOfflineFault(dynamic i)
        {

            using (var conn = DbContext.Open())
            {
                conn.Execute(@"INSERT INTO fault
        (faultCode,devName,lat,lng,ip,memo,level,faultTime,faultType)
    VALUES
        (@faultCode,@devName,@lat,@lng,@ip,@memo,@level,@faultTime,@faultType)",
                    new { faultCode = i.crsCode, devName = i.crsName, lat = i.lat, lng = i.lng, ip = i.crsIp, memo = "供电故障", level = 3, faultTime = DateTime.Now,faultType=1 });
                conn.Close();
                conn.Dispose();
            }

        }
    }
}
