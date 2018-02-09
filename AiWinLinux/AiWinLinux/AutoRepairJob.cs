using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;

namespace AiWinLinux
{
    [DisallowConcurrentExecution]//加上并发限制
    public class AutoRepairJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            // Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var para = AutoRepairService.GetIpList(DbContext.SubCode);
            if (para.Any())
            {
                AutoRepairService.InSertDb(para);

            }
            para.Clear();
            await Console.Out.WriteLineAsync("AutoRepair status");
            //return Task.FromResult<object>(null);
        }
    }
}
