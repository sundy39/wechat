using System;
using System.Collections.Generic;
using System.Linq;
using WeChat.Api.Client;
using WeChat.Data.Components;

namespace WeChat.Http.Models
{
    internal class MenuModel
    {       
        private ApiClient ApiClient = ApiClient.Create();

        public string Query()
        {
            return ApiClient.QueryMenu();
        }

        public string Create(string json)
        {
            return ApiClient.CreateMenu(json.Replace("\r", string.Empty).Replace("\n", string.Empty));
        }
        
        public string Delete()
        {
            return ApiClient.DeleteMenu();
        }


    }
}