using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace AiWinLinux
{
    public class AutoRepairService
    {
        public static List<dynamic> GetIpList(string subCode)
        {
            using (var conn = DbContext.Open())
            {
                var sql = @"SELECT * FROM crossing  where subCode=@SubCode";

                var result = conn.Query<dynamic>(sql, new { subCode });
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
                PingOptions pingOption = new PingOptions();
                pingOption.DontFragment = true;

                string data = "sendData:goodgoodgoodgoodgoodgood";
                byte[] buffer = Encoding.ASCII.GetBytes(data);
                int timeout = 120;
                PingReply reply = pingSender.Send(i.crsIp, timeout, buffer);
                
                //Ping pingSender = new Ping();

                //PingReply reply = pingSender.Send(i.crsIp, 2);//第一个参数为ip地址，第二个参数为ping的时间

                if (reply.Status != IPStatus.Success)//ping不通

                {

                    if (IsExist(i.crsCode))//存在更新为0,最新故障时间
                    {
                        UpdateOff(i);
                        
                    }
                    else
                    {
                        InsertOff(i);//插入初次故障
                      
                    }

                }
                else//自动修复好了
                {
                    if (IsExist(i.crsCode))//存在更新自动修复次数+1,同时锁上
                    {
                        bool islock = IsLock(i.crsCode);
                        if (!islock)
                        {
                            UpdateIsLock(i);
                        }
                       

                    }

                }



            });

        }
        public static void UpdateIsLock(dynamic i)
        {
            using (var conn = DbContext.Open())
            {
                conn.Execute(@"UPDATE autorepair SET repairTime=@repairTime,isLock=1,repairSum=repairSum+1 WHERE faultCode=@faultCode",
                    new { faultCode = i.crsCode, repairTime = DateTime.Now });
                conn.Close();
                conn.Dispose();
            }

        }
        public static bool IsLock(string faultCode)
        {
            using (var conn = DbContext.Open())
            {
                var a = conn.QueryFirstOrDefault<dynamic>(@"select isLock from autorepair where faultCode=@faultCode",
                            new { faultCode }).isLock;
                conn.Close();
                conn.Dispose();
                return a;
            }


        }
        public static bool IsExist(string faultCode)
        {
            using (var conn = DbContext.Open())
            {
                var a = conn.ExecuteScalar<int>(@"select count(*) from autorepair where faultCode=@faultCode",
                            new { faultCode }) > 0;
                conn.Close();
                conn.Dispose();
                return a;
            }


        }
        public static void UpdateOff(dynamic i)
        {
            using (var conn = DbContext.Open())
            {
                conn.Execute(@"UPDATE autorepair SET newFaultTime=@newFaultTime,isLock=@isLock WHERE faultCode=@faultCode",
                    new { faultCode = i.crsCode, isLock =0, newFaultTime = DateTime.Now});
                conn.Close();
                conn.Dispose();
            }

        }
        public static void InsertOff(dynamic i)
        {

            using (var conn = DbContext.Open())
            {
                conn.Execute(@"INSERT INTO autorepair
        (faultCode,faultTime,newFaultTime,repairSum,isLock)
    VALUES
        (@faultCode,@faultTime,@newFaultTime,@repairSum,@isLock)",
                    new { faultCode = i.crsCode, faultTime =DateTime.Now, newFaultTime =DateTime.Now, repairSum=0, isLock=0 });
                conn.Close();
                conn.Dispose();
            }

        }
    }
}
