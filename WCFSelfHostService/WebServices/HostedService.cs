using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using WCFSelfHostService.Logger;

namespace WCFSelfHostService.WebServices
{
    [ServiceBehavior(Namespace = "http://www.example.com/",
    ConcurrencyMode = ConcurrencyMode.Multiple,
    InstanceContextMode = InstanceContextMode.PerCall)]
    public class HostedService : IHostedService
    {
        public bool GetSomeObject(SomeModel m)
        {
            Log.MonitoringLogger.Info(m.AString + ", " + m.AnInt);
            return m.ABool;
        }

        public string GetSomeString(int id)
        {
            return Convert.ToString(id);
        }
    }
}
