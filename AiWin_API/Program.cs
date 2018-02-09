using System;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Topshelf;

namespace AiWin_API
{
    public class Program
    {
  
        public static void Main(string[] args)
        {
            var exePath = Process.GetCurrentProcess().MainModule.FileName;
            var directoryPath = Path.GetDirectoryName(exePath);
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(directoryPath)
                .UseStartup<Startup>()
                .UseUrls("http://*:9000")
                .Build();
            //host.Run();

            host.RunAsService();

        }
      
    }


}


