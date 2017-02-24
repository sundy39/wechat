using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WeChat.Data;
using WeChat.Web.Models;

namespace WeChat.Web.Controllers
{
    [RoutePrefix("OAuth2")]
    [Route("{action=Index}")]
    public class OAuth2Controller : Controller
    {
        private static readonly string RedirectUri = "http://" + DevConfig.DomainName + "/OAuth2";

        [Route("Authorize")]
        // https://open.weixin.qq.com/connect/oauth2/authorize?appid=APPID&redirect_uri=REDIRECT_URI&response_type=code&scope=SCOPE&state=STATE#wechat_redirect
        public ActionResult Authorize()
        {
            string redirect_uri = Server.UrlEncode(RedirectUri);

            // scope: snsapi_base or snsapi_userinfo
            string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope={2}&state={3}#wechat_redirect";
            url = string.Format(url, DevConfig.AppID, redirect_uri, "snsapi_userinfo", "state");
            return Redirect(url);
        }

        public ActionResult Index()
        {
            string code = this.Request.QueryString["code"];
            if (string.IsNullOrWhiteSpace(code))
            {
                ViewBag.Content = "The user not gives authorization permission";
            }
            else
            {
                ViewBag.Content = new OAuth2Model().GetUserInfo(code);
            }
            return View();
        }


    }
}