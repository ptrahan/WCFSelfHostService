using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using WCFSelfHostService.Logger;

namespace WCFSelfHostService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Log.MonitoringLogger.Info("WCFSelfHostService Main");
#if DEBUG
            
            MainService mainService = new MainService(args);
            mainService.OnDebug();
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
#else
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new MainService(args)
            };
            ServiceBase.Run(ServicesToRun);
#endif
        }
    }
}
