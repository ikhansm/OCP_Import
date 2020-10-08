using OCP_Import.Helper;
using ShopifySharp;
using ShopifySharp.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OCP_Import.Controllers
{
    public class ShopifyController : Controller
    {
        private readonly IService.ISellerService _iSellerService;
        public ShopifyController(IService.ISellerService iSellerService)
        {
            _iSellerService = iSellerService;
        }

        // GET: Shopify
        public async Task<ActionResult> Handshake(string shop)
        {
            
            if (!AuthorizationService.IsAuthenticRequest(Request.QueryString.ToKvps(), ApplicationEngine.ShopifySecretKeyPublicApp))
            {
                throw new Exception("Request is not authentic.");
            }
            else
            {
               Session["SellerInstall"] = shop;
                var cookie = new System.Web.HttpCookie(".App.Handshake.ShopUrl", shop)
                {
                    Expires = DateTime.Now.AddDays(2),
                    Secure = true
                };
                cookie.Path += ";SameSite=None";
                Response.SetCookie(cookie);
                var seller = await _iSellerService.GetSellerDetails(shop, "Active");
                if (seller != null)
                {
                    bool installedStatus = await _iSellerService.ValidateStoreInstalled(seller);
                    if (installedStatus == true)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else{
                        Session["SellerInstall"] = shop;

                        if (!await AuthorizationService.IsValidShopDomainAsync(shop))
                        {
                            ModelState.AddModelError("", "The URL you entered is not a valid *.myshopify.com URL.");

                            //Preserve the user's shopUrl so they don't have to type it in again.
                            //ViewBag.ShopUrl = shop;
                            throw new Exception("Invalid URL.");
                            //return View();
                        }
                        else
                        {
                            //Determine the permissions that your app will need and request them here.
                            var repermissions = new List<ShopifySharp.Enums.AuthorizationScope>()
                         {
                           ShopifySharp.Enums.AuthorizationScope.ReadOrders,
                          ShopifySharp.Enums.AuthorizationScope.WriteOrders,
                        
                            AuthorizationScope.ReadProducts,
                             AuthorizationScope.WriteProducts
                           
                          };
                            var reauthUrl = AuthorizationService.BuildAuthorizationUrl(repermissions, shop, ApplicationEngine.ShopifyApiKeyPublicApp, ApplicationEngine.RedirectUrl_HandShake);

                            return Redirect(Convert.ToString(reauthUrl));
                        }



                    }


                }
                else
                {
                    //Determine the permissions that your app will need and request them here.
                    var newpermissions = new List<ShopifySharp.Enums.AuthorizationScope>()
                         {
                           ShopifySharp.Enums.AuthorizationScope.ReadOrders,
                          ShopifySharp.Enums.AuthorizationScope.WriteOrders,
                          AuthorizationScope.ReadScriptTags,
                           AuthorizationScope.ReadThemes,
                            AuthorizationScope.ReadProducts,
                             AuthorizationScope.WriteProducts
                           
                          };
                   
                    //Build the authorization URL
                    var newauthUrl = AuthorizationService.BuildAuthorizationUrl(newpermissions, shop, ApplicationEngine.ShopifyApiKeyPublicApp, ApplicationEngine.RedirectUrl_HandShake);

                    //Redirect the user to the authorization URL
                    return Redirect(Convert.ToString(newauthUrl));
                }

            }
        }
        public async Task<ActionResult> AuthResult(string shop, string code)
        {

            var cookie = new System.Web.HttpCookie(".App.Handshake.ShopUrl", shop)
            {
                Expires = DateTime.Now.AddDays(2),
                Secure = true
            };
            cookie.Path += ";SameSite=None";
            Response.SetCookie(cookie);

            //  Response.SetCookie(new HttpCookie(".App.Handshake.ShopUrl", shop) { Expires = DateTime.Now.AddDays(2), SameSite=SameSiteMode.None,Secure=true });
          

            //Validate the signature of the request to ensure that it's valid
            if (!AuthorizationService.IsAuthenticRequest(Request.QueryString.ToKvps(), ApplicationEngine.ShopifySecretKeyPublicApp))
            {
                //The request is invalid and should not be processed.
                throw new Exception("Request is not authentic.");
            }
            else
            {
                //The request is valid. Exchange the temporary code for a permanent access token
                string accessToken;
                try
                {
                    accessToken = await AuthorizationService.Authorize(code, shop, ApplicationEngine.ShopifyApiKeyPublicApp, ApplicationEngine.ShopifySecretKeyPublicApp);



                    #region SaveDetailsToDBIfAccepted
                   
                        var shopService = new ShopService(shop, accessToken);
                        var shopDetails = await shopService.GetAsync();

                        //Save Seller Details to DB
                        await _iSellerService.SaveSellerDetails(shop, accessToken, shopDetails);
                        //Create the AppUninstalled webhook
                        await _iSellerService.CreateWebhook(shop, accessToken);

                 

                    #endregion






                }
                catch (Exception ex)
                {
                
                }
                Session["domain"] = shop; 
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<ActionResult> Charge()
        {
            RecurringCharge charge = new RecurringCharge()
            {
                Name = "OCP Import",
                Price = 0,
                ReturnUrl = ApplicationEngine.ReturnUrl_Charge,
                Test = true,
                TrialDays = 0
            };
           
            //Create the charge
            charge = await new RecurringChargeService(Convert.ToString(Session["domain"]), Convert.ToString(Session["token"])).CreateAsync(charge);
            ViewBag.ConfirmationURL = charge.ConfirmationUrl;
            return View();
            //return RedirectPermanent(charge.ConfirmationUrl);
        }

        public async Task<ActionResult> ChargeResult(string shop, long charge_id)
        {
            string domain = Convert.ToString(Session["domain"]), token = Convert.ToString(Session["token"]);

            if (domain == "" || token == "")
            {
                return RedirectToAction("Handshake", "Shopify", new { shop = shop });
            }

            var service = new RecurringChargeService(domain, token);

            RecurringCharge charge;

            //Try to get the charge. If a "404 Not Found" exception is thrown, the charge has been deleted.
            try
            {
                charge = await service.GetAsync(charge_id);
            }
            catch (ShopifyException e)
                when (e.Message.Equals("Not found", StringComparison.OrdinalIgnoreCase))
            {
                //The charge has been deleted. Redirect the user to accept a new charge.
                return RedirectToAction("Charge", "Shopify");
            }

            //Ensure the charge can be activated
            if (charge.Status != "accepted")
            {
                ViewBag.domain1 = domain;
                return View();
                //Charge has not been accepted. Redirect the user to accept a new charge.
                //return RedirectToAction("Charge", "Shopify");
            }

            #region SaveDetailsToDBIfAccepted
            if (charge.Status == "accepted")
            {
                var shopService = new ShopService(domain, token);
                var shopDetails = await shopService.GetAsync();

                //Save Seller Details to DB
                 await _iSellerService.SaveSellerDetails(domain, token, shopDetails);

                //Activate the charge
                await service.ActivateAsync(charge_id);

                //Create the AppUninstalled webhook
                await _iSellerService.CreateWebhook(domain, token);

            }

            #endregion

            return RedirectToAction("Index", "Home");
        }

        public async Task<string> AppUninstalled(string domain)
        {
            if (!await AuthorizationService.IsAuthenticWebhook(Request.Headers.ToKvps(), Request.InputStream, ApplicationEngine.ShopifySecretKeyPublicApp))
            {
                throw new UnauthorizedAccessException("This request is not an authentic webhook request.");
            }
            try
            {
                var seller =await _iSellerService.GetSellerDetails(domain,"Active");
                if (seller != null)
                {
                    seller.InstallStatus = "InActive";
                    seller.UnInstallDateTime = DateTime.Now;
                    await _iSellerService.UpdateSellerDetails(seller);

                }
            }
            catch (Exception ex)
            {
            //    LogResult.WriteErrorToDB(ex.Message, ex.StackTrace, domain, "error occured in app uninstalled webhook");
            }

            return "Handled AppUninstalled Webhook.";
        }

      


    }
}