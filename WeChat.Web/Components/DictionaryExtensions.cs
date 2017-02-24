using System;
using System.Collections.Generic;
using System.Linq;

namespace WeChat.Data.Extensions
{
    public static class DictionaryExtensions
    {
        public static string GetValue(this IEnumerable<KeyValuePair<string, string>> nameValuePairs, string key)
        {
            string value = null;
            if (nameValuePairs.Any(p => p.Key == key))
            {
                value = nameValuePairs.First(p => p.Key == key).Value;
            }
            return value;
        }

        public static void ArrangeUserInfo(this Dictionary<string, object> userInfo)
        {
            string privilege = userInfo["privilege"].ToString();
            privilege = privilege.Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\"", string.Empty);
            privilege = privilege.Trim().TrimStart('[').TrimEnd(']').Trim();
            string[] ss = privilege.Split(',');
            for (int i = 0; i < ss.Length; i++)
            {
                ss[i] = ss[i].Trim();
            }
            privilege = string.Join(",", ss);
            userInfo["privilege"] = privilege;
        }


    }
}
