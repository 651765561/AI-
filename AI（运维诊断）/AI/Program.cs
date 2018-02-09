using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;

namespace AI
{
    public class Program
    {
        private static IScheduler _scheduler;
        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); /* 支持中文 */
            StartScheduler();
           // Startup hh = new Startup();
           BuildWebHost(args).Run();

        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseUrls("http://*:9000")
                .UseStartup<Startup>()
                .Build();


        private static void StartScheduler()
        {
            var schedulerFactory = new StdSchedulerFactory();
            _scheduler = schedulerFactory.GetScheduler().Result;
            _scheduler.Start().Wait();

            var job = JobBuilder.Create<EleJob>()
                .WithIdentity("Status")
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("StatusCron")
                .StartNow()
                .WithCronSchedule("*/5 * * * * ?")
                .Build();

            _scheduler.ScheduleJob(job, trigger).Wait();

        }
    }
}
