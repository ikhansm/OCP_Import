using Quartz;
using System;
using System.Threading.Tasks;

namespace LogMaker
{
    [DisallowConcurrentExecution]
    public class Jobclass : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.JobDetail.JobDataMap;
            try
            {
                 Log.RemoveLogsFile();
            }
            catch (Exception ex)
            {
       //         FileHelper.WriteExceptionMessage("CommonFunctions/Jobclass : " + ex.InnerException != null ? ex.InnerException.ToString() : ex.ToString());
            }
        }
    }
}
