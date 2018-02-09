using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;

namespace AiWinLinux
{
    [DisallowConcurrentExecution]//加上并发限制
    public class EleJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            // Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var para = EleServices.GetIpList(DbContext.SubCode);
            if (para.Any())
            {
                EleServices.InSertDb(para);

            }
            para.Clear();
            await Console.Out.WriteLineAsync("Ele status");
            //return Task.FromResult<object>(null);
        }
    }
}
