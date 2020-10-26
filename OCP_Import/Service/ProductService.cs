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
using LogMaker;

namespace OCP_Import.Service
{
    public class ProductService:IService.IProductService,IDisposable
    {
        private Models.EDM.db_OCP_ImportEntities db = new Models.EDM.db_OCP_ImportEntities();

        public async Task<ShopifySharp.Product> CreateProduct(ImportService.Wrapper.Product product, string vendor,string myShopifyDomain, string accessToken,ImportService.Wrapper.ColorList colorMapping)
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
            // product variant list created from ftp xml.
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

            if (exist.Count()>0)
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
                    Log.Error("Error in CreateProduct method in class ProductService.cs", ex, vendor);

                }
            }
            return newProduct;
        }

        public string CheckColorMapping(ImportService.Wrapper.ColorList clist, string colorName)
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
            
           var sst= container.Resolve<ImportService.Service.Service>();
            var xmlData =sst.ReadCSV(sellerId);
            var colorMappingList = sst.GetColorFamily("ColorMapping/COLOR_MAPPING.xml");
            var result = false;
            var vendor = xmlData.ProductAttributeGroups.ProductAttributeGroup.ProductAttributeGroupCode;
            var plist = xmlData.Products.Product;
            List<ImportService.Wrapper.Product> successList = new List<ImportService.Wrapper.Product>();
            List<ImportService.Wrapper.Product> errorList = new List<ImportService.Wrapper.Product>();

            foreach (var p in plist)
            {
                try
                {
                    var _p = await CreateProduct(p, vendor, seller.MyShopifyDomain,seller.ShopifyAccessToken, colorMappingList);
                    successList.Add(p);
                }
                catch (Exception ex)
                {
                    errorList.Add(p);
                    result = true;
                   
                    Log.Error("Error in ProcessXmlProducts method", ex, vendor);
                }

              

            }
           
            if (successList.Count >0)
            {
                  xmlData.Products.Product = new List<ImportService.Wrapper.Product>();
                  xmlData.Products.Product.AddRange(successList);
                  var sSetting = seller.tblSchedulerSettings.FirstOrDefault();
                    var filename = seller.MyShopifyDomain;
                  sst.CreateFileSFTP(sSetting.FtpHost, sSetting.FtpUserName, sSetting.FtpPassword, sSetting.FtpFilePath, "success", filename, xmlData);
                }
            if (errorList.Count>0)
            {
                xmlData.Products.Product = new List<ImportService.Wrapper.Product>();
                xmlData.Products.Product.AddRange(errorList);
                var sSetting = seller.tblSchedulerSettings.FirstOrDefault();
                var filename = seller.MyShopifyDomain;
                sst.CreateFileSFTP(sSetting.FtpHost, sSetting.FtpUserName, sSetting.FtpPassword, sSetting.FtpFilePath, "error", filename, xmlData);


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
                    Log.Error("Error in SaveSchedulerSettings method in class ProductService.cs while updating scheduler settings data", ex, settingModel.brand);

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
                    Log.Error("Error in SaveSchedulerSettings method in class ProductService.cs while creating new entry in settings during settings update", ex, settingModel.brand);
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
                Log.Error("Error in SaveSchedulerSettings method in class ProductService.cs while recreating scheduler during settings update", ex, settingModel.brand);

            }


            return Tuple.Create(result, resultMessage);
        
        }

        public async Task SyncProducts(int sellerId)
        {
            try
            {
                var container = new Unity.UnityContainer();
                var _importService = container.Resolve<ImportService.Service.Service>();
                var seller = await db.tblSchedulerSettings.Where(x => x.SellerId == sellerId).FirstOrDefaultAsync();
                var downloadResult = _importService.DownloadFileSFTP(seller.FtpHost, seller.FtpUserName, seller.FtpPassword, seller.FtpFilePath, sellerId);

                if (downloadResult.Item1 == true)
                {
                    await ProcessXmlProducts(sellerId);

                }
            }
            catch (Exception ex)
            {

                Log.Error("Error in SyncProducts method for sellerId = "+sellerId +" in class ProductService.cs ", ex);


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
                Log.Error("error in Rescheduling jobs during project startup", ex);

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