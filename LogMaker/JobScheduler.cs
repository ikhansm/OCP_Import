using Quartz;
using System;
using Quartz.Impl;
using System.Threading.Tasks;
using System.Collections.Specialized;

namespace LogMaker
{
    public class JobScheduler
    {
        public static async Task Start()
        {
           // int RemoveFilesIntervalTime = (Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["RemoveFilesIntervalDay"].ToString()) * 24);
            // Grab the Scheduler instance from the Factory
            NameValueCollection props = new NameValueCollection
                {
                    { "quartz.serializer.type", "binary" }
                };
            StdSchedulerFactory factory = new StdSchedulerFactory(props);
            IScheduler scheduler = await factory.GetScheduler();
            // and start it off
            await scheduler.Start();
            // define the job and tie it to our Jobclass class
            IJobDetail job = JobBuilder.Create<Jobclass>()
                .WithIdentity("job1", "group1")
                .Build();
            // Trigger the job to run now, and then repeat every 10 seconds
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .StartNow()
                //.WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(9, 30))
                .WithSimpleSchedule(x => x
                .WithIntervalInSeconds(20)   
                //.WithIntervalInHours(RemoveFilesIntervalTime)
                    .RepeatForever())
                .Build();
            // Tell quartz to schedule the job using our trigger
            await scheduler.ScheduleJob(job, trigger);
        }

    }
}
