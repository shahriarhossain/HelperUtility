using System;
using System.Xml.Serialization;
using System.IO;
using System.Xml;

namespace PaySpan.Sync.Service.Helper
{
    public class XMLGenerator
    {
        public static string GetXMLFromObject(object o)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter tw = null;
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            try
            {
                XmlSerializer serializer = new XmlSerializer(o.GetType());
                
                tw = new XmlTextWriter(sw);
  
                serializer.Serialize(tw, o, ns);
            }
            catch (Exception ex)
            {
                //Handle Exception Code
            }
            finally
            {
                sw.Close();
                if (tw != null)
                {
                    tw.Close();
                }
            }
            return sw.ToString();
        }

    }
}
