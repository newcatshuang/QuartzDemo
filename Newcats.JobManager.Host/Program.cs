using System;
using System.IO;
using log4net.Config;
using Topshelf;

namespace Newcats.JobManager.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            FileInfo logConfig = new FileInfo($"{AppDomain.CurrentDomain.BaseDirectory}log4net.config");
            XmlConfigurator.ConfigureAndWatch(logConfig);

            HostFactory.Run(x =>
            {
                x.Service<JobServer>(s =>
                {
                    s.ConstructUsing(name => new JobServer());
                    s.WhenStarted(d => d.Start());
                    s.WhenStopped(d => d.Stop());
                });

                x.RunAsLocalSystem();

                x.SetDescription("JobManagerHostServer");
                x.SetDisplayName("JobManagerHostServer");
                x.SetServiceName("JobManagerHostServer");
            });

            //JobServer job = new JobServer();
            //job.Start();
            //Console.ReadKey();
        }
    }
}