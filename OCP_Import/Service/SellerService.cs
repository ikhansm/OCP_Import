using LogMaker;
using OCP_Import.Helper;
using OCP_Import.Models.EDM;
using ShopifySharp;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;


namespace OCP_Import.Service
{
    public class SellerService:IService.ISellerService, IDisposable
    {

        private Models.EDM.db_OCP_ImportEntities db = new Models.EDM.db_OCP_ImportEntities();

        public async Task<Models.EDM.tblSeller> GetSellerDetails(string shopDomain,string installStatus)
        {
            var data=  await db.tblSellers.Where(x => x.MyShopifyDomain == shopDomain && x.InstallStatus == installStatus).FirstOrDefaultAsync();
            return data;
        }
        public async Task<bool> ValidateStoreInstalled(Models.EDM.tblSeller storeData)
        {
            bool isExist = false;
            try
            {

                var shopService = new ShopifySharp.ShopService(storeData.ShopDomain, storeData.ShopifyAccessToken);
                var shopDetails = await shopService.GetAsync();
                isExist = true;
            }
            catch (ShopifySharp.ShopifyException ex)
            {
                if (ex.HttpStatusCode == System.Net.HttpStatusCode.Unauthorized && ex.Message == "Error: [API] Invalid API key or access token (unrecognized login or wrong password)")
                {
                    isExist = false;
                    Log.Error("Error in ValidateStoreInstalled method in class Sellerservice.cs", ex);
                }
                else
                {
                    isExist = true;
                }
            }

            return isExist;


        }
        public async  Task SaveSellerDetails(string domain, string token, Shop shopDetails)
        {
            tblSeller seller =await db.tblSellers.Where(x => x.MyShopifyDomain == domain).FirstOrDefaultAsync();
            try
            {
                if (seller == null)
                {
                    seller = new tblSeller();

                    seller.MyShopifyDomain = domain;
                    seller.ShopifyAccessToken = token;
                    seller.Email = shopDetails.Email;
                    seller.PhoneNumber = shopDetails.Phone;
                    seller.UserName = shopDetails.ShopOwner;
                    seller.InstallStatus = "Active";
                    seller.Host = domain.Replace(".myshopify", "");
                    seller.TimezoneOffset = GetTimezoneOffset(shopDetails.Timezone);
                    seller.ShopName = shopDetails.Name;
                    seller.ShopDomain = shopDetails.Domain;
                    seller.CreatedDateTime = DateTime.Now;
                    db.tblSellers.Add(seller);
                }
                else
                {
                    seller.MyShopifyDomain = domain;
                    seller.ShopifyAccessToken = token;
                    seller.Email = shopDetails.Email;
                    seller.PhoneNumber = shopDetails.Phone;
                    seller.UserName = shopDetails.ShopOwner;
                    seller.InstallStatus = "Active";
                    seller.Host = domain.Replace(".myshopify", "");
                    seller.TimezoneOffset = GetTimezoneOffset(shopDetails.Timezone);
                    seller.ShopName = shopDetails.Name;
                    seller.ShopDomain = shopDetails.Domain;
                    db.Entry(seller).State = EntityState.Modified;
                }


                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error("Error in SaveSellerDetails in SellerService.cs class", ex);
            }
        }

        public async  Task<bool> CreateWebhook(string domain, string token)
        {
            bool isSuccess = false;
            try
            {
                var serviceWebhook = new WebhookService(domain, token);
                var hook = new Webhook()
                {
                    Address = ApplicationEngine.Address_ChargeResult_UnInstall,
                    CreatedAt = DateTime.Now,
                    Format = "json",
                    Topic = "app/uninstalled",
                };
                hook = await serviceWebhook.CreateAsync(hook);

                var shopUpdateWebhook = new Webhook()
                {
                    Address = ApplicationEngine.Url_Path + "/api/services/updateshopdetails",
                    CreatedAt = DateTime.Now,
                    Format = "json",
                    Topic = "shop/update",
                };
                shopUpdateWebhook = await serviceWebhook.CreateAsync(shopUpdateWebhook);
                isSuccess = true;
            }
            catch (ShopifyException e)
            {
                Log.Error("Error on creating webhook", e);
                throw e;
            }
            return isSuccess;
        }

        public async Task UpdateSellerDetails(Models.EDM.tblSeller seller) {
            try
            {
                db.Entry(seller).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Log.Error("Error in UpdateSellerDetails in SellerService.cs", ex);
            }
        }

        public  string GetTimezoneOffset(string timezoneOffset)
        {
            timezoneOffset = timezoneOffset.Split(')')[0].Split('(')[1].Remove(0, 3);
            string plusOrMinus = Convert.ToString(timezoneOffset.First());
            timezoneOffset = timezoneOffset.Remove(0, 1);
            string[] timeArray = timezoneOffset.Split(':');
            int hourTym = Convert.ToInt32(timeArray[0]);
            int minTym = Convert.ToInt32(timeArray[1]);
            if (minTym == 0)
            {
                timezoneOffset = plusOrMinus + Convert.ToString(hourTym);
            }
            else if (minTym == 30)
            {
                timezoneOffset = plusOrMinus + Convert.ToString(hourTym) + ".5";
            }
            else if (minTym == 45)
            {
                timezoneOffset = plusOrMinus + Convert.ToString(hourTym) + ".75";
            }
            return timezoneOffset;
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