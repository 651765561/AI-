using System;

namespace AiWin_API
{
    /// <summary>
    /// 作者:yekin-yu
    /// 日期：2016/12/14 12:03:01
    /// </summary>
    public class MtbDevStatus
    {
        /// <summary>
        ///  ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///  Camera_One
        /// </summary>
        public bool CameraOne { get; set; }

        /// <summary>
        ///  Camera_Two
        /// </summary>
        public bool CameraTwo { get; set; }

        /// <summary>
        ///  Camera_Three
        /// </summary>
        public bool CameraThree { get; set; }

        /// <summary>
        /// Fiber Optic Transceiver(是否重启) FOT
        /// </summary>
        public bool FOT { get; set; }

        /// <summary>
        /// 路由器(是否重启) Router
        /// </summary>
        public bool Router { get; set; }

        /// <summary>
        /// 风扇 Fan
        /// </summary>
        public bool Fan { get; set; }

        /// <summary>
        /// air circuit breaker ACB
        /// </summary>
        public bool ACB { get; set; }

        /// <summary>
        /// 超温报警 TempAlarm
        /// </summary>
        public bool TempAlarm { get; set; }

        /// <summary>
        /// Entrance Securitry Alarm ESAlarm
        /// </summary>
        public bool ESAlarm { get; set; }

        /// <summary>
        ///  CoolingTemp
        /// </summary>
        public double? CoolingTemp { get; set; }

        /// <summary>
        ///  AlarmTemp
        /// </summary>
        public double? AlarmTemp { get; set; }

        /// <summary>
        ///  CaseTemp
        /// </summary>
        public double? CaseTemp { get; set; }

        /// <summary>
        ///  ESSum
        /// </summary>
        public int ESSum { get; set; }

        /// <summary>
        ///  SwitchOnSum
        /// </summary>
        public int SwitchOnSum { get; set; }

        /// <summary>
        /// Switch Interval(合闸间隔) SwitchInt
        /// </summary>
        public int SwitchInt { get; set; }

        /// <summary>
        ///  CreateDate
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        ///  CrsCode
        /// </summary>
        public string CrsCode { get; set; }

        /// <summary>
        ///  IsOnline
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        ///  AliveTime
        /// </summary>
        public int AliveTime { get; set; }

        /// <summary>
        /// 电池电压 Voltage
        /// </summary>
        public double Voltage { get; set; }

        /// <summary>
        /// 门限电压 ThresholdVoltage
        /// </summary>
        public double ThresholdVoltage { get; set; }

        /// <summary>
        ///  SubCode
        /// </summary>
        public string SubCode { get; set; }
        public bool CloseAlarm { get; set; }
    }
}