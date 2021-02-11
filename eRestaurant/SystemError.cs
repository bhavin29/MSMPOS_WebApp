using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NLog;

namespace RocketPOS
{
    public static class SystemLogs
    {
        static Logger logger;
        public static void Register(string log)
        {
            logger = LogManager.GetCurrentClassLogger();
            logger.Error(log.ToString());
        }
    }
}
