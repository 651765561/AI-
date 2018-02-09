using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Dapper;

namespace AiWinLinux
{
    public class CameraService
    {
        public static List<dynamic> GetIpList(string subCode)
        {
            using (var conn = DbContext.Open())
            {
                var sql = "SELECT a.crsCode,a.cameraCode,a.whichOne,b.crsName,b.lat,b.lng,c.ip from crossingcamera a,crossing b, camera c WHERE (a.crsCode=b.crsCode and a.cameraCode=c.cameraCode) and b.subCode= '"+subCode+"'";

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
                Ping pingSender = new Ping();

                PingReply reply = pingSender.Send(i.ip, 5);//第一个参数为ip地址，第二个参数为ping的时间

                if (reply.Status != IPStatus.Success)//ping不通

                {

                    if (IsExistFault(i.cameraCode))
                    {
                        UpdateOfflineFault(i);
                        // Console.WriteLine($"Ip={i.crsIp}&Msg=fault更新离线状态");
                    }
                    else
                    {
                        InsertOfflineFault(i);
                        // Console.WriteLine($"Ip={i.crsIp}&Msg=fault插入离线状态");
                    }

                }

           

            });

        }
        public static void UpdateOfflineFault(dynamic i)
        {
            using (var conn = DbContext.Open())
            {
                conn.Execute(@"UPDATE fault SET devName=@devName,lat=@lat,lng=@lng,ip=@ip,memo=@memo,level=@level  WHERE faultCode=@faultCode",
                    new { faultCode = i.cameraCode, devName = i.crsName, lat = i.lat, lng = i.lng, ip = i.ip, memo = "摄像机("+i.whichOne+")网络故障", level = 3 });
                conn.Close();
                conn.Dispose();
            }

        }
        public static void InsertOfflineFault(dynamic i)
        {

            using (var conn = DbContext.Open())
            {
                conn.Execute(@"INSERT INTO fault
        (faultCode,devName,lat,lng,ip,memo,level,faultTime)
    VALUES
        (@faultCode,@devName,@lat,@lng,@ip,@memo,@level,@faultTime)",
                    new { faultCode = i.cameraCode, devName = i.crsName, lat = i.lat, lng = i.lng, ip = i.ip, memo = "摄像机(" + i.whichOne + ")网络故障", level = 3, faultTime = DateTime.Now });
                conn.Close();
                conn.Dispose();
            }

        }
        public static bool IsExistFault(string faultCode)
        {
            using (var conn = DbContext.Open())
            {
                var a = conn.ExecuteScalar<int>(@"select count(*) from fault where faultCode=@faultCode",
                            new { faultCode }) > 0;
                conn.Close();
                conn.Dispose();
                return a;
            }


        }
    }
}
