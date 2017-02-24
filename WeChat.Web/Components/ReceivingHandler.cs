using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using WeChat.Diagnostics.Log;

namespace WeChat.Data.Components
{
    public class ReceivingHandler // Business Logic
    {
        public string Handle(string received)
        {
            return Handle(XElement.Parse(received)).ToString();
        }

        // for example
        private XElement Handle(XElement received)
        {
            Dictionary<string, object> dict = ReceivingMessage.ToDictionary(received);
            string msgType = dict["MsgType"].ToString();
            switch (msgType)
            {
                case "text":
                    return CallbackMessage.CreateTextMessage("Echo:" + dict["Content"].ToString(), dict);
                case "image":
                    return CallbackMessage.CreateImageMessage(dict["MediaId"].ToString(), dict);
                case "voice":
                    return CallbackMessage.CreateVoiceMessage(dict["MediaId"].ToString(), dict);
                case "video":
                case "shortvideo":
                    return CallbackMessage.CreateVideoMessage(dict["MediaId"].ToString(), "Video", "Echo", dict);
                case "location":
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("Echo:");
                        sb.AppendLine(dict["Label"].ToString());
                        sb.AppendLine("Location:" + dict["Location_X"].ToString() + "," + dict["Location_Y"].ToString());
                        sb.AppendLine("Scale:" + dict["Scale"].ToString());
                        return CallbackMessage.CreateTextMessage(sb.ToString(), dict);
                    }
                case "link":
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("Echo:");
                        sb.AppendLine("MsgType:link");
                        sb.AppendLine("Title:" + dict["Title"].ToString());
                        sb.AppendLine("Description:" + dict["Description"].ToString());
                        sb.AppendLine("Url:" + dict["Url"].ToString());
                        return CallbackMessage.CreateTextMessage(sb.ToString(), dict);
                    }
                case "event":
                    {
                        string eventType = dict["Event"].ToString();
                        Log4.Logger.Debug("event:" + eventType);
                        switch (eventType)
                        {
                            case "subscribe":
                                if (dict.ContainsKey("EventKey"))
                                {
                                    string eventKey = dict["EventKey"].ToString();
                                    if (eventKey.StartsWith("qrscene_"))
                                    {
                                        string ticket = dict["Ticket"].ToString();
                                    }
                                }
                                return CallbackMessage.CreateTextMessage("Welcome", dict);
                            case "unsubscribe":
                                // send successful,but the client never received
                                return CallbackMessage.CreateTextMessage("Goodbye", dict);
                            case "SCAN":
                                // ? never received
                                {
                                    string eventKey = dict["EventKey"].ToString();
                                    string ticket = dict["Ticket"].ToString();
                                    return CallbackMessage.CreateTextMessage("Welcome back", dict);
                                }
                            case "LOCATION":
                                {
                                    StringBuilder sb = new StringBuilder();
                                    sb.AppendLine("location event:");
                                    sb.AppendLine(dict["Latitude"].ToString());
                                    sb.AppendLine(dict["Longitude"].ToString());
                                    sb.AppendLine(dict["Precision"].ToString());
                                    return CallbackMessage.CreateTextMessage(sb.ToString(), dict);
                                }
                            case "CLICK":
                                {
                                    StringBuilder sb = new StringBuilder();
                                    sb.AppendLine("menu-click event:");
                                    string key = dict["EventKey"].ToString();
                                    sb.AppendLine("key:" + key);
                                    Dictionary<string, object> menu = AppMenu.Find(key);
                                    sb.AppendLine("name:" + menu["name"].ToString());
                                    return CallbackMessage.CreateTextMessage(sb.ToString(), dict);
                                }
                            case "VIEW":
                                {
                                    StringBuilder sb = new StringBuilder();
                                    sb.AppendLine("menu-url-redirection event:");
                                    sb.AppendLine("url:" + dict["EventKey"].ToString());
                                    return CallbackMessage.CreateTextMessage(sb.ToString(), dict);
                                }
                        }
                    }
                    break;
            }

            throw new NotSupportedException(msgType);
        }


    }
}