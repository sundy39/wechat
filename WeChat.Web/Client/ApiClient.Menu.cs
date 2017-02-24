using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using WeChat.Data.Components;
using WeChat.Diagnostics.Log;

namespace WeChat.Api.Client
{
    // ApiClient.Menu.cs    
    public partial class ApiClient
    {
        // Custom-defined Menu

        // https://api.weixin.qq.com/cgi-bin/menu/create?access_token=ACCESS_TOKEN
        public string CreateMenu(string menu)
        {
            string accessToken = GetAccessToken();

            string relativeUri = string.Format("/cgi-bin/menu/create?access_token={0}", accessToken);
            HttpContent content = new StringContent(menu, Encoding.UTF8, "application/json");

            // {"errcode":0,"errmsg":"ok"}
            // {"errcode":40018,"errmsg":"invalid button name size"}
            string json = ApiPost(relativeUri, content);
            Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            if (result.ContainsKey("errcode"))
            {
                if (result["errcode"].ToString() == "0")
                {
                    Log4.Logger.Debug(json);

                    AppMenu.Save(menu);
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
            return json;
        }

        public string QueryMenu()
        {
            string accessToken = GetAccessToken();

            string relativeUri = string.Format("/cgi-bin/menu/get?access_token={0}", accessToken);

            // {"menu":{"button":[{"type":"click","name":"Daily Song","key":"V1001_TODAY_MUSIC","sub_button":[]},{"type":"click","name":" Artist Profile ","key":"V1001_TODAY_SINGER","sub_button":[]},{"name":"Menu","sub_button":[{"type":"view","name":"Search","url":"http://www.soso.com/","sub_button":[]},{"type":"view","name":"Video","url":"http://v.qq.com/","sub_button":[]},{"type":"click","name":"Like us","key":"V1001_GOOD","sub_button":[]}]}]}}
            string json = ApiGet(relativeUri);
            Dictionary<string, object> result = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            if (result.ContainsKey("menu"))
            {
                Log4.Logger.Debug(json);

                string menu = result["menu"].ToString();
                AppMenu.Save(menu);
                return menu;
            }
            else
            {
                Log4.Logger.Error(json);
                return json;
            }            
        }

        public string DeleteMenu()
        {
            string accessToken = GetAccessToken();

            string relativeUri = string.Format("/cgi-bin/menu/delete?access_token={0}", accessToken);

            // { "errcode":0,"errmsg":"ok"}
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
            return json;
        }


    }
}