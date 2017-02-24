using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using WeChat.Data.Security;
using WeChat.Data.Extensions;
using WeChat.Diagnostics.Log;
using System.Text;
using WeChat.Data;

namespace WeChat.Http.Models
{
    internal class AccessModel
    {
        public HttpResponseMessage Access(HttpRequestMessage request)
        {
            IEnumerable<KeyValuePair<string, string>> queryStr = request.GetQueryNameValuePairs();
            string signature = queryStr.GetValue("signature");
            string timestamp = queryStr.GetValue("timestamp");
            string nonce = queryStr.GetValue("nonce");
            string echostr = queryStr.GetValue("echostr");

            Log4.Logger.Debug("signature:" + signature);
            Log4.Logger.Debug("timestamp:" + timestamp);
            Log4.Logger.Debug("nonce:" + nonce);
            Log4.Logger.Debug("echostr:" + echostr);

            string[] array = { DevConfig.Token, timestamp, nonce };
            Array.Sort(array);
            string hashed = string.Join(string.Empty, array);
            hashed = new SHA1Hasher().Hash(hashed);

            Log4.Logger.Debug("Hashed:" + hashed);

            if (hashed == signature)
            {
                Log4.Logger.Debug("Return echostr.");

                HttpResponseMessage response = new HttpResponseMessage();
                response.Content = new StringContent(echostr);
                return response;
            }
            else
            {
                Log4.Logger.Debug("Return HttpStatusCode.Forbidden.");

                HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.Forbidden);
                return response;
            }
        }


    }
}