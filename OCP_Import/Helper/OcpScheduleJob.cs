using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OCP_Import.Helper
{
    [DisallowConcurrentExecution]
    public class OcpScheduleJob : IJob
    {   // private static IProductSyncStgInterfaceService _productSyncStgInterface = new ProductSyncStgDataAccessService(new ProductSyncStgDataAccessRepository());
        private static long JobRunID = 0;
        public async Task Execute(IJobExecutionContext context)
        {

            try
            {
 //               FileHelper.WriteExceptionMessage("SteveMaddenStageUS", LogStatus: "INFO", LogErrorMessage: "SM Sync Inventory Quantity - Start");
                //  var result = _productSyncStgInterface.InsertJobHistory("SM Sync Inventory Quantity", CommonFunctions.GetESTDateTime(DateTime.UtcNow), "SHOPIFY_INVENTORY_QUANTITY_SM", 1, null);
                //  if (result != null && result.JobRunID.HasValue)
                //    JobRunID = result.JobRunID.Value;
 //               FileHelper.WriteExceptionMessage("SteveMaddenStageUS", LogStatus: "INFO", LogErrorMessage: "JobRunID - " + JobRunID);
                JobDataMap dataMap = context.JobDetail.JobDataMap;
 // execute method               await SMInventoryItem.SyncInventoryItemStg(JobRunID, 1, "https://stg-stevemadden.myshopify.com", "328ce2212388083571875b537277a2c1", CommonFunctions.GetESTDateTime(DateTime.UtcNow.AddHours(-(Convert.ToInt16(1)))), "SteveMaddenStageUS", "113");
            }
            catch (System.Exception ex)
            {
                string exception = ex.Message;
                if (ex.InnerException != null)
                    exception = ex.InnerException.Message;
                //  _productSyncStgInterface.sp_update_job_history(JobRunID, CommonFunctions.GetESTDateTime(DateTime.UtcNow), 0, null, exception);
 //               FileHelper.WriteExceptionMessage("SteveMaddenStageUS", LogStatus: "ERROR", LogErrorMessage: exception);
            }
            finally
            {
//               FileHelper.WriteExceptionMessage("SyncInventoryItemStg", LogStatus: "INFO", LogErrorMessage: "SM Sync Inventory Quantity - End");
            }

        }
    }
}