using System;
using System.Threading.Tasks;
using Quartz;
using Quartz.Impl;

namespace QuartzApp
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

            ISimpleTrigger simpleTrigger = (ISimpleTrigger)TriggerBuilder.Create()
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInMinutes(1)
                    .RepeatForever())
                .EndAt(DateBuilder.DateOf(22, 0, 0))//晚上22点结束
                .Build();

            ITrigger trigger1 = TriggerBuilder.Create()
                .WithCronSchedule("0 0/2 8-17 * * ?")//每天早上8点到下午5点建立一个触发器，每隔一分钟就会触发一次：
                .Build();

            ITrigger trigger2 = TriggerBuilder.Create()
                .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(10, 42))//每天在上午10:42开始执行
                .Build();
        }
    }

    public class HelloWorldJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            try
            {
                return Task.Run(() => Console.WriteLine($"hello world at time {DateTime.Now}"));
            }
            catch (JobExecutionException ex)
            {
                throw ex;
            }
        }
    }

    /*
    表达式举例
    "0 0 12 * * ?" 每天中午12点触发
    "0 15 10 ? * *" 每天上午10:15触发
    "0 15 10 * * ?" 每天上午10:15触发
    "0 15 10 * * ? *" 每天上午10:15触发
    "0 15 10 * * ? 2005" 2005年的每天上午10:15触发
    "0 * 14 * * ?" 在每天下午2点到下午2:59期间的每1分钟触发
    "0 0/5 14 * * ?" 在每天下午2点到下午2:55期间的每5分钟触发
    "0 0/5 14,18 * * ?" 在每天下午2点到2:55期间和下午6点到6:55期间的每5分钟触发
    "0 0-5 14 * * ?" 在每天下午2点到下午2:05期间的每1分钟触发
    "0 10,44 14 ? 3 WED" 每年三月的星期三的下午2:10和2:44触发
    "0 15 10 ? * MON-FRI" 周一至周五的上午10:15触发
    "0 15 10 15 * ?" 每月15日上午10:15触发
    "0 15 10 L * ?" 每月最后一日的上午10:15触发
    "0 15 10 ? * 6L" 每月的最后一个星期五上午10:15触发
    "0 15 10 ? * 6L 2002-2005" 2002年至2005年的每月的最后一个星期五上午10:15触发
    "0 15 10 ? * 6#3" 每月的第三个星期五上午10:15触发


    作者：骄傲牛
    链接：https://www.jianshu.com/p/e9ce1a7e1ed1
    來源：简书
    简书著作权归作者所有，任何形式的转载都请联系作者获得授权并注明出处。
    */
}
