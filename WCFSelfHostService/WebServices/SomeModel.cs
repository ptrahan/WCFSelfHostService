using System;
using System.Runtime.Serialization;

namespace WCFSelfHostService.WebServices
{
    [Serializable]
    [DataContract(Namespace = "")]
    public class SomeModel
    {
        [DataMember]
        public string AString;
        [DataMember]
        public bool ABool;
        [DataMember]
        public int AnInt;
    }
}
