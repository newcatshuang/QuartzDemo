using System;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace App1
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
            Console.ReadKey();
        }

        private static async Task MainAsync()
        {
            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();//获取调度器
            await scheduler.Start();//启动调度器

            IJobDetail job = JobBuilder.Create<HelloWorldJob>().Build();//定义一个job

            //ISimpleTrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
            //    .StartNow()//立即开始
            //    .WithSimpleSchedule(x => x//使用简单调度器
            //        .WithIntervalInSeconds(2)//每2秒执行一次
            //        .RepeatForever())//一直循环
            //    .Build();

            //await scheduler.ScheduleJob(job, trigger);//等待执行任务

            //IDailyTimeIntervalTrigger dailyTrigger = (IDailyTimeIntervalTrigger)TriggerBuilder.Create()
            //    .StartNow()
            //    .WithDailyTimeIntervalSchedule(x => x
            //        .WithIntervalInSeconds(2)
            //        .WithRepeatCount(20))
            //    .Build();
            //await scheduler.ScheduleJob(job, dailyTrigger);

            ICronTrigger cronTrigger = (ICronTrigger)TriggerBuilder.Create()
                .StartNow()
                .WithCronSchedule("* * * * * ?")
                .Build();
            await scheduler.ScheduleJob(job, cronTrigger);
        }
    }

    public class HelloWorldJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() => Console.WriteLine($"hello world at time {DateTime.Now}"));
        }
    }
}
