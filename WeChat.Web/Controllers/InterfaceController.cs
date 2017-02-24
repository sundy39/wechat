using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using WeChat.Http.Models;
using WeChat.Diagnostics.Log;

namespace WeChat.Http.Controllers
{
    [RoutePrefix("")]
    public class InterfaceController : ApiController
    {
        [Route()]
        public HttpResponseMessage Get()
        {
            try
            {
                return new AccessModel().Access(this.Request);
            }
            catch (Exception e)
            {
                Log4.Logger.Error("Access", e);
                throw e;
            }
        }

        [Route()]
        public HttpResponseMessage Post()
        {
            try
            {
                return new ReceivingModel().Handle(this.Request);
            }
            catch(Exception e)
            {
                Log4.Logger.Error("Handle", e);
                throw e;
            }
        }

    }
}
