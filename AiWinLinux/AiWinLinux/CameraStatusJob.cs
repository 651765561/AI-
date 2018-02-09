using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quartz;

namespace AiWinLinux
{
    [DisallowConcurrentExecution]//加上并发限制
    public class CameraStatusJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            // Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var para = CameraService.GetIpList(DbContext.SubCode);
            if (para.Any())
            {
                CameraService.InSertDb(para);

            }
            para.Clear();
            await Console.Out.WriteLineAsync("camera status");
            //return Task.FromResult<object>(null);
        }
    }
}
