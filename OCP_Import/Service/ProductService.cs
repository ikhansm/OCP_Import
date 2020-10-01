using ShopifySharp;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OCP_Import.Service
{
    public class ProductService:IService.IProductService,IDisposable
    {
        private Models.EDM.db_OCP_ImportEntities db = new Models.EDM.db_OCP_ImportEntities();


        public Helper.ProductCatalogImport ReadCSV()
        {

            var h = new OCP_Import.Helper.Utility();
            var filePath = HttpContext.Current.Server.MapPath("~/xmldata/catalog.xml");
            var products = h.DeserializeToObject<Helper.ProductCatalogImport>(filePath);

            return products;
        }


        public async Task<ShopifySharp.Product> CreateProduct(Helper.Product product,string vendor)
        {
            ShopifySharp.ProductService service = new ShopifySharp.ProductService("ocp-import.myshopify.com", "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");

            var newProduct = new ShopifySharp.Product()
            {
                Title = product.Name,
                Vendor = vendor,
                ProductType = "ocpimport",
                //Variants
            };

            var _pvList =new List<ShopifySharp.ProductVariant>();

            foreach (var pv in product.ProductVariants.ProductVariant)
            {
                var _pv = new ShopifySharp.ProductVariant();
                _pv.Option1 = pv.ColorName +" "+ pv.SizeName;
                _pv.SKU = pv.Sku;
                _pv.Price = Convert.ToDecimal(pv.ProductVariantSources.ProductVariantSource.Price);
                _pv.InventoryQuantity =Convert.ToInt32(pv.Quantity);
                _pvList.Add(_pv);

            }
            newProduct.Variants = _pvList.Take(3).ToList();
            var createdDate = product.Attributes.Attribute.Where(x => x.ProductAttributeCode == "createddate").Select(x => x.ProductAttributeValue).FirstOrDefault();
            if (createdDate != null)
            {
                newProduct.CreatedAt = Convert.ToDateTime(createdDate);

            }
            try
            {
                newProduct= await service.CreateAsync(newProduct);
            } catch (Exception ex)
            { 
            
            }
            return newProduct;
        }


        public async Task<bool> ProcessXmlProducts() {

            var xmlData = ReadCSV();
            var result = false;
            var vendor = xmlData.ProductAttributeGroups.ProductAttributeGroup.ProductAttributeGroupCode;
            var plist = xmlData.Products.Product.Take(5);
            foreach (var p in plist)
            {
                try
                {
                    var _p = await CreateProduct(p, vendor);
                }
                catch (Exception ex)
                {
                    result = true;
                }
                }

            return result;
        
        }



        public async Task<Tuple<bool, string>> SaveSchedulerSettings(Models.Settings.SchedulerSettingModel settingModel)
        {
            bool result = true;
            string resultMessage = string.Empty;

            var settings =await db.tblSchedulerSettings.Where(x => x.SellerId == settingModel.sellerId).FirstOrDefaultAsync();
            if (settings != null)
            {
                try { 
                settings.Brand = settingModel.brand;
                settings.FtpFilePath = settingModel.ftpFilePath;
                settings.FtpHost = settingModel.ftpHost;
                settings.FtpPassword = settingModel.ftpPassword;
                settings.FtpPort = settingModel.ftpPort;
                settings.FtpUserName = settingModel.ftpUserName;
                settings.SyncTime = settingModel.syncTime;
                settings.UpdateAt = DateTime.Now;
                db.Entry(settings).State = EntityState.Modified;
                await db.SaveChangesAsync();
                    resultMessage = "success";
                }
                catch (Exception ex)
                {
                    resultMessage = ex.Message;
                }
            }

            return Tuple.Create(result, resultMessage);
        
        }


        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~VerifyFohf_Cash() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }





        #endregion



    }
}