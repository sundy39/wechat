using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WeChat.Api.Client;
using WeChat.Http.Models;

namespace WeChat.Web.Controllers
{
    [Authorize]
    [RoutePrefix("Admin/Menu")]
    public class MenuController : Controller
    {
        [Route("Query")]
        public ActionResult Query()
        {
            ViewBag.Json = new MenuModel().Query();
            return View();
        }

        [Route("Create")]
        public ActionResult Create()
        {
            ViewBag.Result = string.Empty;
            return View();
        }

        [HttpPost]
        [Route("Create")]
        public ActionResult Create(FormCollection collection)
        {
            string json = collection["menu"];     
            ViewBag.Result = new MenuModel().Create(json);
            return View();
        }

        [Route("Delete")]
        public ActionResult Delete()
        {            
            ViewBag.Json = new MenuModel().Query(); 
            ViewBag.Result = string.Empty;
            return View();
        }

        [HttpPost]
        [Route("Delete")]
        public ActionResult Delete(FormCollection collection)
        {
            ViewBag.Result = new MenuModel().Delete();
            return View();
        }


    }
}