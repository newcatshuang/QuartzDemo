using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;

namespace Newcats.JobManager.Host
{
    public class StoredProcedureJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            Console.WriteLine($"hello:{DateTime.Now.ToString()}");
            return Task.CompletedTask;
        }
    }
}
