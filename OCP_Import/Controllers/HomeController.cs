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




        public ActionResult Index()
        {

       //     var data = _iProductService.ProcessXmlProducts();



            return View();
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