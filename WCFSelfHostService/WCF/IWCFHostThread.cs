using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCFSelfHostService.WCF
{
    public interface IWCFHostThread
    {
        void CloseWebService();
        void OpenWebService(Uri webServiceBaseAddress);
    }
}
