using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using UCFAR.Util.Dapper;

namespace AI
{
    [DisallowConcurrentExecution]//加上并发限制
    public class EleJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var ele = Di.ServiceProvider.GetRequiredService<EleServices>();
            var para = ele.GetCrossingList();
            if (para.Any())
            {
                ele.InsertCrossingStatus(para);

            }
            para.Clear();
            await Console.Out.WriteLineAsync("Ele status");
          
        }
    }
}
