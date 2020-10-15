using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace OCP_Import.Controllers
{
    public class HomeController : Controller
    {

        private readonly IService.IProductService _iProductService;
        public HomeController(IService.IProductService iProductService)
        {
            _iProductService = iProductService;
        }




        public async Task<ActionResult> Index()
        {
            LoggerFunctions.FileHelper.WriteExceptionMessage("SM", LogStatus: "INFO", LogErrorMessage: "File Removed Successfully | file Name:Controller" );
            if (Session["SellerInstall"] != null)
            {
                ViewBag.shopOrigin = Session["SellerInstall"].ToString();
            }
             if (Session["domain"] != null)
            {
                ViewBag.shopOrigin = Session["domain"].ToString();
            }
            if (HttpContext.Request.Cookies[".App.Handshake.ShopUrl"] != null)
            {
                HttpCookie cookie = HttpContext.Request.Cookies.Get(".App.Handshake.ShopUrl");
                ViewBag.shopOrigin = cookie.Value;
            }

              Models.Settings.SchedulerSettingModel data = await _iProductService.GetSettingsByShopUrl(ViewBag.shopOrigin);
   //         Models.Settings.SchedulerSettingModel data = await _iProductService.GetSettingsByShopUrl("ocp-import.myshopify.com");

            if (data == null)
            {
                data = new Models.Settings.SchedulerSettingModel();
                data.brand = "";
            }
            // var data =await _iProductService.ProcessXmlProducts();
          //       Service.ProductService ps = new Service.ProductService();
            //   ps.DownloadFileSFTP();
            //  await Helper.ProcessPendingFilesSchedule.ProcessPendingFilesScheduleJobSync("1", 1);
            //   Models.EDM.db_OCP_ImportEntities db = new Models.EDM.db_OCP_ImportEntities();
            //   var seller = await db.tblSchedulerSettings.Where(x => x.SellerId == 5).FirstOrDefaultAsync();
            //   var downloadResult = ps.DownloadFileSFTP(seller.FtpHost, seller.FtpUserName, seller.FtpPassword, seller.FtpFilePath, 5);
            //       await ps.ProcessXmlProducts(5);
          //  await ps.ProcessXmlProducts(5);


            return View(data);
        }

        [HttpPost]
        public async Task<ActionResult> SaveSettings(Models.Settings.SchedulerSettingModel model) {

            if (ModelState.IsValid) {

              var data = await _iProductService.SaveSchedulerSettings(model);

                if (data.Item1 == true)
                {
                    return Json(new { res = "success" });
                }
                else
                {
                    return Json(new { res = "error",message =data.Item1});
                }

            }
            else
            {
                return Json(new { res = "error", message = "Enter all fields." });
            }

          
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}