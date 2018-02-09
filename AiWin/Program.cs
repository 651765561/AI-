using System.IO;
using System.Diagnostics;
using System.Threading;
using Topshelf;

namespace AiWin
{
    public class Program
    {

        public class StartJob
        {
            private static System.Timers.Timer _timer;
            private static int _inTimer = 0;
            public StartJob()
            {
                _timer = new System.Timers.Timer
                {
                    Interval = 1000D,
                    Enabled = true,
                    AutoReset = true
                };
                _timer.Elapsed += (sender, a) =>
                {
                    if (Interlocked.Exchange(ref _inTimer, 1) == 0)
                    {
                        CrossingService.InSertDb();
                        Interlocked.Exchange(ref _inTimer, 0);
                    }
                       
                };
            }
            public void Start() { _timer.Start(); }
            public void Stop() { _timer.Stop(); }
        }

        public static void Main(string[] args)
        {
           
       
            HostFactory.Run(x =>
            {
                x.Service<StartJob>(s =>
                {
                    s.ConstructUsing(name => new StartJob());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();

                x.SetDescription("智能机箱状态监测服务");
                x.SetDisplayName("智能机箱状态监测服务");
                x.SetServiceName("AiWin");
            });


        }

    }


}


