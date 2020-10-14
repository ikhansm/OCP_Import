using OCP_Import.Service;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.SqlTypes;
using System.IO;
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



    public class ProcessPendingFilesSchedule
    {
        public async static Task ProcessPendingFilesScheduleJobSync(int fileType, TimeSpan Interval)
        {
            string prefix = "processpendingfile";
            string JobName = prefix + "Job_" + fileType;
            string JobTrigger = prefix + "Trigger_" + fileType;
            string JobGroup = prefix + "Group";
            var jobKey = JobKey.Create(JobName, JobGroup);
            ISchedulerFactory schedFact = new StdSchedulerFactory();
            IScheduler sched =await schedFact.GetScheduler();
            // define the job
            IJobDetail job = JobBuilder.Create<Processforuplodingfile>().WithIdentity(JobName, JobGroup).Build();
            // define the trigger
            ITrigger ProcessforUplodingfileSyncTrigger = TriggerBuilder.Create()
             .WithIdentity(JobTrigger, JobGroup)
             .StartNow()
             //.WithSimpleSchedule(x => x.WithInterval(Interval).RepeatForever())
            // .Build();
              .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(Interval.Hours, Interval.Minutes))
              .ForJob(jobKey)
               .Build();



            job.JobDataMap["fileType"] = fileType;
           await sched.ScheduleJob(job, ProcessforUplodingfileSyncTrigger);
           await sched.Start();
        }

        public async static Task CancelSchedulerJob(int jobId)
        {

            string prefix = "processpendingfile";
            string JobName = prefix + "Job_" + jobId;
            string JobGroup = prefix + "Group";
            ISchedulerFactory schedFact = new StdSchedulerFactory();
            IScheduler sched =await schedFact.GetScheduler();
            var jobKey = JobKey.Create(JobName,JobGroup);
            var jobExist =await sched.CheckExists(jobKey);
            if (jobExist)
            {

              await  sched.DeleteJob(jobKey);
            
            }


        }


    }

    [DisallowConcurrentExecution]
    internal class Processforuplodingfile : IJob
    {
        private static long JobRunID = 0;
        public async Task Execute(IJobExecutionContext context)
        {

            try
            {

                var jobName = context.JobDetail.Key.Name;
                int jobId = Convert.ToInt32(jobName.Replace("processpendingfileJob_",""));

                ProductService p = new ProductService();
                await  p.SyncProducts(jobId);
                
                //var appPath=  System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
                //var fPath = Path.Combine(appPath, "DownloadFile/test.txt");
                //Helper.Utility.appendToFile(fPath);

            }
            catch (System.Exception ex)
            {
                string exception = ex.Message;
                if (ex.InnerException != null)
                    exception = ex.InnerException.Message;
                    LoggerFunctions.FileHelper.WriteExceptionMessage("Global", "UpdateSellerDetails", "SellerService.cs", "", exception);

            }
            finally
            {
            }

        }
    
    }





}