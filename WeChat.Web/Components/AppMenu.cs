using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WeChat.Data.Components
{
    public class AppMenu
    {
        private const string MENU_FILE_NAME = "menu.json";

        public static void Save(string json)
        {
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppMenu.MENU_FILE_NAME);
            File.WriteAllText(fileName, json, Encoding.UTF8);
        }

        private static string Read()
        {
            string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppMenu.MENU_FILE_NAME);
            return File.ReadAllText(fileName, Encoding.UTF8);
        }

        private static Dictionary<string, object>[] ToDictionary(string json)
        {
            Dictionary<string, Dictionary<string, object>[]> menu = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>[]>>(json);
            Dictionary<string, object>[] items = menu["button"];
            for (int i = 0; i < items.Length; i++)
            {
                Dictionary<string, object> item = items[i];
                if (item.ContainsKey("sub_button"))
                {
                    string sub_button = item["sub_button"].ToString();
                    if (sub_button.TrimStart('[').TrimEnd(']').Trim() == string.Empty)
                    {
                        item.Remove("sub_button");
                    }
                    else
                    {
                        Dictionary<string, object>[] subButtons = JsonConvert.DeserializeObject<Dictionary<string, object>[]>(sub_button);
                        for (int j = 0; j < subButtons.Length; j++)
                        {
                            Dictionary<string, object> subButton = subButtons[j];
                            if (subButton.ContainsKey("sub_button"))
                            {
                                string sub_sub_button = subButton["sub_button"].ToString();
                                if (sub_sub_button.TrimStart('[').TrimEnd(']').Trim() == string.Empty)
                                {
                                    subButton.Remove("sub_button");
                                }
                            }
                        }
                        item["sub_button"] = subButtons;
                    }
                }
            }
            return items;
        }

        private static Dictionary<string, object>[] _Menu = null;

        public static Dictionary<string, object>[] Menu
        {
            get
            {
                if (_Menu == null)
                {
                    string json = Read();
                    _Menu = ToDictionary(json);
                }
                return _Menu;
            }
        }

        public static Dictionary<string, object> Find(string key)
        {
            Dictionary<string, object> menu = Menu.FirstOrDefault(d => d.ContainsKey("key") && d["key"].ToString() == key);
            if (menu != null) return menu;

            IEnumerable<Dictionary<string, object>[]> sub_menus = Menu.Where(d => d.ContainsKey("sub_button")).Select(d => d["sub_button"] as Dictionary<string, object>[]);
            foreach (Dictionary<string, object>[] sub_menu in sub_menus)
            {
                Dictionary<string, object> item = sub_menu.FirstOrDefault(d => d.ContainsKey("key") && d["key"].ToString() == key);
                if (item != null) return item;
            }
            return null;
        }


    }
}