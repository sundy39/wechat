using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WeChat.Web.Controllers
{
    [RoutePrefix("Home")]
    [Route("{action=Index}")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [Route("About")]
        public ActionResult About()
        {
            ViewBag.Title = "About";

            return View();
        }

        [Route("Contact")]
        public ActionResult Contact()
        {
            ViewBag.Title = "Contact";

            return View();
        }


    }
}
