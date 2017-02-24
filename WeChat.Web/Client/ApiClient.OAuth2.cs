using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using WeChat.Data;
using WeChat.Diagnostics.Log;
using WeChat.Data.Extensions;

namespace WeChat.Api.Client
{
    // ApiClient.OAuth2.cs
    public partial class ApiClient
    {
        // https://api.weixin.qq.com/sns/oauth2/access_token?appid=APPID&secret=SECRET&code=CODE&grant_type=authorization_code
        public Dictionary<string, object> GetOAuth2AccessToken(string code)
        {
            string relativeUri = string.Format("/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code", DevConfig.AppID, DevConfig.AppSecret, code);

            //{
            //   "access_token":"ACCESS_TOKEN",
            //   "expires_in":7200,
            //   "refresh_token":"REFRESH_TOKEN",
            //   "openid":"OPENID",
            //   "scope":"SCOPE"
            //}
            //{"errcode":40029,"errmsg":"invalid code"}
            string json = ApiGet(relativeUri);
            Dictionary<string, object> accessToken = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            if (accessToken.ContainsKey("access_token"))
            {
                Log4.Logger.Debug(json);

                accessToken.Add("access_token_start", DateTime.Now);
            }
            else
            {
                Log4.Logger.Error(json);
            }
            return accessToken;
        }

        // https://api.weixin.qq.com/sns/oauth2/refresh_token?appid=APPID&grant_type=refresh_token&refresh_token=REFRESH_TOKEN
        public Dictionary<string, object> RefreshOAuth2AccessToken(string refreshToken)
        {
            string relativeUri = string.Format("/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}", DevConfig.AppID, refreshToken);

            //{
            //   "access_token":"ACCESS_TOKEN",
            //   "expires_in":7200,
            //   "refresh_token":"REFRESH_TOKEN",
            //   "openid":"OPENID",
            //   "scope":"SCOPE"
            //}
            //{"errcode":40029,"errmsg":"invalid code"}
            string json = ApiGet(relativeUri);
            Dictionary<string, object> accessToken = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            if (accessToken.ContainsKey("access_token"))
            {
                Log4.Logger.Debug(json);

                accessToken.Add("access_token_start", DateTime.Now);
            }
            else
            {
                Log4.Logger.Error(json);
            }
            return accessToken;
        }

        // https://api.weixin.qq.com/sns/auth?access_token=ACCESS_TOKEN&openid=OPENID
        public Dictionary<string, object> OAuth2AccessTokenIsValid(string oAuth2AccessToken, string openid)
        {
            string relativeUri = string.Format("/sns/auth?access_token={0}&openid={1}", oAuth2AccessToken, openid);

            // { "errcode":0,"errmsg":"ok"}
            // { "errcode":40003,"errmsg":"invalid openid"}
            string json = ApiGet(relativeUri);
            Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            if (result.ContainsKey("errcode"))
            {
                if (result["errcode"].ToString() == "0")
                {
                    Log4.Logger.Debug(json);
                }
                else
                {
                    Log4.Logger.Error(json);
                }
            }
            else
            {
                Log4.Logger.Error(json);
            }
            return result;
        }

        // lang: "zh_CN" or "zh_TW" or "en"
        // https://api.weixin.qq.com/sns/userinfo?access_token=ACCESS_TOKEN&openid=OPENID&lang=LANG
        public string GetUserInfo(string oAuth2AccessToken, string openid, string lang)
        {
            string relativeUri = string.Format("/sns/userinfo?access_token={0}&openid={1}&lang={2}", oAuth2AccessToken, openid, lang);

            //{
            //   "openid":"OPENID",
            //   "nickname":"NICKNAME",
            //   "sex":"1",
            //   "province":"PROVINCE"
            //   "city":"CITY",
            //   "country":"COUNTRY",
            //   "headimgurl":"http://wx.qlogo.cn/mmopen/g3MonUZtNHkdmzicIlibx6iaFqAc56vxLSUfpb6n5WKSYVY0ChQKkiaJSgQ1dZuTOgvLLrhJbERQQ4eMsv84eavHiaiceqxibJxCfHe/46", 
            //   "privilege":[
            //	 "PRIVILEGE1",
            //	 "PRIVILEGE2"],
            //   "unionid":"o6_bmasdasdsad6_2sgVt7hMZOPfL"
            //}
            //{"errcode":40003,"errmsg":" invalid openid "}
            string json = ApiGet(relativeUri);
            Dictionary<string, object> userInfo = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            if (userInfo.ContainsKey("openid"))
            {
                Log4.Logger.Debug(json);

                userInfo.ArrangeUserInfo();
            }
            else
            {
                Log4.Logger.Error(json);
            }
            return json;
        }


    }
}