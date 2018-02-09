using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace AI
{
    public class Ctl
    {
        private readonly CrossingService _crossingService;
        public Ctl(CrossingService crossingService)
        {
            _crossingService = crossingService;
        }
        /// <summary>
        /// 相机一开||关
        /// </summary>
        /// <param name="crossingId"></param>
        /// <param name="cameraOne"></param>
        /// <returns></returns>
        public  JsonMessage CameraOne(string crossingId, string cameraOne)
        {
            var msg = new JsonMessage { Success = false, Message = "parameter error" };
            if (cameraOne == "1" || cameraOne.ToLower() == "true")
            {
                msg = OnOrOff(crossingId, "EF EF 04 02 01 E5", "相机一开启");

            }
            if (cameraOne == "0" || cameraOne.ToLower() == "false")
            {
                msg = OnOrOff(crossingId, "EF EF 04 02 00 E4", "相机一关闭");

            }

            return msg;
        }

        /// <summary>
        /// 相机二开||关
        /// </summary>
        /// <param name="crossingId"></param>
        /// <param name="cameraTwo"></param>
        /// <returns></returns>
        public  JsonMessage CameraTwo(string crossingId, string cameraTwo)
        {
            var msg = new JsonMessage { Success = false, Message = "parameter error" };
            if (cameraTwo == "1" || cameraTwo.ToLower() == "true")
            {
                msg = OnOrOff(crossingId, "EF EF 04 01 01 E4", "相机二开启");
            }
            if (cameraTwo == "0" || cameraTwo.ToLower() == "false")
            {
                msg = OnOrOff(crossingId, "EF EF 04 01 00 E3", "相机二关闭");
            }

            return msg;
        }

        /// <summary>
        /// 相机三开||关
        /// </summary>
        /// <param name="crossingId"></param>
        /// <param name="cameraThree"></param>
        /// <returns></returns>
        public  JsonMessage CameraThree(string crossingId, string cameraThree)
        {
            var msg = new JsonMessage { Success = false, Message = "parameter error" };
            if (cameraThree == "1" || cameraThree.ToLower() == "true")
            {
                msg = OnOrOff(crossingId, "EF EF 04 07 01 EA", "相机三开启");
            }
            if (cameraThree == "0" || cameraThree.ToLower() == "false")
            {
                msg = OnOrOff(crossingId, "EF EF 04 07 00 E9", "相机三关闭");
            }

            return msg;
        }

        /// <summary>
        /// 光端机重启
        /// </summary>
        /// <param name="crossingId"></param>
        /// <param name="fot"></param>
        /// <returns></returns>
        public  JsonMessage Fot(string crossingId, string fot)
        {
            var msg = new JsonMessage { Success = false, Message = "parameter error" };
            if (fot == "1" || fot.ToLower() == "true")
            {
                msg = OnOrOff(crossingId, "EF EF 04 03 01 E6", "光端机重启");
            }
            if (fot == "0" || fot.ToLower() == "false")
            {
                msg = OnOrOff(crossingId, "EF EF 04 03 00 E5", "光端机重启");
            }

            return msg;
        }

        /// <summary>
        /// 路由器重启
        /// </summary>
        /// <param name="crossingId"></param>
        /// <param name="router"></param>
        /// <returns></returns>
        public  JsonMessage Router(string crossingId, string router)
        {
            var msg = new JsonMessage { Success = false, Message = "parameter error" };
            if (router == "1" || router.ToLower() == "true")
            {
                msg = OnOrOff(crossingId, "EF EF 04 04 01 E7", "路由器重启");
            }
            if (router == "0" || router.ToLower() == "false")
            {
                msg = OnOrOff(crossingId, "EF EF 04 04 00 E6", "路由器重启");
            }

            return msg;
        }

        /// <summary>
        /// 风扇开关
        /// </summary>
        /// <param name="crossingId"></param>
        /// <param name="fan"></param>
        /// <returns></returns>
        public  JsonMessage Fan(string crossingId, string fan)
        {
            var msg = new JsonMessage { Success = false, Message = "parameter error" };
            if (fan == "1" || fan.ToLower() == "true")
            {
                msg = OnOrOff(crossingId, "EF EF 04 05 01 E8", "风扇开启");
            }
            if (fan == "0" || fan.ToLower() == "false")
            {
                msg = OnOrOff(crossingId, "EF EF 04 05 00 E7", "风扇关闭");
            }

            return msg;
        }

        /// <summary>
        /// 空开合分
        /// </summary>
        /// <param name="crossingId"></param>
        /// <param name="acb"></param>
        /// <returns></returns>
        public  JsonMessage Acb(string crossingId, string acb)
        {
            var msg = new JsonMessage { Success = false, Message = "parameter error" };
            if (acb == "1" || acb.ToLower() == "true")
            {
                msg = OnOrOff(crossingId, "EF EF 04 06 01 E9", "空开合闸");
            }
            if (acb == "0" || acb.ToLower() == "false")
            {
                msg = OnOrOff(crossingId, "EF EF 04 06 00 E8", "空开分闸");
            }

            return msg;
        }

        /// <summary>
        /// 关闭报警
        /// </summary>
        /// <param name="crossingId"></param>
        /// <param name="closeAlarm"></param>
        /// <returns></returns>
        public  JsonMessage CloseAlarm(string crossingId, string closeAlarm)
        {
            var msg = new JsonMessage { Success = false, Message = "parameter error" };

            dynamic list = _crossingService.GetIP_IsOnline(crossingId);
            if (list != null)
            {
                // bool isOnline = list.IsOnline;
                string ip = list.CrsIp;

                if (closeAlarm == "1" || closeAlarm.ToLower() == "true")
                {
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPAddress ipAddress = IPAddress.Parse(ip);
                    bool conn = TcpConn.ConnectWithTimeout(socket, new IPEndPoint(ipAddress, 5000), 2000); //超时2秒
                    try
                    {

                        if (conn)
                        {
                            string send = "EF EF 04 15 00 F7";

                            var data = Common.StrToHexByte(send);
                            socket.Send(data);
                            Thread.Sleep(500);
                            msg.Success = true;
                            msg.Message = ip + "报警信息关闭成功";
                            msg.Title = "关闭该次报警提醒";
                        }
                        else
                        {
                            msg.Success = false;
                            msg.Message = ip + "通讯网络错误!";
                            msg.Title = "关闭该次报警提醒";

                        }
                        socket.Dispose();

                    }
                    catch (Exception e)
                    {
                        socket.Dispose();
                        msg.Success = false;
                        msg.Message = ip + "通讯网络错误!" + e.Message;
                        msg.Title = "关闭该次报警提醒";
                    }



                }
                else
                {
                    msg.Success = false;
                    msg.Message = ip + "关闭该次报警提醒失败!";
                    msg.Title = "关闭该次报警提醒";
                }
                //InertLogs(list.CrsName, msg.Title, msg.Message);
            }
            return msg;
        }

        /// <summary>
        /// 设置冷却温度
        /// </summary>
        /// <param name="crossingId"></param>
        /// <param name="coolingTemp"></param>
        /// <returns></returns>
        public  JsonMessage CoolingTemp(string crossingId, string coolingTemp)
        {
            var msg = new JsonMessage { Success = false, Message = "parameter error" };

            dynamic list = _crossingService.GetIP_IsOnline(crossingId);
            if (list != null)
            {
                //bool isOnline = list.IsOnline;
                string ip = list.CrsIp;
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ipAddress = IPAddress.Parse(ip);
                bool conn = TcpConn.ConnectWithTimeout(socket, new IPEndPoint(ipAddress, 5000), 2000); //超时2秒

                try
                {
                    float f = float.Parse(coolingTemp);
                    byte[] c = BitConverter.GetBytes(f);

                    string dd = "EF EF 07 09 " + Common.ByteToHexStr2(c).TrimEnd(' ');

                    if (conn)
                    {
                        var sum = dd.Split(' ').Select(i => Convert.ToInt32(i, 16)).Sum();
                        var s = Convert.ToString(sum, 16);
                        var result = s.Substring(1, s.Length - 1);
                        string send = dd + result;
                        var aa = Common.StrToHexByte(send);
                        //byte[] temp = { 0xEF, 0xEF, 0x04, 0x01, 0x01, 0xE4 };
                        socket.Send(aa); // Sends some data 
                        Thread.Sleep(500);
                        var data = socket.Receive(new byte[9]); // Receives some data back (blocks execution)
                        msg.Success = true;
                        msg.Message = ip + "设置冷却温度:" + f + "℃成功" + data;
                        msg.Title = "设置冷却温度";
                    }
                    else
                    {
                        msg.Success = false;
                        msg.Message = ip + ":通讯网络错误!";
                        msg.Title = "设置冷却温度";
                    }
                    socket.Dispose();
                }
                catch (Exception e)
                {
                    socket.Dispose();
                    msg.Success = false;
                    msg.Message = ip + ":通讯网络错误!" + e.Message;
                    msg.Title = "设置冷却温度";
                }

                // InertLogs(list.CrsName, msg.Title, msg.Message);
            }
            return msg;
        }

        /// <summary>
        /// 设置报警温度
        /// </summary>
        /// <param name="crossingId"></param>
        /// <param name="alarmTemp"></param>
        /// <returns></returns>
        public  JsonMessage AlarmTemp(string crossingId, string alarmTemp)
        {
            var msg = new JsonMessage { Success = false, Message = "parameter error" };

            dynamic list = _crossingService.GetIP_IsOnline(crossingId);
            if (list != null)
            {
                //bool isOnline = list.IsOnline;
                string ip = list.CrsIp;

                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ipAddress = IPAddress.Parse(ip);
                bool conn = TcpConn.ConnectWithTimeout(socket, new IPEndPoint(ipAddress, 5000), 2000); //超时2秒

                try
                {
                    float f = float.Parse(alarmTemp);

                    byte[] c = BitConverter.GetBytes(f);

                    string dd = "EF EF 07 0E " + Common.ByteToHexStr2(c).TrimEnd(' ');

                    if (conn)
                    {
                        var sum = dd.Split(' ').Select(i => Convert.ToInt32(i, 16)).Sum();
                        var s = Convert.ToString(sum, 16);
                        var result = s.Substring(1, s.Length - 1);
                        string send = dd + result;
                        var aa = Common.StrToHexByte(send);
                        socket.Send(aa); // Sends some data
                        Thread.Sleep(500);
                        var data = socket.Receive(new byte[9]); // Receives some data back (blocks execution)
                        //Thread.Sleep(500);
                        msg.Success = true;
                        msg.Message = ip + "设置报警温度:" + f + "℃成功" + data;
                        msg.Title = "设置报警温度";
                    }
                    else
                    {
                        msg.Success = false;
                        msg.Message = ip + ":通讯网络错误!";
                        msg.Title = "设置报警温度";
                    }
                    socket.Dispose();
                }
                catch (Exception e)
                {
                    socket.Dispose();
                    msg.Success = false;
                    msg.Message = ip + ":通讯网络错误!" + e.Message;
                    msg.Title = "设置报警温度";
                }

                //InertLogs(list.CrsName, msg.Title, msg.Message);
            }
            return msg;
        }

        /// <summary>
        /// 设置空开合闸次数
        /// </summary>
        /// <param name="crossingId"></param>
        /// <param name="switchOnSum"></param>
        /// <returns></returns>
        public  JsonMessage SwitchOnSum(string crossingId, string switchOnSum)
        {
            var msg = new JsonMessage { Success = false, Message = "parameter error" };

            dynamic list = _crossingService.GetIP_IsOnline(crossingId);
            if (list != null)
            {
                // bool isOnline = list.IsOnline;
                string ip = list.CrsIp;
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ipAddress = IPAddress.Parse(ip);
                bool conn = TcpConn.ConnectWithTimeout(socket, new IPEndPoint(ipAddress, 5000), 2000); //超时2秒

                try
                {
                    int f = int.Parse(switchOnSum);

                    byte[] c = BitConverter.GetBytes(f);

                    string dd = "EF EF 07 10 " + Common.ByteToHexStr2(c).TrimEnd(' ');

                    if (conn)
                    {
                        var sum = dd.Split(' ').Select(i => Convert.ToInt32(i, 16)).Sum();
                        var s = Convert.ToString(sum, 16);
                        var result = s.Substring(1, s.Length - 1);
                        string send = dd + result;
                        var aa = Common.StrToHexByte(send);
                        socket.Send(aa); // Sends some data
                        Thread.Sleep(500);
                        var data = socket.Receive(new byte[9]); // Receives some data back (blocks execution)
                        //Thread.Sleep(500);
                        msg.Success = true;
                        msg.Message = ip + "设置空开合闸次数:" + f + "成功" + data;
                        msg.Title = "设置空开合闸次数";
                    }
                    else
                    {
                        msg.Success = false;
                        msg.Message = ip + ":通讯网络错误!";
                        msg.Title = "设置空开合闸次数";
                    }
                    socket.Dispose();
                }
                catch (Exception e)
                {
                    socket.Dispose();
                    msg.Success = false;
                    msg.Message = ip + ":通讯网络错误!" + e.Message;
                    msg.Title = "设置空开合闸次数";
                }

                //InertLogs(list.CrsName, msg.Title, msg.Message);
            }
            return msg;
        }

        /// <summary>
        /// 设置空开合闸间隔
        /// </summary>
        /// <param name="crossingId"></param>
        /// <param name="switchInt"></param>
        /// <returns></returns>
        public  JsonMessage SwitchInt(string crossingId, string switchInt)
        {
            var msg = new JsonMessage { Success = false, Message = "parameter error" };

            dynamic list = _crossingService.GetIP_IsOnline(crossingId);
            if (list != null)
            {
                //bool isOnline = list.IsOnline;
                string ip = list.CrsIp;
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ipAddress = IPAddress.Parse(ip);
                bool conn = TcpConn.ConnectWithTimeout(socket, new IPEndPoint(ipAddress, 5000), 2000); //超时2秒

                try
                {
                    int d = int.Parse(switchInt);

                    byte[] c = BitConverter.GetBytes(d);

                    string dd = "EF EF 07 12 " + Common.ByteToHexStr2(c).TrimEnd(' ');
                    if (conn)
                    {
                        var sum = dd.Split(' ').Select(i => Convert.ToInt32(i, 16)).Sum();
                        var s = Convert.ToString(sum, 16);
                        var result = s.Substring(1, s.Length - 1);
                        string send = dd + result;
                        var aa = Common.StrToHexByte(send);
                        socket.Send(aa); // Sends some data
                        Thread.Sleep(500);
                        var data = socket.Receive(new byte[9]); // Receives some data back (blocks execution)
                        msg.Success = true;
                        msg.Message = ip + "设置空开合闸间隔:" + d + "成功" + data;
                        msg.Title = "设置空开合闸间隔";
                    }
                    else
                    {
                        msg.Success = false;
                        msg.Message = ip + ":通讯网络错误!";
                        msg.Title = "设置空开合闸间隔";

                    }
                    socket.Dispose();

                }
                catch (Exception e)
                {
                    socket.Dispose();
                    msg.Success = false;
                    msg.Message = ip + ":通讯网络错误!" + e.Message;
                    msg.Title = "设置空开合闸间隔";
                }

                //InertLogs(list.CrsName, msg.Title, msg.Message);
            }
            return msg;
        }

        /// <summary>
        /// 设置通信链接时间
        /// </summary>
        /// <param name="crossingId"></param>
        /// <param name="aliveTime"></param>
        /// <returns></returns>
        public  JsonMessage AliveTime(string crossingId, string aliveTime)
        {
            var msg = new JsonMessage { Success = false, Message = "parameter error" };

            dynamic list = _crossingService.GetIP_IsOnline(crossingId);
            if (list != null)
            {
                //bool isOnline = list.IsOnline;
                string ip = list.CrsIp;
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ipAddress = IPAddress.Parse(ip);
                bool conn = TcpConn.ConnectWithTimeout(socket, new IPEndPoint(ipAddress, 5000), 2000); //超时2秒

                try
                {
                    int d = int.Parse(aliveTime);

                    byte[] c = BitConverter.GetBytes(d);

                    string dd = "EF EF 07 18 " + Common.ByteToHexStr2(c).TrimEnd(' ');


                    if (conn)
                    {
                        var sum = dd.Split(' ').Select(i => Convert.ToInt32(i, 16)).Sum();
                        var s = Convert.ToString(sum, 16);
                        var result = s.Substring(1, s.Length - 1);
                        string send = dd + result;
                        var aa = Common.StrToHexByte(send);
                        socket.Send(aa); // Sends some data
                        Thread.Sleep(500);
                        var data = socket.Receive(new byte[9]); // Receives some data back (blocks execution)
                        msg.Success = true;
                        msg.Message = ip + "设置通信链接时间:" + d + "小时成功" + data;
                        msg.Title = "设置通信链接时间";
                    }
                    else
                    {
                        msg.Success = false;
                        msg.Message = ip + ":通讯网络错误!";
                        msg.Title = "设置通信链接时间";
                    }
                    socket.Dispose();
                }
                catch (Exception e)
                {
                    socket.Dispose();
                    msg.Success = false;
                    msg.Message = ip + ":通讯网络错误!" + e.Message;
                    msg.Title = "设置通信链接时间";
                }

                //InertLogs(list.CrsName, msg.Title, msg.Message);
            }
            return msg;
        }

        public  JsonMessage OnOrOff(string crossingId, string onOrOff, string ctrlName)
        {
            var msg = new JsonMessage { Success = false, Message = "parameter error" };
            dynamic list = _crossingService.GetIP_IsOnline(crossingId);
            if (list != null)
            {

                //bool isOnline = list.IsOnline;
                string ip = list.CrsIp;
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ipAddress = IPAddress.Parse(ip);
                bool conn = TcpConn.ConnectWithTimeout(socket, new IPEndPoint(ipAddress, 5000), 2000); //超时2秒

                try
                {

                    if (conn)
                    {
                        string send = onOrOff;
                        var data = Common.StrToHexByte(send);
                        socket.Send(data); // Sends some data
                        Thread.Sleep(500);
                        var result = socket.Receive(new byte[6]); // Receives some data back (blocks execution)
                        msg.Success = true;
                        msg.Message = send;
                        msg.Title = ctrlName;

                    }
                    else
                    {
                        msg.Success = false;
                        msg.Message = ip + "通讯网络错误!";
                        msg.Title = ctrlName;

                    }
                    socket.Dispose();
                }
                catch (Exception e)
                {
                    msg.Success = false;
                    msg.Message = ip + "通讯网络错误!" + e.Message;
                    msg.Title = ctrlName;
                }

                //  InertLogs(list.CrsName, msg.Title, msg.Message,ipAddress,browser);
            }
            return msg;
        }

        /// <summary>
        /// 设置电池合闸门限电压
        /// </summary>
        /// <param name="crossingId"></param>
        /// <param name="thresholdVoltage"></param>

        /// <returns></returns>
        public  JsonMessage ThresholdVoltage(string crossingId, string thresholdVoltage)
        {
            var msg = new JsonMessage { Success = false, Message = "parameter error" };

            dynamic list = _crossingService.GetIP_IsOnline(crossingId);
            if (list != null)
            {
                //bool isOnline = list.IsOnline;
                string ip = list.CrsIp;
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ipAddress = IPAddress.Parse(ip);
                bool conn = TcpConn.ConnectWithTimeout(socket, new IPEndPoint(ipAddress, 5000), 2000); //超时2秒

                try
                {
                    float f = float.Parse(thresholdVoltage);
                    byte[] c = BitConverter.GetBytes(f);

                    string dd = "EF EF 07 1B " + Common.ByteToHexStr2(c).TrimEnd(' ');


                    if (conn)
                    {
                        var sum = dd.Split(' ').Select(i => Convert.ToInt32(i, 16)).Sum();
                        var s = Convert.ToString(sum, 16);
                        var result = s.Substring(1, s.Length - 1);
                        string send = dd + result;
                        var aa = Common.StrToHexByte(send);
                        //byte[] temp = { 0xEF, 0xEF, 0x04, 0x01, 0x01, 0xE4 };
                        socket.Send(aa); // Sends some data 
                        Thread.Sleep(500);
                        var data = socket.Receive(new byte[9]); // Receives some data back (blocks execution)
                        msg.Success = true;
                        msg.Message = ip + "设置电池合闸门限电压:" + f + "伏成功" + data;
                        msg.Title = "设置电池合闸门限电压";
                    }
                    else
                    {
                        msg.Success = false;
                        msg.Message = ip + ":通讯网络错误!";
                        msg.Title = "设置电池合闸门限电压";

                    }
                    socket.Dispose();
                }
                catch (Exception e)
                {
                    socket.Dispose();
                    msg.Success = false;
                    msg.Message = ip + ":通讯网络错误!" + e.Message;
                    msg.Title = "设置电池合闸门限电压";
                }

                //InertLogs(list.CrsName, msg.Title, msg.Message);
            }
            return msg;
        }

    }
}
