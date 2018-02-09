using System;

namespace AiWinLinux
{
    /// <summary>
    /// 作者:yekin-yu
    /// 日期：2017/8/16 17:07:55
    /// </summary>
    public class MTbfault
    {
        /// <summary>
        ///  id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///  faultCode
        /// </summary>
        public string FaultCode { get; set; }

        /// <summary>
        ///  devName
        /// </summary>
        public string DevName { get; set; }

        /// <summary>
        ///  devManager
        /// </summary>
        public string DevManager { get; set; }

        /// <summary>
        ///  phone
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        ///  lat
        /// </summary>
        public double? Lat { get; set; }

        /// <summary>
        ///  lng
        /// </summary>
        public double? Lng { get; set; }

        /// <summary>
        ///  ip
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        ///  memo
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        ///  level
        /// </summary>
        public int? Level { get; set; }

        /// <summary>
        ///  faultTime
        /// </summary>
        public DateTime FaultTime { get; set; }

        /// <summary>
        ///  distributeTime
        /// </summary>
        public DateTime DistributeTime { get; set; }

        /// <summary>
        ///  isDistribute
        /// </summary>
        public bool IsDistribute { get; set; }

    }
}