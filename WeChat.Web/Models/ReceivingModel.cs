using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using WeChat.Data;
using WeChat.Data.Components;
using WeChat.Data.Extensions;
using WeChat.Diagnostics.Log;

namespace WeChat.Http.Models
{
    internal class ReceivingModel
    {
        private Tencent.WXBizMsgCrypt _bizMsgCrypt = null;
        private Tencent.WXBizMsgCrypt BizMsgCrypt
        {
            get
            {
                if (_bizMsgCrypt == null)
                {
                    _bizMsgCrypt = new Tencent.WXBizMsgCrypt(DevConfig.Token, DevConfig.EncodingAESKey, DevConfig.AppID);
                }
                return _bizMsgCrypt;
            }
        }

        public HttpResponseMessage Handle(HttpRequestMessage request)
        {
            Byte[] buffer;
            using (Stream stream = request.Content.ReadAsStreamAsync().Result)
            {
                stream.Position = 0;
                buffer = new Byte[stream.Length];
                stream.Read(buffer, 0, (Int32)stream.Length);
            }
            string receivingMessage = Encoding.UTF8.GetString(buffer);

            //
            string sendingMessage;
            IEnumerable<KeyValuePair<string, string>> queryStr = request.GetQueryNameValuePairs();
            string encrypt_type = queryStr.GetValue("encrypt_type");
            Log4.Logger.Debug("encrypt_type:" + encrypt_type);
            if (encrypt_type == "aes")
            {
                string msg_signature = queryStr.GetValue("msg_signature");
                string timestamp = queryStr.GetValue("timestamp");
                string nonce = queryStr.GetValue("nonce");

                string receivedXml = string.Empty;
                int error = BizMsgCrypt.DecryptMsg(msg_signature, timestamp, nonce, receivingMessage, ref receivedXml);
                if (error != 0)
                {
                    Log4.Logger.ErrorFormat("BizMsgCrypt.DecryptMsg:{0}, Error:{1}", receivingMessage, error);
                    return new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
                }
                Log4.Logger.Debug("Receiving:" + receivedXml);

                string replyXml = new ReceivingHandler().Handle(receivedXml);
                Log4.Logger.Debug("Reply:" + replyXml);

                string encryptedMessage = string.Empty;
                error = BizMsgCrypt.EncryptMsg(replyXml, timestamp, nonce, ref encryptedMessage);
                if (error != 0)
                {
                    Log4.Logger.ErrorFormat("BizMsgCrypt.EncryptMsg:{0}, Error:{1}", replyXml, error);
                    return new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError);
                }

                sendingMessage = encryptedMessage;
            }
            else // encrypt_type == "raw"
            {
                Log4.Logger.Debug("Receiving:" + receivingMessage);
                string replyXml = new ReceivingHandler().Handle(receivingMessage);
                Log4.Logger.Debug("Reply:" + replyXml);

                sendingMessage = replyXml;
            }
            Log4.Logger.Debug("SendingMessage:" + sendingMessage);

            //
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new StringContent(sendingMessage, Encoding.UTF8);
            return response;
        }


    }
}