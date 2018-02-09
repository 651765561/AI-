using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Topshelf;

namespace AiWin_New
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //HostFactory.Run(x =>
            //{
            //    x.Service<StartJob>(s =>
            //    {
            //        s.ConstructUsing(name => new StartJob());
            //        s.WhenStarted(tc => tc.Start());
            //        s.WhenStopped(tc => tc.Stop());
            //    });
            //    x.RunAsLocalSystem();

            //    x.SetDescription("智能机箱状态监测服务");
            //    x.SetDisplayName("智能机箱状态监测服务");
            //    x.SetServiceName("AiWin_New");
            //    x.EnablePauseAndContinue();
            //});

            HostFactory.Run(x =>
            {
                x.Service<QuartzServiceRunner>(s =>
                {
                    s.ConstructUsing(name => new QuartzServiceRunner());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();
                x.StartAutomatically();
                x.EnablePauseAndContinue();
                x.SetDescription("智能机箱状态监测服务");
                x.SetDisplayName("智能机箱状态监测服务");
                x.SetServiceName("AiWin_New");
            });
        }

    }
}


