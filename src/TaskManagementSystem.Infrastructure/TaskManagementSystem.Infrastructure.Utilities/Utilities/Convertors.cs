using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace TaskManagementSystem.Infrastructure.Utilities.Utilities
{
    public static class Convertors
    {
        public static Stream ConvertObjectToCSV(dynamic inputObject)
        {
            StringBuilder buildCSV = new StringBuilder();
            XmlSerializer xmlSerializer = new XmlSerializer(inputObject.GetType());
            
            using var strWriter = new StringWriter();
            
            using XmlWriter writer = XmlWriter.Create(strWriter);
            
            xmlSerializer.Serialize(writer, inputObject);
            
            string xmlData = strWriter.ToString();

            XDocument xDoc = XDocument.Parse(xmlData);
            IEnumerable<XElement> elements = xDoc.Root.Elements();

            //Create column names
            foreach(XElement xElement in elements)
            {
                foreach(XElement subElement in xElement.Elements())
                {
                    if(buildCSV.Length > 0)
                        buildCSV.Append("," + subElement.Name);
                    else
                        buildCSV.Append(subElement.Name);
                }
                break;
            }
            buildCSV.Append("\r\n");
            //Append value for each column
            foreach (XElement element in elements)
            {
                foreach(XElement subElement in element.Elements())
                {
                    buildCSV.Append(subElement.Value.Trim() + ",");
                }
                buildCSV.ToString().Remove(buildCSV.ToString().Length - 1);
                buildCSV.Append("\r\n");
            }

            //Convert string to stream
            byte[] streamByte =  Encoding.ASCII.GetBytes(buildCSV.ToString());
            
            MemoryStream stream = new MemoryStream(streamByte);

            return stream;
        }
    }
}
    