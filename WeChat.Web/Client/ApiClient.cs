using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using WeChat.Diagnostics.Log;

namespace WeChat.Api.Client
{
    public partial class ApiClient
    {
        protected HttpClient HttpClient = new HttpClient();

        // Initialize HttpClient
        protected ApiClient()
        {
            string baseAddress = ConfigurationManager.AppSettings["BaseAddress"];
            HttpClient.BaseAddress = new Uri(baseAddress);
        }

        protected Dictionary<string, List<string>> CallbackIP = null;

        // IP Address List
        public List<string> GetCallbackIP()
        {
            if (CallbackIP == null)
            {
                string accessToken = GetAccessToken();

                string relativeUri = string.Format("/cgi-bin/getcallbackip?access_token={0}", accessToken);

                // {"ip_list":["127.0.0.1","127.0.0.1"]}
                // {"errcode":40013,"errmsg":"invalid appid"}
                string json = ApiGet(relativeUri);
                Dictionary<string, List<string>>  callbackIP = JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
                if (callbackIP.ContainsKey("ip_list"))
                {
                    Log4.Logger.Debug(json);

                    CallbackIP = callbackIP;
                }
                else
                {
                    Log4.Logger.Error(json);
                }
            }
            return CallbackIP["ip_list"];
        }

        protected string ApiGet(string relativeUri)
        {
            HttpResponseMessage response = HttpClient.GetAsync(relativeUri).Result;
            string json = response.Content.ReadAsStringAsync().Result;
            return json;
        }

        protected string ApiPost(string relativeUri, HttpContent content)
        {
            HttpResponseMessage response = HttpClient.PostAsync(relativeUri, content).Result;
            string json = response.Content.ReadAsStringAsync().Result;
            return json;
        }

        private static ApiClient Instance = null;
        public static ApiClient Create()
        {
            if (Instance == null)
            {
                Instance = new ApiClient(); 
            }
            return Instance;
        }


    }
}