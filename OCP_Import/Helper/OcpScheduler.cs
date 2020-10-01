using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OCP_Import.Helper
{
    public class OcpScheduler
    {
        public static async Task Start()
        {
            // Grab the Scheduler instance from the Factory
            NameValueCollection props = new NameValueCollection
            {
                { "quartz.serializer.type", "binary" }
            };
            StdSchedulerFactory factory = new StdSchedulerFactory(props);
            IScheduler scheduler = await factory.GetScheduler();

            // and start it off
            await scheduler.Start();

            // define the job and tie it to our HelloJob class
            IJobDetail job = JobBuilder.Create<OcpScheduleJob>()
                .WithIdentity("SMStgxstoreInventoryItemSchedulejob1", "SMStgxstoreInventoryItemSchedulegroup1")
                .Build();

            // Trigger the job to run now, and then repeat every 10 seconds
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("SMStgxstoreInventoryItemScheduletrigger1", "SMStgxstoreInventoryItemSchedulegroup1")
                .StartNow()
                .WithCronSchedule("0 5 0/1 1/1 * ? *")
                //
                //.WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(1, 0))
                //.WithSimpleSchedule(x => x
                //    .WithIntervalInHours(1)
                //    .RepeatForever())
                .Build();

            // Tell quartz to schedule the job using our trigger
            await scheduler.ScheduleJob(job, trigger);
        }
    }
}