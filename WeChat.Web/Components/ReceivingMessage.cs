using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using WeChat.Diagnostics.Log;

namespace WeChat.Data.Components
{
    public class ReceivingMessage
    {
        private static readonly DateTime DateBase = new DateTime(1970, 1, 1);

        public static Dictionary<string, object> ToDictionary(XElement received)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            dict.Add("ToUserName", received.Element("ToUserName").Value);
            dict.Add("FromUserName", received.Element("FromUserName").Value);

            int iCreateTime = int.Parse(received.Element("CreateTime").Value);
            DateTime createTime = DateBase.AddSeconds(iCreateTime);
            dict.Add("CreateTime", createTime);

            string msgType = received.Element("MsgType").Value;
            dict.Add("MsgType", msgType);
            if (msgType == "event")
            {
                EventFillDictionary(received, dict);
                return dict;
            }

            // Common Messages
            // To avoid duplicate retry messages, it is recommended to use msgid.           
            long msgId = long.Parse(received.Element("MsgId").Value);
            dict.Add("MsgId", msgId);
            switch (msgType)
            {
                case "text":
                    dict.Add("Content", received.Element("Content").Value);
                    break;
                case "image":
                    dict.Add("PicUrl", received.Element("PicUrl").Value);
                    dict.Add("MediaId", received.Element("MediaId").Value);
                    break;
                case "voice":
                    dict.Add("MediaId", received.Element("MediaId").Value);
                    dict.Add("Format", received.Element("Format").Value);
                    break;
                case "video":
                    dict.Add("MediaId", received.Element("MediaId").Value);
                    dict.Add("ThumbMediaId", received.Element("ThumbMediaId").Value);
                    break;
                case "shortvideo":
                    dict.Add("MediaId", received.Element("MediaId").Value);
                    dict.Add("ThumbMediaId", received.Element("ThumbMediaId").Value);
                    break;
                case "location":
                    dict.Add("Location_X", double.Parse(received.Element("Location_X").Value));
                    dict.Add("Location_Y", double.Parse(received.Element("Location_Y").Value));
                    dict.Add("Scale", int.Parse(received.Element("Scale").Value));
                    dict.Add("Label", received.Element("Label").Value);
                    break;
                case "link":
                    dict.Add("Title", received.Element("Title").Value);
                    dict.Add("Description", received.Element("Description").Value);
                    dict.Add("Url", received.Element("Url").Value);
                    break;
            }
            return dict;
        }

        //Event-based Messages
        // To avoid duplicate retry messages, it is recommended to use FromUserName + CreateTime.
        private static void EventFillDictionary(XElement received, Dictionary<string, object> dict)
        {
            string eventType = received.Element("Event").Value;
            dict.Add("Event", eventType);
            Log4.Logger.Debug(eventType);
            switch (eventType)
            {
                case "subscribe":
                    //if (received.Element("EventKey") != null)
                    //{
                    //    // Starts with "qrscene_"
                    //    dict.Add("EventKey", received.Element("EventKey").Value);
                    //    dict.Add("Ticket", received.Element("Ticket").Value);
                    //}
                    break;
                case "unsubscribe":
                    break;
                case "SCAN":
                    ////dict.Add("EventKey", uint.Parse(received.Element("EventKey").Value))
                    //dict.Add("EventKey", long.Parse(received.Element("EventKey").Value));
                    //dict.Add("Ticket", received.Element("Ticket").Value);
                    break;
                case "LOCATION":
                    dict.Add("Latitude", double.Parse(received.Element("Latitude").Value));
                    dict.Add("Longitude", double.Parse(received.Element("Longitude").Value));
                    dict.Add("Precision", double.Parse(received.Element("Precision").Value));
                    break;
                case "CLICK":
                    dict.Add("EventKey", received.Element("EventKey").Value);
                    break;
                case "VIEW":
                    dict.Add("EventKey", received.Element("EventKey").Value);
                    break;
            }
        }


    }
}