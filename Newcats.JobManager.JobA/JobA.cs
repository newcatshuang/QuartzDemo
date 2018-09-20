using System;
using System.Threading.Tasks;
using Quartz;

namespace Newcats.JobManager.JobA
{
    public class JobA : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"hello at JobA at {DateTime.Now}");
            return Task.CompletedTask;
        }
    }
}
