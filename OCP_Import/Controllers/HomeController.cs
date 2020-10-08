using System;
using System.Collections.Generic;
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
            if (data == null)
            {
                data = new Models.Settings.SchedulerSettingModel();
                data.brand = "";
            }
            // var data =await _iProductService.ProcessXmlProducts();
          //   Service.ProductService ps = new Service.ProductService();
         //   ps.DownloadFileSFTP();
            //  await Helper.ProcessPendingFilesSchedule.ProcessPendingFilesScheduleJobSync("1", 1);

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