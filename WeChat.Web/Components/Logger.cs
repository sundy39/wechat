using log4net;
using log4net.Config;
using System;
using System.IO;

namespace WeChat.Diagnostics.Log
{
    public static class Log4
    {
        private static ILog logger = null;

        public static ILog Logger
        {
            get
            {
                if (logger == null)
                {
                    string fileName = "log4net.config";
                    if (!File.Exists(fileName))
                    {
                        fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
                    }
                    FileInfo file = new FileInfo(fileName);
                    XmlConfigurator.ConfigureAndWatch(file);
                    logger = LogManager.GetLogger("logger");
                }
                return logger;
            }
        }
    }
}