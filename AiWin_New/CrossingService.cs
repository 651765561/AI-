using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace AiWin_New
{
    public class CrossingService
    {
        public static readonly LogFileFolder Log = new LogFileFolder("CrossingService");
        public static dynamic GetIP_IsOnline(string crsCode)
        {
            using (var conn = DbContext.Open())
            {
                var sql = @"SELECT IsOnline,CrsIp,CrsName FROM TB_Crossing where CrsCode=@CrsCode";
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
                var sql = @"SELECT CrsIp,SubCode,CrsCode FROM TB_Crossing  where SubCode=@SubCode";

                var result = conn.Query<dynamic>(sql, new { subCode });
                conn.Close();
                conn.Dispose();
                return result.ToList();
            }



        }
        public static void InSertDb(List<dynamic> para)
        {
            //var para =GetIpList(DbContext.GetSubCode());

            foreach (var i in para)
            {
                Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ipAddress = IPAddress.Parse(i.CrsIp);
                bool conn = TcpConn.ConnectWithTimeout(clientSocket, new IPEndPoint(ipAddress, 5000), 2000); //超时2秒

                if (conn)
                {
                   // Log.Info($"Ip={i.CrsIp}&Msg=Socket连接成功");
                    try
                    {
                        // Log.Info($"Ip={i.CrsIp}&Msg=获取设备状态...");
                        string send = "EF EF 04 14 00 F6";
                        var msg = Common.StrToHexByte(send);
                        try
                        {
                            clientSocket.Send(msg); // Sends some data  BitConverter.ToString(msg, 0))
                        }
                        catch (Exception e)
                        {
                            // clientSocket.Shutdown(SocketShutdown.Both);//可去除
                            clientSocket.Close();
                            clientSocket.Dispose();
                            Log.Error($"Ip={i.CrsIp}&Msg=发送状态:{e.Message}");
                            continue;
                        }
                        var buffer = new byte[103];
                        try
                        {
                           
                            clientSocket.Receive(buffer); // Receives some data back (blocks execution)
                        }
                        catch (Exception e)
                        {
                            // clientSocket.Shutdown(SocketShutdown.Both);//可去除
                            clientSocket.Close();
                            clientSocket.Dispose();
                            Log.Error($"Ip={i.CrsIp}&Msg=接收状态异常:{e.Message}");
                            continue;
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
                                CrsCode = i.CrsCode,
                                SubCode = i.SubCode,
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
                            try
                            {
                                UpdateIsOnline(true, i.CrsCode);
                                //Log.Info($"Ip={i.CrsIp}&Msg=TB_Crossing更新在线状态成功");
                            }
                            catch (Exception e)
                            {
                                Log.Error($"Ip={i.CrsIp}&Msg=TB_Crossing更新在线状态异常:{e.Message}");
                            }

                            if (IsExist(i.CrsCode))
                            {
                                try
                                {
                                    UpdateOnline(model);
                                    // Log.Info($"Ip={i.CrsIp}&Msg=TB_DevStatus更新在线状态成功");
                                }
                                catch (Exception e)
                                {

                                    Log.Error($"Ip={i.CrsIp}&Msg=TB_DevStatus更新在线状态异常:{e.Message}");
                                }

                            }
                            else
                            {
                                try
                                {
                                    InsertOnline(model);
                                    // Log.Info($"Ip={i.CrsIp}&Msg=TB_DevStatus插入在线状态成功");
                                }
                                catch (Exception e)
                                {
                                    Log.Error($"Ip={i.CrsIp}&Msg=TB_DevStatus插入在线状态异常:{e.Message}");
                                }

                            }

                        }
                        catch (Exception e)
                        {
                            clientSocket.Close();
                            clientSocket.Dispose();
                            Log.Error($"Ip={i.CrsIp}&Msg=接收状态参数转换异常:{e.Message}");
                            continue;
                        }
  

                    }
                    catch (Exception e)
                    {
                        Log.Error($"Ip={i.CrsIp}&Msg={e.Message}");
                        UpdateIsOnline(false, i.CrsCode);
                        Log.Fatal($"Ip={i.CrsIp}&Msg=TB_Crossing更新离线状态成功");
                        var model = GetMDevstatusByCrsCode(i.CrsCode, i.SubCode);
                        if (IsExist(i.CrsCode))
                        {
                            UpdateOnline(model);
                            Log.Fatal($"Ip={i.CrsIp}&Msg=TB_DevStatus更新离线成功");
                        }
                        else
                        {
                            InsertOnline(model);
                            Log.Fatal($"Ip={i.CrsIp}&Msg=TB_DevStatus插入离线状态成功");
                        }
                        // clientSocket.Shutdown(SocketShutdown.Both);//可去除
                        clientSocket.Close();
                        clientSocket.Dispose();

                    }

                }
                else
                {
                    UpdateIsOnline(false, i.CrsCode);
                    Log.Fatal($"Ip={i.CrsIp}&Msg=TB_Crossing更新离线状态");
                    var model = GetMDevstatusByCrsCode(i.CrsCode, i.SubCode);
                    if (IsExist(i.CrsCode))
                    {
                        UpdateOnline(model);
                        Log.Fatal($"Ip={i.CrsIp}&Msg=TB_DevStatus更新离线状态");
                    }
                    else
                    {
                        InsertOnline(model);
                        Log.Fatal($"Ip={i.CrsIp}&Msg=TB_DevStatus插入离线状态");
                    }
                    Log.Error($"Ip={i.CrsIp}&Msg=Socket连接失败");
                    // clientSocket.Shutdown(SocketShutdown.Both);//可去除
                    clientSocket.Close();
                    clientSocket.Dispose();

                }
                clientSocket.Close();
                clientSocket.Dispose();


            }
            


        }
        public static void UpdateIsOnline(bool isOnline, string crsCode)
        {
            using (var conn = DbContext.Open())
            {
                conn.Execute(@"UPDATE TB_Crossing SET IsOnline=@IsOnline  WHERE CrsCode=@CrsCode",
                    new { isOnline, crsCode });
                conn.Close();
                conn.Dispose();
            }

        }
        public static bool IsExist(string crsCode)
        {
            using (var conn = DbContext.Open())
            {
                var a = conn.ExecuteScalar<int>(@"select count(*) from TB_DevStatus where CrsCode=@CrsCode",
                            new { crsCode }) > 0;
                conn.Close();
                conn.Dispose();
                return a;
            }


        }
        public static void UpdateOnline(MtbDevStatus model)
        {
            using (var conn = DbContext.Open())
            {
                conn.Execute(@"UPDATE TB_DevStatus SET 
 CameraOne=@CameraOne,CameraTwo=@CameraTwo,CameraThree=@CameraThree,FOT=@FOT,Router=@Router,Fan=@Fan,ACB=@ACB,TempAlarm=@TempAlarm,ESAlarm=@ESAlarm,CoolingTemp=@CoolingTemp,AlarmTemp=@AlarmTemp,CaseTemp=@CaseTemp,ESSum=@ESSum,SwitchOnSum=@SwitchOnSum,SwitchInt=@SwitchInt,CreateDate=@CreateDate,CrsCode=@CrsCode,IsOnline=@IsOnline,AliveTime=@AliveTime,Voltage=@Voltage,ThresholdVoltage=@ThresholdVoltage,SubCode=@SubCode  WHERE CrsCode=@CrsCode",
                    new { model.CameraOne, model.CameraTwo, model.CameraThree, model.FOT, model.Router, model.Fan, model.ACB, model.TempAlarm, model.ESAlarm, model.CoolingTemp, model.AlarmTemp, model.CaseTemp, model.ESSum, model.SwitchOnSum, model.SwitchInt, model.CreateDate, model.CrsCode, model.IsOnline, model.AliveTime, model.Voltage, model.ThresholdVoltage, model.SubCode });
                conn.Close();
                conn.Dispose();
            }

        }
        public static void InsertOnline(MtbDevStatus model)
        {

            using (var conn = DbContext.Open())
            {
                conn.Execute(@"INSERT INTO TB_DevStatus
        (CameraOne,CameraTwo,CameraThree,FOT,Router,Fan,ACB,TempAlarm,ESAlarm,CoolingTemp,AlarmTemp,CaseTemp,ESSum,SwitchOnSum,SwitchInt,CreateDate,CrsCode,IsOnline,AliveTime,Voltage,ThresholdVoltage,SubCode)
    VALUES
        (@CameraOne,@CameraTwo,@CameraThree,@FOT,@Router,@Fan,@ACB,@TempAlarm,@ESAlarm,@CoolingTemp,@AlarmTemp,@CaseTemp,@ESSum,@SwitchOnSum,@SwitchInt,@CreateDate,@CrsCode,@IsOnline,@AliveTime,@Voltage,@ThresholdVoltage,@SubCode)",
                    new { model.CameraOne, model.CameraTwo, model.CameraThree, model.FOT, model.Router, model.Fan, model.ACB, model.TempAlarm, model.ESAlarm, model.CoolingTemp, model.AlarmTemp, model.CaseTemp, model.ESSum, model.SwitchOnSum, model.SwitchInt, model.CreateDate, model.CrsCode, model.IsOnline, model.AliveTime, model.Voltage, model.ThresholdVoltage, model.SubCode });
                conn.Close();
                conn.Dispose();
            }

        }
        public static MtbDevStatus GetMDevstatusByCrsCode(string crsCode, string subCode)
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
                SubCode = subCode,
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
