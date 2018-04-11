using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace VTCTIManagerService.WebAdmin
{
    public class XmlObjectSerializer
    {
        static public XDocument SerializeObject<T>(T obj)
        {
            try
            {
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
            System.IO.MemoryStream ms = new System.IO.MemoryStream();

            serializer.Serialize(ms, obj);
            ms.Position = 0;
            XDocument xmlDoc = XDocument.Load(ms);
            return xmlDoc;   
       
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        static public XDocument SerializeObjectList<T>(List<T> obj)
        {
            try
            {
                System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                System.IO.MemoryStream ms = new System.IO.MemoryStream();

                foreach (T o in obj)
                {
                    serializer.Serialize(ms, o);
                }
                XDocument xmlDoc;
                ms.Position = 0;

                if (obj.Count() > 0)
                    xmlDoc = XDocument.Load(ms);
                else
                    xmlDoc = new XDocument();

                return xmlDoc;
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
