using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using WCFSelfHostService.Logger;
using WCFSelfHostService.WCF;
using WCFSelfHostService.WebServices;

namespace WCFSelfHostService
{
    public enum ServiceState
    {
        SERVICE_STOPPED = 0x00000001,
        SERVICE_START_PENDING = 0x00000002,
        SERVICE_STOP_PENDING = 0x00000003,
        SERVICE_RUNNING = 0x00000004,
        SERVICE_CONTINUE_PENDING = 0x00000005,
        SERVICE_PAUSE_PENDING = 0x00000006,
        SERVICE_PAUSED = 0x00000007,
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ServiceStatus
    {
        public long dwServiceType;
        public ServiceState dwCurrentState;
        public long dwControlsAccepted;
        public long dwWin32ExitCode;
        public long dwServiceSpecificExitCode;
        public long dwCheckPoint;
        public long dwWaitHint;
    };

    public partial class MainService : ServiceBase
    {
        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        private List<IWCFHostThread> m_wcfHostList = new List<IWCFHostThread>();

        public MainService(string[] args)
        {
            Log.MonitoringLogger.Info("MainService initializing.");

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
            Log.MonitoringLogger.Info("MainService.OnDebug()");
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            Log.MonitoringLogger.Info("MainService.OnStart()");

            try
            {
                System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);

                // Update the service state to Start Pending.
                ServiceStatus serviceStatus = new ServiceStatus()
                {
                    dwCurrentState = ServiceState.SERVICE_START_PENDING,
                    dwWaitHint = 100000
                };
                SetServiceStatus(this.ServiceHandle, ref serviceStatus);

                //////////////////////////////////////////////////////////////
                // Start code here:
                Log.MonitoringLogger.Info("Service initializing...");
                InitializeService();
                Log.MonitoringLogger.Info("Service is now initialized.");


                /////////////////////////////////////////////////////////////
                // Update the service state to Running.
                serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
                SetServiceStatus(this.ServiceHandle, ref serviceStatus);
            }
            catch (Exception ex)
            {
                Log.ExceptionLogger.Error(ex.StackTrace);                
                throw ex;
            }
        }

        private void InitializeService()
        {
            // init your wcf services
            WCFHostThread<HostedService> wcfHost = new WCFHostThread<HostedService>();
            wcfHost.OpenWebService(new Uri(Properties.Settings.Default.HostedService));

            m_wcfHostList.Add(wcfHost);

            // start the rest interface web server
            //WebApp.Start<Startup>(url: restServiceBaseAddress);
        }        

        protected override void OnStop()
        {
            Log.MonitoringLogger.Info("MainService.OnStop()");

            try
            {
                m_wcfHostList.ForEach(w => w.CloseWebService());
            }
            catch (Exception ex)
            {                
                Log.ExceptionLogger.Error(ex.StackTrace);                
                throw ex;
            }
        }
    }
}
