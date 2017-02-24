using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using WeChat.Api.Client;
using WeChat.Data;
using WeChat.Diagnostics.Log;

namespace WeChat.Web.Models
{
    internal class OAuth2Model
    {
        private const string Language = "en";

        private ApiClient ApiClient = ApiClient.Create();

        public string GetUserInfo(string code)
        {
            Dictionary<string, object> accessToken = ApiClient.GetOAuth2AccessToken(code);
            if (accessToken.ContainsKey("access_token"))
            {
                string oAuth2AccessToken = accessToken["access_token"].ToString();
                string openid = accessToken["openid"].ToString();
                string lang = Language;
                return ApiClient.GetUserInfo(oAuth2AccessToken, openid, lang);
            }
            else
            {
                return JsonConvert.SerializeObject(accessToken);
            }
        }


    }
}