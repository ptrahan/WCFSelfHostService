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
        

        public MainService()
        {
            InitializeComponent();

            
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
