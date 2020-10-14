using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCP_Import.IService
{
   public interface IProductService
    {
      
        Task<bool> ProcessXmlProducts(int sellerId);
        Task<Tuple<bool, string>> SaveSchedulerSettings(Models.Settings.SchedulerSettingModel settingModel);
        Task<Models.Settings.SchedulerSettingModel> GetSettingsByShopUrl(string shopUrl);
    }
}
