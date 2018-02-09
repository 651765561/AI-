namespace AiWinLinux
{
    /// <summary>
    /// 作者:yekin-yu
    /// 日期：2016/12/14 12:01:53
    /// </summary>
    public class MtbCrossing
    {
        /// <summary>
        ///  ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///  CrsCode
        /// </summary>
        public string CrsCode { get; set; }

        /// <summary>
        ///  CrsName
        /// </summary>
        public string CrsName { get; set; }

        /// <summary>
        ///  CrsIp
        /// </summary>
        public string CrsIp { get; set; }

        /// <summary>
        ///  Lat
        /// </summary>
        public double? Lat { get; set; }

        /// <summary>
        ///  Lng
        /// </summary>
        public double? Lng { get; set; }

        /// <summary>
        ///  IsEnable
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        ///  IsOnline
        /// </summary>
        public bool IsOnline { get; set; }

        /// <summary>
        ///  SubCode
        /// </summary>
        public string SubCode { get; set; }

    }
}