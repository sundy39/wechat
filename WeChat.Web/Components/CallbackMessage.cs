using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace WeChat.Data.Components
{
    public class CallbackMessage // Sending Messages
    {
        private static readonly DateTime DateBase = new DateTime(1970, 1, 1);

        public static XElement CreateTextMessage(string content, Dictionary<string, object> received)
        {
            XElement message = CreateMessage("text", received);
            XElement node = new XElement("Content", new XCData(content));
            message.Add(node);
            return message;
        }

        public static XElement CreateImageMessage(string media_id, Dictionary<string, object> received)
        {
            XElement message = CreateMessage("image", received);
            XElement node = new XElement("Image");
            message.Add(node);
            node.Add(new XElement("MediaId", new XCData(media_id)));
            return message;
        }

        public static XElement CreateVoiceMessage(string media_id, Dictionary<string, object> received)
        {
            XElement message = CreateMessage("voice", received);
            XElement node = new XElement("Voice");
            message.Add(node);
            node.Add(new XElement("MediaId", new XCData(media_id)));
            return message;
        }

        public static XElement CreateVideoMessage(string media_id, string title, string description, Dictionary<string, object> received)
        {
            XElement message = CreateMessage("video", received);
            XElement node = new XElement("Video");
            message.Add(node);
            node.Add(new XElement("MediaId", new XCData(media_id)),
                new XElement("Title", new XCData(title)), new XElement("Description", new XCData(description)));
            return message;
        }

        public static XElement CreateMusicMessage(string title, string description, string musicUrl, string hqMusicUrl, string thumbMediaId, Dictionary<string, object> received)
        {
            XElement message = CreateMessage("music", received);
            XElement node = new XElement("Music");
            message.Add(node);
            node.Add(new XElement("Title", new XCData(title)), new XElement("Description", new XCData(description)),
                new XElement("MusicUrl", new XCData(musicUrl)), new XElement("HQMusicUrl", new XCData(hqMusicUrl)),
                new XElement("ThumbMediaId", new XCData(thumbMediaId)));
            return message;
        }

        public static XElement CreateNewsMessage(IEnumerable<Dictionary<string, object>> items, Dictionary<string, object> received)
        {
            XElement message = CreateMessage("news", received);
            message.SetElementValue("ArticleCount", items.Count());
            XElement node = new XElement("Articles");
            message.Add(node);
            foreach (Dictionary<string, object> item in items)
            {
                XElement itemNode = new XElement("item");
                itemNode.Add(new XElement("Title", new XCData(item["Title"].ToString())),
                    new XElement("Description", new XCData(item["Description"].ToString())),
                    new XElement("PicUrl", new XCData(item["PicUrl"].ToString())),
                    new XElement("Url", new XCData(item["Url"].ToString())));
                node.Add(itemNode);
            }
            return message;
        }

        private static XElement CreateMessage(string msgType, Dictionary<string, object> received)
        {
            XElement message = new XElement("xml");
            XElement node = new XElement("ToUserName", new XCData(received["FromUserName"].ToString()));
            message.Add(node);
            node = new XElement("FromUserName", new XCData(received["ToUserName"].ToString()));
            message.Add(node);
            message.SetElementValue("CreateTime", (int)((DateTime.Now - DateBase).TotalSeconds));
            node = new XElement("MsgType", new XCData(msgType));
            message.Add(node);
            return message;
        }


    }
}