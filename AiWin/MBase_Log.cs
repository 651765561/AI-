using System;

namespace AiWin
{
     /// <summary>
    /// 作者:yekin-yu
    /// 日期：2016/11/22 11:06:44
    /// </summary>
    public class MBaseLog
    {
        /// <summary>
        ///  ID
        /// </summary>
         public int Id{ get;set; }
        
        /// <summary>
        ///  OperateTime
        /// </summary>
         public DateTime OperateTime{ get;set; }
        
        /// <summary>
        ///  OperateAccount
        /// </summary>
         public string OperateAccount{ get;set; }
        
        /// <summary>
        ///  OperateTypeId
        /// </summary>
         public int OperateTypeId{ get;set; }
        
        /// <summary>
        ///  OperateType
        /// </summary>
         public string OperateType{ get;set; }
        
        /// <summary>
        ///  IPAddress
        /// </summary>
         public string IpAddress{ get;set; }
        
        /// <summary>
        ///  IPAddressName
        /// </summary>
         public string IpAddressName{ get;set; }
        
        /// <summary>
        ///  Browser
        /// </summary>
         public string Browser{ get;set; }
        
        /// <summary>
        ///  ExecuteResult
        /// </summary>
         public int ExecuteResult{ get;set; }
        
        /// <summary>
        ///  ExecuteResultJson
        /// </summary>
         public string ExecuteResultJson{ get;set; }
        
        /// <summary>
        ///  DeleteMark
        /// </summary>
         public bool DeleteMark{ get;set; }
        
    }
}