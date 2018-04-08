using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Description;
using System.Threading;


namespace VTCTIManager.WebService
{
    public class WCFHostThread
    {
        private ServiceHost host;
        private bool started = false;
        private Logger log = Logger.Instance;
        private ManualResetEvent openDone;
        private Exception exceptionThrown = null;

        public WCFHostThread()
        {

        }

        public void OpenWebService(Uri webServiceBaseAddress)
        {
            lock (this)
            {
                if (started)
                {
                    log.Info("The web service host is already running. Close it before calling OpenWebService().");
                    return;
                }
            }

            openDone = new ManualResetEvent(false);

            Thread processingThread = new Thread(new ParameterizedThreadStart(OpenWebServiceThread));
            processingThread.Start(webServiceBaseAddress);

            openDone.WaitOne();

            if (exceptionThrown != null)
            {
                log.Exception(exceptionThrown);
            }
            else
            {
                log.Info("The web service is ready at " + webServiceBaseAddress);
            }
            
        }

        private void OpenWebServiceThread(object obj)
        {
            try
            {
                Uri webServiceBaseAddress = (Uri)obj;

                lock (this)
                {
                    if (started)
                    {
                        //log.Info("The web service host is already running. Close it before calling OpenWebService().");
                        return;
                    }
                    // Create the ServiceHost.
                    host = new ServiceHost(typeof(CTIManagerWebService), webServiceBaseAddress);

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

                    host.Open();

                    started = true;
                }
                
            }
            catch (Exception ex)
            {
                this.exceptionThrown = new Exception(ex.Message);
            }
            finally
            {
                this.openDone.Set();
            }
        }

        public void CloseWebService()
        {
            try
            {
                lock (this)
                {
                    if (host != null)
                    {
                        // Close the ServiceHost.
                        try
                        {
                            host.Close();
                        }
                        catch (Exception) { }
                    }

                    started = false;
                }
            }
            catch (Exception)
            {
               
            }
        }
    }
}
