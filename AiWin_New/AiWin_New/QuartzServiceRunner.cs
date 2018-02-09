using System.Configuration;
using Quartz;
using Quartz.Impl;
using Topshelf;

namespace AiWin_New
{
    class QuartzServiceRunner
    {
        private readonly IScheduler _scheduler;

        public QuartzServiceRunner()
        {
            _scheduler = StdSchedulerFactory.GetDefaultScheduler();
        }

        public void Start()
        {
            //从配置文件中读取任务启动时间
            string cronExpr = ConfigurationManager.AppSettings["cronExpr"];
            IJobDetail job = JobBuilder.Create<StartJob>().WithIdentity("job1", "group1").Build();
            //创建任务运行的触发器
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("triggger1", "group1")
                .WithSchedule(CronScheduleBuilder.CronSchedule(new CronExpression(cronExpr)))
                .Build();
            //启动任务
            _scheduler.ScheduleJob(job, trigger);
            _scheduler.Start();

        }

        public void Stop()
        {
            _scheduler.Clear();
           
        }

        public bool Continue(HostControl hostControl)
        {
            _scheduler.ResumeAll();
            return true;
        }

        public bool Pause(HostControl hostControl)
        {
            _scheduler.PauseAll();
            return true;
        }

    }
}
