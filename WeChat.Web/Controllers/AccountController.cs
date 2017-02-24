using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using WeChat.Data;

namespace WeChat.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(FormCollection collection, string returnUrl)
        {
            string password = collection["Password"];
            if (string.IsNullOrWhiteSpace(password))
            {
                ViewBag.ErrorMessage = "The password is incorrect";
            }

            if (password == Config.Password)
            {
                FormsAuthentication.SetAuthCookie(Config.UserName, false);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = "The password is incorrect";
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}