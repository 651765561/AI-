using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using Quartz;

namespace AiWinLinux
{
    [DisallowConcurrentExecution]//加上并发限制
    public class StatusJob : IJob
    {

        public async Task Execute(IJobExecutionContext context)
        {
           // Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var para = CrossingService.GetIpList(DbContext.SubCode);
            if (para.Any())
            {
                CrossingService.InSertDb(para);

            }
            para.Clear();
            await Console.Out.WriteLineAsync("crs status");
            //return Task.FromResult<object>(null);
        }

    }
}
