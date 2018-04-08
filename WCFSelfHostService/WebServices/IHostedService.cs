using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;


namespace WCFSelfHostService.WebServices
{
    [ServiceContract(Namespace = "http://www.example.com/")] 
    interface IHostedService
    {
        [OperationContract]
        string GetSomeString(int id);

        [OperationContract]
        bool GetSomeObject(SomeModel m); // Returns an operation ID.
       
    }
}
