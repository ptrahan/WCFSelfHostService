using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;


namespace WCFSelfHostService
{
    public partial class MainService : ServiceBase
    {
        
        public MainService(string[] args)
        {
            InitializeComponent();
            string eventSourceName = "WCFSelfHostService";
            string logName = "WCFSelfHostServiceLog";
            if (args.Count() > 0)
            {
                eventSourceName = args[0];
            }
            if (args.Count() > 1)
            {
                logName = args[1];
            }
            EventLog eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(eventSourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(eventSourceName, logName);
            }
            eventLog1.Source = eventSourceName;
            eventLog1.Log = logName;


        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected async override void OnStart(string[] args)
        {
            

            
        }

        protected async override void OnStop()
        {
          
        }
    }
}
