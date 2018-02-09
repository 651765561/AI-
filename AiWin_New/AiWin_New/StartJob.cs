using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Quartz;

namespace AiWin_New
{
    [DisallowConcurrentExecution]//加上并发限制
    public class StartJob : IJob
    {
        public static readonly LogFileFolder Log = new LogFileFolder("StartJob");

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                var para = CrossingService.GetIpList(DbContext.GetSubCode());

                 CrossingService.InSertDb(para);
               
                //for (int i = 0; i < para.Count; i += 20)
                //{
                //    var i1 = i;
                //    Log.Info($"启动线程{i1}-{i1 + 20}");
                //    Task.Run(() =>
                //    {
                //        CrossingService.InSertDb(para.Skip(i1).Take(20).ToList());
                //    });

                //}
                para.Clear();

            }
            catch (Exception ex)
            {
                Log.FatalFormat("\r\n状态监测过程中发生异常:\r\n{0}\r\n{1}\r\n",
                    ex.Message,
                    ex.StackTrace);
            }



        }
   

    }
}
