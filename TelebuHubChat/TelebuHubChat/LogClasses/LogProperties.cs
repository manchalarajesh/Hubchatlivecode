using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Serilog.Core;

namespace TelebuHubChat.LogClasses
{
    public static class LogProperties
    {


        public static Logger infoLogger;
        public static Logger errorLogger;


        public static void info(dynamic Message)
        {
            infoLogger.Information(Message);
        }

        public static void error(dynamic Message)
        {
            errorLogger.Error(Message);
        }

    }
}
