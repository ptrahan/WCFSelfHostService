using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;


namespace VTCTIManagerService.WebAdmin
{
    public class JsonObjectSerializer
    {
        static public string SerializeObject<T>(T obj)
        {
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
            
                serializer.WriteObject(ms, obj);
                ms.Position = 0;
                
                return Encoding.UTF8.GetString(ms.ToArray()); 
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        static public string SerializeObjectList<T>(List<T> obj)
        {
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                System.IO.MemoryStream ms = new System.IO.MemoryStream();

                foreach (T o in obj)
                {
                    serializer.WriteObject(ms, o);
                }

                ms.Position = 0;

                if (obj.Count() > 0)
                    return Encoding.UTF8.GetString(ms.ToArray()); 
                else
                    return "";
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
