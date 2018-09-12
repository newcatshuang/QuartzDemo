using System.Diagnostics;
using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Logging;

namespace Newcats.HangFire.HostServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
            //CreateWebHostBuilder(args).Build().RunAsService();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            string pathToExe = Process.GetCurrentProcess().MainModule.FileName;
            string pathToContentRoot = Path.GetDirectoryName(pathToExe);
            return WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddFilter("System", LogLevel.Warning);
                    logging.AddFilter("Microsoft", LogLevel.Warning);
                    logging.AddLog4Net();//自动记录全局的异常日志（不需要自己写全局异常过滤器记录）
                })
                //.UseContentRoot(pathToContentRoot)
                .UseStartup<Startup>();
        }
    }
}
