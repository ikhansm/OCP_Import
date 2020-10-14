using ShopifySharp;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using OCP_Import.Helper;
using System.Net;
using System.IO;
using Unity;

namespace OCP_Import.Service
{
    public class ProductService:IService.IProductService,IDisposable
    {
        private Models.EDM.db_OCP_ImportEntities db = new Models.EDM.db_OCP_ImportEntities();

        public async Task<ShopifySharp.Product> CreateProduct(ImportServices.Wrapper.Product product, string vendor,string myShopifyDomain, string accessToken,ImportServices.Wrapper.ColorList colorMapping)
        {
            ShopifySharp.ProductService service = new ShopifySharp.ProductService(myShopifyDomain, accessToken);

            var newProduct = new ShopifySharp.Product()
            {
                 Vendor = vendor,
            };
            newProduct.Options = new List<ProductOption> { new ProductOption { Name = "Color" }, new ProductOption { Name = "Size" } };
            string productTitle = product.Name;
            string productHandle = product.Name;
            var colorList = product.ProductVariants.ProductVariant.Select(x => new { colorName = x.ColorName, colorCode = x.ColorCode }).ToList();
            var distColorList = colorList.Distinct().Select(x => x).ToList();

            string colorName = colorList.Distinct().FirstOrDefault().colorName;
            if (!string.IsNullOrEmpty(colorName))
            {
                colorName= CheckColorMapping(colorMapping,colorName);
            
                productHandle += "-" + colorName;
                productTitle += " " + colorName;
            }
            newProduct.Handle = productHandle.ToLower();
            newProduct.Title = productTitle.ToUpper();
            var productTags = new List<string>();
            foreach (var c in distColorList)
            {
                string _color = CheckColorMapping(colorMapping, c.colorName);
                productTags.Add(product.Name.ToLower() + "-" + _color.ToLower());


            }

            var _pvList = new List<ShopifySharp.ProductVariant>();
            var priceTagList = new List<string>();

            foreach (var pv in product.ProductVariants.ProductVariant)
            {
                var _pv = new ShopifySharp.ProductVariant();
                string variant_color = CheckColorMapping(colorMapping, pv.ColorName);
                _pv.SKU = product.Name.Replace(" ", "-").ToUpper();
                _pv.Option1 = variant_color;
                _pv.Option2 = pv.SizeName;
                _pv.Option3 = pv.ColorCode;
                _pv.TaxCode = "PC040144";
                _pv.Title = $@"{pv.ColorName.ToUpper()} /\ {pv.SizeName} /\ {variant_color}";
                _pv.Price = Convert.ToDecimal(pv.ProductVariantSources.ProductVariantSource.Price);
                _pv.InventoryQuantity = Convert.ToInt32(pv.Quantity);
                _pvList.Add(_pv);
                if (_pv.Price < 25)
                {
                    priceTagList.Add("PRICE_under_$25");
                }
                else if (_pv.Price >= 25 && _pv.Price <= 50)
                {
                    priceTagList.Add("PRICE_$25-$50");
                }
                else if (_pv.Price >= 50 && _pv.Price <= 100)
                {
                    priceTagList.Add("PRICE_$50-$100");
                }
                else if (_pv.Price >= 100 && _pv.Price <= 150)
                {
                    priceTagList.Add("PRICE_$100-$150");
                }

            }
            var productVariants=  _pvList.DistinctBy(x => x.Title).Select(x => x).ToList();


            var exist =await GetProductByHandle(newProduct.Handle, service);

            if (exist != null)
            {
              
                foreach (var p in exist)
                {

                    var existingPV = p.Variants;
                    var result = from x in productVariants
                                 join y in existingPV
                    on new { X1 = x.Option1, X2 = x.Option2 } equals new { X1 = y.Option1, X2 = y.Option2 } select x;
                    var newVariants = productVariants.Where(p1 => !result.Any(p2 => p2.Option1 == p1.Option1 && p2.Option2 == p1.Option2));
                    if (newVariants != null)
                    {
                        ShopifySharp.ProductVariantService pvs = new ProductVariantService(myShopifyDomain, accessToken);

                        foreach (var nvp in newVariants)
                        {
                            var variant = await pvs.CreateAsync(p.Id??0, nvp);
                        }

                    }

                }

            }
            else
            {



                newProduct.Variants = productVariants;
                foreach (var p in priceTagList.Distinct().Select(x => x).ToList())
                {
                    productTags.Add(p.ToLower());

                }
                newProduct.Tags = string.Join(",", productTags.ToArray());

                var createdDate = product.Attributes.Attribute.Where(x => x.ProductAttributeCode == "createddate").Select(x => x.ProductAttributeValue).FirstOrDefault();
                if (createdDate != null)
                {
                    newProduct.CreatedAt = Convert.ToDateTime(createdDate);

                }







                try
                {
                    newProduct = await service.CreateAsync(newProduct);
                }
                catch (Exception ex)
                {
                    string exception = ex.Message;
                    if (ex.InnerException != null)
                        exception = ex.InnerException.Message;
                    LoggerFunctions.FileHelper.WriteExceptionMessage("Global", "Error in CreateProduct method ", "ProductService.cs", "ERROR", exception);


                }
            }
            return newProduct;
        }

        public string CheckColorMapping(ImportServices.Wrapper.ColorList clist, string colorName)
        {
            if (clist.ColorMapping.Where(x => x.COLOR_NAME == colorName.ToUpper()).Count() > 0)
            {
                colorName = clist.ColorMapping.Where(x => x.COLOR_NAME == colorName.ToUpper()).FirstOrDefault().WEB_FRIENDLY_COLOR_NAME;
            }

           return colorName.ToUpper();
        }
        public async Task<IEnumerable<ShopifySharp.Product>> GetProductByHandle(string handle, ShopifySharp.ProductService service)
        {
            ShopifySharp.Filters.ProductListFilter filter = new ShopifySharp.Filters.ProductListFilter();
            filter.Handle = handle;
            var data =await service.ListAsync(filter);
            return data.Items;
        }

        public async Task<bool> ProcessXmlProducts(int sellerId) {
            var seller =await db.tblSellers.Where(x => x.SellerId == sellerId).FirstOrDefaultAsync();
            var container = new Unity.UnityContainer();
           var sst= container.Resolve<ImportServices.Service.Service>();
            var xmlData =sst.ReadCSV(sellerId);
            var colorMappingList = sst.GetColorFamily("ColorMapping/COLOR_MAPPING.xml");
            var result = false;
            var vendor = xmlData.ProductAttributeGroups.ProductAttributeGroup.ProductAttributeGroupCode;
            var plist = xmlData.Products.Product;
            foreach (var p in plist)
            {
                try
                {
                    var _p = await CreateProduct(p, vendor, seller.MyShopifyDomain,seller.ShopifyAccessToken, colorMappingList);
                }
                catch (Exception ex)
                {

                    result = true;
                    string exception = ex.Message;
                    if (ex.InnerException != null)
                        exception = ex.InnerException.Message;
                    LoggerFunctions.FileHelper.WriteExceptionMessage("Global", "Error in ProcessXmlProducts method ", "ProductService.cs", "ERROR", exception);



                }
            }

            return result;
        
        }

        public async Task<Models.Settings.SchedulerSettingModel> GetSettingsByShopUrl(string shopUrl)
        {
            var data = await (from s in db.tblSellers
                              join ss in db.tblSchedulerSettings on
                                s.SellerId equals ss.SellerId into gj
                              from subpet in gj.DefaultIfEmpty()
                              where s.ShopDomain == shopUrl
                              select new Models.Settings.SchedulerSettingModel {
                              brand= subpet.Brand,
                              ftpFilePath= subpet.FtpFilePath,
                              ftpHost= subpet.FtpHost,
                              ftpPassword= subpet.FtpPassword,
                              ftpPort= subpet.FtpPort,
                              ftpUserName= subpet.FtpUserName,
                              sellerId=s.SellerId,
                               syncTime= subpet.SyncTime,


                              }).FirstOrDefaultAsync();

            return data;

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
                     result = false;
                    resultMessage = ex.Message;
                }
            }
            else
            {
                try
                {
                    settings = new Models.EDM.tblSchedulerSetting();
                settings.Brand = settingModel.brand;
                settings.FtpFilePath = settingModel.ftpFilePath;
                settings.FtpHost = settingModel.ftpHost;
                settings.FtpPassword = settingModel.ftpPassword;
                settings.FtpPort = settingModel.ftpPort;
                settings.FtpUserName = settingModel.ftpUserName;
                settings.SyncTime = settingModel.syncTime;
                settings.UpdateAt = DateTime.Now;
                    settings.SellerId = settingModel.sellerId;
                db.tblSchedulerSettings.Add(settings);
                    await db.SaveChangesAsync();
                    resultMessage = "success";
                }
                catch (Exception ex)
                {
                     result = false;
                    resultMessage = ex.Message;
                    string exception = ex.Message;
                    if (ex.InnerException != null)
                        exception = ex.InnerException.Message;
                    LoggerFunctions.FileHelper.WriteExceptionMessage("Global", "Error in SaveSchedulerSettings method while updating scheduler settings data", "ProductService.cs", "ERROR", exception);

                }


            }

            try
            {
              await  CreateScheduler(settingModel.syncTime, settingModel.sellerId);
                resultMessage = "success";
            }
            catch (Exception ex)
            {
                result = false;
                resultMessage = ex.Message;
                string exception = ex.Message;
                if (ex.InnerException != null)
                    exception = ex.InnerException.Message;
                LoggerFunctions.FileHelper.WriteExceptionMessage("Global", "Error in SaveSchedulerSettings while creating scheduler", "ProductService.cs", "ERROR", exception);

            }


            return Tuple.Create(result, resultMessage);
        
        }

        public async Task SyncProducts(int sellerId)
        {
            try
            {
                var container = new Unity.UnityContainer();
                var _importService = container.Resolve<ImportServices.Service.Service>();
                var seller = await db.tblSchedulerSettings.Where(x => x.SellerId == sellerId).FirstOrDefaultAsync();
                var downloadResult = _importService.DownloadFileSFTP(seller.FtpHost, seller.FtpUserName, seller.FtpPassword, seller.FtpFilePath, sellerId);

                if (downloadResult == true)
                {

                    await ProcessXmlProducts(sellerId);

                }
            }
            catch (Exception ex)
            {

                string exception = ex.Message;
                if (ex.InnerException != null)
                    exception = ex.InnerException.Message;
                LoggerFunctions.FileHelper.WriteExceptionMessage("Global", "Error in SyncProducts method ", "ProductService.cs", "ERROR", exception);


            }


        }

        public async Task CreateScheduler(string time, int jobId)
        {
           await ProcessPendingFilesSchedule.CancelSchedulerJob(jobId);

            var datatime = Convert.ToDateTime(time);
            var tSpan = new TimeSpan(datatime.Hour, datatime.Minute, datatime.Second);

           await ProcessPendingFilesSchedule.ProcessPendingFilesScheduleJobSync(jobId, tSpan);



        }


        public async Task<bool> ReScheduleAllJobs()
        {
            try
            {
                var sellers = db.tblSchedulerSettings.ToList();
                foreach (var s in sellers)
                {
                    await CreateScheduler(s.SyncTime, s.tblSeller.SellerId);
                }
                return true;
            }
            catch (Exception ex)
            {
                string exception = ex.Message;
                if (ex.InnerException != null)
                    exception = ex.InnerException.Message;
                LoggerFunctions.FileHelper.WriteExceptionMessage("Global", "Error in ReScheduleAllJobs while ReScheduling scheduler", "ProductService.cs", "ERROR", exception);

                return false;
            }
            
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