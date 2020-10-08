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
using Renci.SshNet;
using Renci.SshNet.Sftp;
namespace OCP_Import.Service
{
    public class ProductService:IService.IProductService,IDisposable
    {
        private Models.EDM.db_OCP_ImportEntities db = new Models.EDM.db_OCP_ImportEntities();


        public Helper.ProductCatalogImport ReadCSV(int sellerId)
        {

            var h = new OCP_Import.Helper.Utility();
            var appPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            var filePath = Path.Combine(appPath, "DownloadFile/Downloaded_" + sellerId + ".xml");
            //var filePath = HttpContext.Current.Server.MapPath("~/DownloadFile/Downloaded_"+ sellerId + ".xml");
            var products = h.DeserializeToObject<Helper.ProductCatalogImport>(filePath);

            return products;
        }

        public async Task<ShopifySharp.Product> CreateProduct(Helper.Product product, string vendor,string myShopifyDomain, string accessToken)
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
                productHandle += "-" + colorName;
                productTitle += " " + colorName;
            }
            newProduct.Handle = productHandle.ToLower();
            newProduct.Title = productTitle.ToUpper();

            var productTags = new List<string>();
            foreach (var c in distColorList)
            {
                productTags.Add(product.Name.ToLower() + "-" + c.colorName.ToLower());


            }

            var _pvList = new List<ShopifySharp.ProductVariant>();
            var priceTagList = new List<string>();

            foreach (var pv in product.ProductVariants.ProductVariant)
            {
                var _pv = new ShopifySharp.ProductVariant();
          
                _pv.SKU = product.Name.Replace(" ", "-").ToUpper();
                _pv.Option1 = pv.ColorName.ToUpper();
                _pv.Option2 = pv.SizeName;
                _pv.Option3 = pv.ColorCode;
                _pv.TaxCode = "PC040144";
                _pv.Title = $@"{pv.ColorName.ToUpper()} /\ {pv.SizeName} /\ {pv.ColorCode}";

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
            newProduct.Variants = _pvList.DistinctBy(x => x.Title).Select(x => x).ToList();
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

            }
            return newProduct;
        }


        public async Task<bool> ProcessXmlProducts(int sellerId) {
            var seller =await db.tblSellers.Where(x => x.SellerId == sellerId).FirstOrDefaultAsync();
            var xmlData = ReadCSV(sellerId);
            var result = false;
            var vendor = xmlData.ProductAttributeGroups.ProductAttributeGroup.ProductAttributeGroupCode;
            var plist = xmlData.Products.Product.Take(5);
            foreach (var p in plist)
            {
                try
                {
                    var _p = await CreateProduct(p, vendor, seller.MyShopifyDomain,seller.ShopifyAccessToken);
                }
                catch (Exception ex)
                {
                    result = true;
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
            }


            return Tuple.Create(result, resultMessage);
        
        }


        public void DownloadFileFTP() {
            try
            {

                String RemoteFtpPath = $@"{ApplicationEngine.FTP_Host+ApplicationEngine.FTP_File_Path}/test.txt";
                String LocalDestinationPath = HttpContext.Current.Server.MapPath("~/DownloadFile");
                String Username = ApplicationEngine.FTP_User_Name;
                String Password = ApplicationEngine.FTP_Password;

                //   Boolean UsePassive = true;

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(RemoteFtpPath));
                // request.EnableSsl = false;
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Proxy = null;

                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = true;
                System.Net.ServicePointManager.Expect100Continue = false;
                request.Credentials = new NetworkCredential(Username, Password);

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);

                using (FileStream writer = new FileStream(LocalDestinationPath, FileMode.Create))
                {

                    long length = response.ContentLength;
                    int bufferSize = 2048;
                    int readCount;
                    byte[] buffer = new byte[2048];

                    readCount = responseStream.Read(buffer, 0, bufferSize);
                    while (readCount > 0)
                    {
                        writer.Write(buffer, 0, readCount);
                        readCount = responseStream.Read(buffer, 0, bufferSize);
                    }
                }

                reader.Close();
                response.Close();
            }
            catch (Exception ex)
            { }
            //       String LocalDestinationPath = HttpContext.Current.Server.MapPath("~/DownloadFile");
            //         Helper.Utility.FTPDownload(LocalDestinationPath, "test.txt", ApplicationEngine.FTP_Host+ ApplicationEngine.FTP_File_Path+ "/test.txt", ApplicationEngine.FTP_User_Name, ApplicationEngine.FTP_Password);
        }


        public bool DownloadFileSFTP(string host, string username,string password, string pathRemoteFile,int sellerId)
        {
            bool downloadresult = false;
            // Path to file on SFTP server
          //   string pathRemoteFile = $@"{ApplicationEngine.FTP_File_Path}/test.txt";
            // Path where the file should be saved once downloaded (locally)
          //  string pathLocalFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "download_sftp_file.txt");
            //String pathLocalFile = HttpContext.Current.Server.MapPath("~/DownloadFile/Downloaded_"+ sellerId + ".xml");
            var appPath = System.Web.Hosting.HostingEnvironment.ApplicationPhysicalPath;
            var pathLocalFile = Path.Combine(appPath, "DownloadFile/Downloaded_" + sellerId + ".xml");
            if (!System.IO.File.Exists(pathLocalFile))
            {
                try
                {
                    System.IO.File.Create(pathLocalFile).Dispose();
                }catch(Exception ex) { }
            }

            using (SftpClient sftp = new SftpClient(host, username, password))
            {
                try
                {
                    sftp.Connect();

                //    Console.WriteLine("Downloading {0}", pathRemoteFile);

                    using (Stream fileStream = File.OpenWrite(pathLocalFile))
                    {
                        sftp.DownloadFile(pathRemoteFile, fileStream);
                        downloadresult = true;
                    }

                    sftp.Disconnect();
                }
                catch (Exception er)
                {
                 //   Console.WriteLine("An exception has been caught " + er.ToString());
                }
            }
            return downloadresult;
            //       String LocalDestinationPath = HttpContext.Current.Server.MapPath("~/DownloadFile");
            //         Helper.Utility.FTPDownload(LocalDestinationPath, "test.txt", ApplicationEngine.FTP_Host+ ApplicationEngine.FTP_File_Path+ "/test.txt", ApplicationEngine.FTP_User_Name, ApplicationEngine.FTP_Password);
        }


        public async Task SyncProducts(int sellerId)
        {

            var seller = await db.tblSchedulerSettings.Where(x => x.SellerId == sellerId).FirstOrDefaultAsync();
            var downloadResult = DownloadFileSFTP(seller.FtpHost, seller.FtpUserName, seller.FtpPassword, seller.FtpFilePath, sellerId);
            if (downloadResult == true)
            {

                await ProcessXmlProducts(sellerId);

            }



        }



        public async Task CreateScheduler(string time, int jobId)
        {
           await ProcessPendingFilesSchedule.CancelSchedulerJob(jobId);

            var datatime = Convert.ToDateTime(time);
            var tSpan = new TimeSpan(datatime.Hour, datatime.Minute, datatime.Second);

           await ProcessPendingFilesSchedule.ProcessPendingFilesScheduleJobSync(jobId, tSpan);



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