using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using WeChat.Data;
using WeChat.Diagnostics.Log;

namespace WeChat.Api.Client
{
    // ApiClient.AccessToken.cs
    public partial class ApiClient
    {
        // Access_Token

        protected const string ACCESS_TOKEN_FILE_NAME = "access_token";
        protected Dictionary<string, object> AccessToken = null;
        protected System.Threading.Timer Timer;

        public string GetAccessToken()
        {
            if (AccessToken == null)
            {
                Dictionary<string, object> accessToken = Read();
                if (accessToken == null)
                {
                    RefreshAccessToken();
                }
                else
                {
                    AccessToken = accessToken;
                    Elapsed(false);
                }
                int expires_in = int.Parse(AccessToken["expires_in"].ToString());

                int interval = expires_in / 4 * 1000;
                Timer = new Timer(Elapsed, true, interval, interval);
            }
            return AccessToken["access_token"].ToString();
        }

        protected void Elapsed(object state)
        {
            int expires_in = int.Parse(AccessToken["expires_in"].ToString());
            DateTime access_token_start = (DateTime)AccessToken["access_token_start"];
            if ((DateTime.Now - access_token_start).TotalSeconds > expires_in / 2)
            {
                if ((bool)state)
                {
                    lock (AccessToken)
                    {
                        RefreshAccessToken();
                    }
                }
                else
                {
                    RefreshAccessToken();
                }
            }
        }

        protected void RefreshAccessToken()
        {
            string relativeUri = "/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
            relativeUri = string.Format(relativeUri, DevConfig.AppID, DevConfig.AppSecret);

            // {"access_token":"ACCESS_TOKEN","expires_in":7200}
            // {"errcode":40013,"errmsg":"invalid appid"}
            string json = ApiGet(relativeUri);
            Dictionary<string, object> accessToken = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            if (accessToken.ContainsKey("access_token"))
            {
                Log4.Logger.Debug(json);

                accessToken.Add("access_token_start", DateTime.Now);
                Save(accessToken);
                AccessToken = accessToken;
            }
            else
            {
                Log4.Logger.Error(json);
            }
        }

        protected void Save(Dictionary<string, object> accessToken)
        {
            string json = JsonConvert.SerializeObject(accessToken);

            //
            string encrypted = Tencent.Cryptography.AES_encrypt(json, DevConfig.EncodingAESKey, DevConfig.AppID);

            //
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ACCESS_TOKEN_FILE_NAME);
            File.WriteAllText(fileName, encrypted, Encoding.UTF8);
        }

        protected Dictionary<string, object> Read()
        {
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ACCESS_TOKEN_FILE_NAME);
            if (!File.Exists(fileName)) return null;

            //          
            string encrypted = File.ReadAllText(fileName, Encoding.UTF8); ;
            string appid = DevConfig.AppID;
            string json = Tencent.Cryptography.AES_decrypt(encrypted, DevConfig.EncodingAESKey, ref appid);
            if (appid != DevConfig.AppID) return null;

            //
            Dictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            return dict;
        }


    }
}