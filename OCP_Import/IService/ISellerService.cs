using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OCP_Import.IService
{
   public interface ISellerService
    {
        Task<Models.EDM.tblSeller> GetSellerDetails(string shopDomain,string installStatus);
        Task UpdateSellerDetails(Models.EDM.tblSeller seller);

        Task<bool> ValidateStoreInstalled(Models.EDM.tblSeller storeData);
        Task SaveSellerDetails(string domain, string token, ShopifySharp.Shop shopDetails, long charge_id);
        Task<bool> CreateWebhook(string domain, string token);
        Helper.ProductCatalogImport ReadCSV();
    }
}
