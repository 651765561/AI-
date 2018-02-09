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

namespace AiWinLinux
{
    public class Program
    {
        private static IScheduler _scheduler;
        public static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            StartScheduler();
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

            var job = JobBuilder.Create<StatusJob>()
                .WithIdentity("Status")
                .Build();

            var trigger = TriggerBuilder.Create()
                .WithIdentity("StatusCron")
                .StartNow()
                .WithCronSchedule("*/5 * * * * ?")
                .Build();

            var job2 = JobBuilder.Create<CameraStatusJob>()
                .WithIdentity("Status2")
                .Build();

            var trigger2 = TriggerBuilder.Create()
                .WithIdentity("StatusCron2")
                .StartNow()
                .WithCronSchedule("*/5 * * * * ?")
                .Build();
            var job3 = JobBuilder.Create<AutoRepairJob>()
                .WithIdentity("Status3")
                .Build();

            var trigger3 = TriggerBuilder.Create()
                .WithIdentity("StatusCron3")
                .StartNow()
                .WithCronSchedule("*/5 * * * * ?")
                .Build();
            var job4 = JobBuilder.Create<EleJob>()
                .WithIdentity("Status4")
                .Build();

            var trigger4 = TriggerBuilder.Create()
                .WithIdentity("StatusCron4")
                .StartNow()
                .WithCronSchedule("*/5 * * * * ?")
                .Build();
            _scheduler.ScheduleJob(job, trigger).Wait();
            _scheduler.ScheduleJob(job2, trigger2).Wait();
            _scheduler.ScheduleJob(job3, trigger3).Wait();
            _scheduler.ScheduleJob(job4, trigger4).Wait();
        }
    }
}
