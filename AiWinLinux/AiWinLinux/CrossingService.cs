using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace AiWinLinux
{
    public class CrossingService
    {

        public static dynamic GetIP_IsOnline(string crsCode)
        {
            using (var conn = DbContext.Open())
            {
                var sql = @"SELECT CrsIp,CrsName FROM crossing where crsCode=@crsCode";
                var result = conn.QueryFirstOrDefault(sql, new { crsCode });
                conn.Close();
                conn.Dispose();
                return result;
            }



        }
        public static List<dynamic> GetIpList(string subCode)
        {
            using (var conn = DbContext.Open())
            {
                var sql = @"SELECT * FROM crossing  where SubCode=@SubCode";

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
                Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ipAddress = IPAddress.Parse(i.crsIp);
                bool conn = TcpConn.ConnectWithTimeout(clientSocket, new IPEndPoint(ipAddress, 5000), 2000); //超时2秒

                if (conn)
                {
                    // Log.Info($"Ip={i.crsIp}&Msg=Socket连接成功");
                    try
                    {
                        // Log.Info($"Ip={i.crsIp}&Msg=获取设备状态...");
                        string send = "EF EF 04 14 00 F6";
                        var msg = Common.StrToHexByte(send);
                        try
                        {
                            clientSocket.Send(msg); // Sends some data  BitConverter.ToString(msg, 0))
                        }
                        catch (Exception e)
                        {
                            // clientSocket.Shutdown(SocketShutdown.Both);//可去除
                            clientSocket.Dispose();
                            Console.WriteLine($"Ip={i.crsIp}&Msg=SendData:{e.Message}");

                            // UpdateIsOnline(false, i.crsCode);
                            //Console.WriteLine($"Ip={i.crsIp}&Msg=Crossing更新离线状态成功");
                            var model = GetMDevstatusByCrsCode(i.crsCode);
                            if (IsExist(i.crsCode))
                            {
                                UpdateOnline(model);
                               // Console.WriteLine($"Ip={i.crsIp}&Msg=crossingstatus更新离线成功");
                            }
                            else
                            {
                                InsertOnline(model);
                               // Console.WriteLine($"Ip={i.crsIp}&Msg=crossingstatus插入离线状态成功");
                            }
                            //continue;
                            // parallelLoopStat.Break();
                            return;//不加return，可能会发生该进程资源未释放。
                        }
                        var buffer = new byte[103];
                        try
                        {

                            clientSocket.Receive(buffer); // Receives some data back (blocks execution)
                        }
                        catch (Exception e)
                        {
                            // clientSocket.Shutdown(SocketShutdown.Both);//可去除

                            clientSocket.Dispose();
                            Console.WriteLine($"Ip={i.crsIp}&Msg=Receive:{e.Message}");

                            // UpdateIsOnline(false, i.crsCode);
                            // Console.WriteLine($"Ip={i.crsIp}&Msg=Crossing更新离线状态成功");
                            var model = GetMDevstatusByCrsCode(i.crsCode);
                            if (IsExist(i.crsCode))
                            {
                                UpdateOnline(model);
                               // Console.WriteLine($"Ip={i.crsIp}&Msg=crossingstatus更新离线成功");
                            }
                            else
                            {
                                InsertOnline(model);
                              //  Console.WriteLine($"Ip={i.crsIp}&Msg=crossingstatus插入离线状态成功");
                            }
                            //continue;
                            // parallelLoopStat.Break();
                            return;//不加return，可能会发生该进程资源未释放。
                        }
                        try
                        {
                            var response = BitConverter.ToString(buffer, 0);
                            // Receives some data back (blocks execution)  AB-CD-09-08-2A-E8-07-00-00-00-A243
                            var dataArr = response.Replace("-", "");
                            var byteDataArr = Common.StrToHexByte(dataArr);

                            //8421码按位与操作
                            var cameraTwo = (byteDataArr[4] & 0x08) == 0x08;
                            var cameraOne = (byteDataArr[4] & 0x04) == 0x04;
                            var fot = (byteDataArr[4] & 0x02) == 0x02;
                            var router = (byteDataArr[4] & 0x01) == 0x01;

                            var fan = (byteDataArr[4] & 0x10) == 0x10;
                            var acb = (byteDataArr[4] & 0x20) == 0x20;
                            var cameraThree = (byteDataArr[4] & 0x40) == 0x40;
                            var tempAlarm = (byteDataArr[5] & 0x01) == 0x01;
                            var esAlarm = (byteDataArr[5] & 0x02) == 0x02;

                            byte[] coolingTempbyte = new byte[4];
                            coolingTempbyte[0] = byteDataArr[6];
                            coolingTempbyte[1] = byteDataArr[7];
                            coolingTempbyte[2] = byteDataArr[8];
                            coolingTempbyte[3] = byteDataArr[9];
                            var coolingTemp = BitConverter.ToSingle(coolingTempbyte, 0);
                            if (float.IsNaN(coolingTemp))
                            {
                                coolingTemp = 0f;
                            }

                            byte[] alarmTempbyte = new byte[4];
                            alarmTempbyte[0] = byteDataArr[10];
                            alarmTempbyte[1] = byteDataArr[11];
                            alarmTempbyte[2] = byteDataArr[12];
                            alarmTempbyte[3] = byteDataArr[13];
                            var alarmTemp = BitConverter.ToSingle(alarmTempbyte, 0);
                            if (float.IsNaN(alarmTemp))
                            {
                                alarmTemp = 0f;
                            }
                            byte[] caseTempbyte = new byte[4];
                            caseTempbyte[0] = byteDataArr[14];
                            caseTempbyte[1] = byteDataArr[15];
                            caseTempbyte[2] = byteDataArr[16];
                            caseTempbyte[3] = byteDataArr[17];
                            var caseTemp = BitConverter.ToSingle(caseTempbyte, 0);

                            byte[] esSumbyte = new byte[4];
                            esSumbyte[0] = byteDataArr[18];
                            esSumbyte[1] = byteDataArr[19];
                            esSumbyte[2] = byteDataArr[20];
                            esSumbyte[3] = byteDataArr[21];
                            var esSum = BitConverter.ToInt32(esSumbyte, 0);

                            byte[] switchOnSumbyte = new byte[4];
                            switchOnSumbyte[0] = byteDataArr[22];
                            switchOnSumbyte[1] = byteDataArr[23];
                            switchOnSumbyte[2] = byteDataArr[24];
                            switchOnSumbyte[3] = byteDataArr[25];
                            var switchOnSum = BitConverter.ToInt32(switchOnSumbyte, 0);

                            byte[] switchIntbyte = new byte[4];
                            switchIntbyte[0] = byteDataArr[26];
                            switchIntbyte[1] = byteDataArr[27];
                            switchIntbyte[2] = byteDataArr[28];
                            switchIntbyte[3] = byteDataArr[29];
                            var switchInt = BitConverter.ToInt32(switchIntbyte, 0);

                            byte[] aliveTimebyte = new byte[4];
                            aliveTimebyte[0] = byteDataArr[30];
                            aliveTimebyte[1] = byteDataArr[31];
                            aliveTimebyte[2] = byteDataArr[32];
                            aliveTimebyte[3] = byteDataArr[33];
                            var aliveTime = BitConverter.ToInt32(aliveTimebyte, 0);

                            byte[] voltageByte = new byte[4];
                            voltageByte[0] = byteDataArr[34];
                            voltageByte[1] = byteDataArr[35];
                            voltageByte[2] = byteDataArr[36];
                            voltageByte[3] = byteDataArr[37];
                            var voltage = BitConverter.ToSingle(voltageByte, 0);

                            byte[] thresholdVoltageByte = new byte[4];
                            thresholdVoltageByte[0] = byteDataArr[38];
                            thresholdVoltageByte[1] = byteDataArr[39];
                            thresholdVoltageByte[2] = byteDataArr[40];
                            thresholdVoltageByte[3] = byteDataArr[41];
                            var thresholdVoltage = BitConverter.ToSingle(thresholdVoltageByte, 0);

                            MtbDevStatus model = new MtbDevStatus
                            {
                                ACB = acb,
                                AlarmTemp = alarmTemp,
                                CameraOne = cameraOne,
                                CameraTwo = cameraTwo,
                                CameraThree = cameraThree,
                                CaseTemp = caseTemp,
                                CoolingTemp = coolingTemp,
                                CreateDate = DateTime.Now,
                                CrsCode = i.crsCode,
                                SubCode = i.subCode,
                                ESAlarm = esAlarm,
                                ESSum = esSum,
                                SwitchInt = switchInt,
                                SwitchOnSum = switchOnSum,
                                FOT = fot,
                                Router = router,
                                Fan = fan,
                                TempAlarm = tempAlarm,
                                //CrsName = i.CrsName,
                                AliveTime = aliveTime,
                                IsOnline = true,
                                Voltage = voltage,
                                ThresholdVoltage = thresholdVoltage


                            };
                            //try
                            //{
                            //   // UpdateIsOnline(true, i.crsCode);
                            //    //Log.Info($"Ip={i.crsIp}&Msg=Crossing更新在线状态成功");
                            //}
                            //catch (Exception e)
                            //{
                            //    Console.WriteLine($"Ip={i.crsIp}&Msg=Crossing更新在线状态异常:{e.Message}");
                            //}

                            if (IsExist(i.crsCode))
                            {
                                try
                                {
                                    UpdateOnline(model);
                                    // Log.Info($"Ip={i.crsIp}&Msg=crossingstatus更新在线状态成功");
                                }
                                catch (Exception e)
                                {

                                    Console.WriteLine($"Ip={i.crsIp}&Msg=crossingstatus Update Online:{e.Message}");
                                    return;
                                }

                            }
                            else
                            {
                                try
                                {
                                    InsertOnline(model);
                                    // Log.Info($"Ip={i.crsIp}&Msg=crossingstatus插入在线状态成功");
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine($"Ip={i.crsIp}&Msg=crossingstatus Insert Online:{e.Message}");
                                    return;
                                }

                            }

                        }
                        catch (Exception e)
                        {

                            clientSocket.Dispose();
                            Console.WriteLine($"Ip={i.crsIp}&Msg=Receive Para Convert:{e.Message}");

                            //UpdateIsOnline(false, i.crsCode);
                            //Console.WriteLine($"Ip={i.crsIp}&Msg=Crossing更新离线状态成功");
                            var model = GetMDevstatusByCrsCode(i.crsCode);
                            if (IsExist(i.crsCode))
                            {
                                UpdateOnline(model);
                                //Console.WriteLine($"Ip={i.crsIp}&Msg=crossingstatus更新离线成功");
                            }
                            else
                            {
                                InsertOnline(model);
                                //Console.WriteLine($"Ip={i.crsIp}&Msg=crossingstatus插入离线状态成功");
                            }
                            //continue;
                            // parallelLoopStat.Break();
                            return;//不加return，可能会发生该进程资源未释放。
                        }


                    }
                    catch (Exception e)
                    {
                        //Console.WriteLine($"Ip={i.crsIp}&Msg={e.Message}");
                        //UpdateIsOnline(false, i.crsCode);
                        //Console.WriteLine($"Ip={i.crsIp}&Msg=Crossing更新离线状态成功");
                        Console.Write(e.Message);
                        var model = GetMDevstatusByCrsCode(i.crsCode);
                        if (IsExist(i.crsCode))
                        {
                            UpdateOnline(model);
                          //  Console.WriteLine($"Ip={i.crsIp}&Msg=crossingstatus更新离线成功");
                        }
                        else
                        {
                            InsertOnline(model);
                          //  Console.WriteLine($"Ip={i.crsIp}&Msg=crossingstatus插入离线状态成功");
                        }
                        // clientSocket.Shutdown(SocketShutdown.Both);//可去除

                        clientSocket.Dispose();
                        return;//不加return，可能会发生该进程资源未释放。
                    }

                }
                else
                {
                    //UpdateIsOnline(false, i.crsCode);
                    // Console.WriteLine($"Ip={i.crsIp}&Msg=Crossing更新离线状态");
                    try
                    {


                        var model = GetMDevstatusByCrsCode(i.crsCode);
                        if (IsExist(i.crsCode))
                        {
                            UpdateOnline(model);
                          //  Console.WriteLine($"Ip={i.crsIp}&Msg=crossingstatus更新离线状态");
                        }
                        else
                        {
                            InsertOnline(model);
                           // Console.WriteLine($"Ip={i.crsIp}&Msg=crossingstatus插入离线状态");
                        }
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
                        Console.WriteLine($"Ip={i.crsIp}&Msg=Connect Error");
                        // clientSocket.Shutdown(SocketShutdown.Both);//可去除

                        clientSocket.Dispose();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        return;//不加return，可能会发生该进程资源未释放。

                    }
                }

                clientSocket.Dispose();

            });




        }
        //public static void UpdateIsOnline(bool isOnline, string crsCode)
        //{
        //    using (var conn = DbContext.Open())
        //    {
        //        conn.Execute(@"UPDATE Crossing SET IsOnline=@IsOnline,stime=@stime  WHERE CrsCode=@CrsCode",
        //            new { isOnline, crsCode,stime=DateTime.Now });
        //        conn.Close();
        //        conn.Dispose();
        //    }

        //}
        public static bool IsExist(string crsCode)
        {
            using (var conn = DbContext.Open())
            {
                var a = conn.ExecuteScalar<int>(@"select count(*) from crossingstatus where CrsCode=@CrsCode",
                            new { crsCode }) > 0;
                conn.Close();
                conn.Dispose();
                return a;
            }


        }
        public static bool IsExistFault(string faultCode)
        {
            using (var conn = DbContext.Open())
            {
                var a = conn.ExecuteScalar<int>(@"select count(*) from fault where faultCode=@faultCode and faultType=@faultType",
                            new { faultCode,faultType=0 }) > 0;
                conn.Close();
                conn.Dispose();
                return a;
            }


        }
        public static void UpdateOnline(MtbDevStatus model)
        {
            using (var conn = DbContext.Open())
            {
                conn.Execute(@"UPDATE crossingstatus SET 
 CameraOne=@CameraOne,CameraTwo=@CameraTwo,CameraThree=@CameraThree,FOT=@FOT,Router=@Router,Fan=@Fan,ACB=@ACB,TempAlarm=@TempAlarm,ESAlarm=@ESAlarm,CoolingTemp=@CoolingTemp,AlarmTemp=@AlarmTemp,CaseTemp=@CaseTemp,ESSum=@ESSum,SwitchOnSum=@SwitchOnSum,SwitchInt=@SwitchInt,CreateDate=@CreateDate,IsOnline=@IsOnline,AliveTime=@AliveTime,Voltage=@Voltage,ThresholdVoltage=@ThresholdVoltage  WHERE CrsCode=@CrsCode",
                    new { model.CameraOne, model.CameraTwo, model.CameraThree, model.FOT, model.Router, model.Fan, model.ACB, model.TempAlarm, model.ESAlarm, model.CoolingTemp, model.AlarmTemp, model.CaseTemp, model.ESSum, model.SwitchOnSum, model.SwitchInt, model.CreateDate, model.CrsCode, model.IsOnline, model.AliveTime, model.Voltage, model.ThresholdVoltage});
                conn.Close();
                conn.Dispose();
            }

        }
        public static void UpdateOfflineFault(dynamic i)
        {
            using (var conn = DbContext.Open())
            {
                conn.Execute(@"UPDATE fault SET devName=@devName,lat=@lat,lng=@lng,ip=@ip,memo=@memo,level=@level  WHERE faultCode=@faultCode and faultType=@faultType",
                    new { faultCode = i.crsCode, devName = i.crsName, lat = i.lat, lng = i.lng, ip = i.crsIp, memo = "机箱设备离线", level = 3 ,faultType=0});
                conn.Close();
                conn.Dispose();
            }

        }
        public static void InsertOnline(MtbDevStatus model)
        {

            using (var conn = DbContext.Open())
            {
                conn.Execute(@"INSERT INTO crossingstatus
        (CameraOne,CameraTwo,CameraThree,FOT,Router,Fan,ACB,TempAlarm,ESAlarm,CoolingTemp,AlarmTemp,CaseTemp,ESSum,SwitchOnSum,SwitchInt,CreateDate,CrsCode,IsOnline,AliveTime,Voltage,ThresholdVoltage)
    VALUES
        (@CameraOne,@CameraTwo,@CameraThree,@FOT,@Router,@Fan,@ACB,@TempAlarm,@ESAlarm,@CoolingTemp,@AlarmTemp,@CaseTemp,@ESSum,@SwitchOnSum,@SwitchInt,@CreateDate,@CrsCode,@IsOnline,@AliveTime,@Voltage,@ThresholdVoltage)",
                    new { model.CameraOne, model.CameraTwo, model.CameraThree, model.FOT, model.Router, model.Fan, model.ACB, model.TempAlarm, model.ESAlarm, model.CoolingTemp, model.AlarmTemp, model.CaseTemp, model.ESSum, model.SwitchOnSum, model.SwitchInt, model.CreateDate, model.CrsCode, model.IsOnline, model.AliveTime, model.Voltage, model.ThresholdVoltage });
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
                    new { faultCode = i.crsCode, devName = i.crsName, lat = i.lat, lng = i.lng, ip = i.crsIp, memo = "智能机箱离线", level = 3, faultTime = DateTime.Now });
                conn.Close();
                conn.Dispose();
            }

        }
        public static MtbDevStatus GetMDevstatusByCrsCode(string crsCode)
        {
            MtbDevStatus model = new MtbDevStatus
            {
                ACB = false,
                AlarmTemp = 0,
                CameraOne = false,
                CameraTwo = false,
                CameraThree = false,
                CaseTemp = 0,
                CoolingTemp = 0,
                CreateDate = DateTime.Now,
                CrsCode = crsCode,
                // SubCode = subCode,
                ESAlarm = false,
                ESSum = 0,
                SwitchInt = 0,
                SwitchOnSum = 0,
                FOT = false,
                Router = false,
                Fan = false,
                TempAlarm = false,
                AliveTime = 2,
                ThresholdVoltage = 0,
                IsOnline = false

            };
            return model;
        }

    }
}
