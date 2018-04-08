using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

[assembly: XmlConfigurator(ConfigFile = "Log4Net.config", Watch = true)]
namespace WCFSelfHostService.Logger
{
    public static class Log
    {
        public static ILog MonitoringLogger
        {
            get { return LogManager.GetLogger("MonitoringLogger"); }
        }

        public static ILog ExceptionLogger
        {
            get { return LogManager.GetLogger("ExceptionLogger"); }
        }
    }
}
