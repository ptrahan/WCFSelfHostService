using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Description;
using System.Threading;
using WCFSelfHostService.Logger;
using WCFSelfHostService.WebServices;

namespace WCFSelfHostService.WCF
{
    // T represents the class type of the service to start
    public class WCFHostThread<T> : IWCFHostThread
    {
        private ServiceHost m_host;
        private bool m_started = false;
        private ManualResetEvent m_openDone;
        private Exception m_exceptionThrown = null;
        
        public WCFHostThread()
        {
        }

        public void OpenWebService(Uri webServiceBaseAddress)
        {
            lock (this)
            {
                if (m_started)
                {
                    Log.MonitoringLogger.Info("The web service host is already running. Close it before calling OpenWebService().");
                }
            }

            m_openDone = new ManualResetEvent(false);

            m_exceptionThrown = null;

            Thread processingThread = new Thread(new ParameterizedThreadStart(OpenWebServiceThread));
            processingThread.Start(webServiceBaseAddress);

            // wait for thread to complete
            m_openDone.WaitOne();

            if (m_exceptionThrown != null)
            {                
                throw m_exceptionThrown;
            }
            else
            {
                Log.MonitoringLogger.Info("The web service is ready at " + webServiceBaseAddress);
            }
        }

        private void OpenWebServiceThread(object obj)
        {
            try
            {
                Uri webServiceBaseAddress = obj as Uri;

                lock (this)
                {
                    if (webServiceBaseAddress == null || m_started)
                    {                        
                        return;
                    }

                    // Create the ServiceHost.
                    m_host = new ServiceHost(typeof(T), webServiceBaseAddress);

                    // Enable metadata publishing.
                    //ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                    //smb.HttpGetEnabled = true;
                    //smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;

                    //host.Description.Behaviors.Add(smb);

                    // Open the ServiceHost to start listening for messages. Since
                    // no endpoints are explicitly configured, the runtime will create
                    // one endpoint per base address for each service contract implemented
                    // by the service.

                    //host.AddServiceEndpoint(ServiceMetadataBehavior.MexContractName, MetadataExchangeBindings.CreateMexHttpBinding(), "mex");

                    //http
                    //host.AddServiceEndpoint(typeof(ICTIManagerWebService), new WSHttpBinding(), webServiceBaseAddress.ToString());

                    m_host.Open();

                    m_started = true;
                }
            }
            catch (Exception ex)
            {
                this.m_exceptionThrown = new Exception(ex.Message);
            }
            finally
            {
                this.m_openDone.Set();
            }
        }

        public void CloseWebService()
        {
            try
            {
                lock (this)
                {
                    if (m_host != null)
                    {
                        // Close the ServiceHost.
                        try
                        {
                            m_host.Close();
                        }
                        catch (Exception) { }
                    }

                    m_started = false;
                }
            }
            catch (Exception)
            {
            }
        }
    }
}
