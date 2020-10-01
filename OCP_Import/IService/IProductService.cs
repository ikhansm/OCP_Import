using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCP_Import.IService
{
   public interface IProductService
    {
        Helper.ProductCatalogImport ReadCSV();
        Task<ShopifySharp.Product> CreateProduct(Helper.Product product, string vendor);
        Task<bool> ProcessXmlProducts();
        Task<Tuple<bool, string>> SaveSchedulerSettings(Models.Settings.SchedulerSettingModel settingModel);
    }
}
