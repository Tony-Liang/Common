using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Text;

namespace LCW.Framework.Common.Serialization
{
    public class XmlSerializerWrapper<T>:ISerializationFormatter
    {
        private XmlSerializer formatter = new XmlSerializer(typeof(T));

        public object Deserialize(System.IO.Stream serializationStream)
        {
            return formatter.Deserialize(serializationStream);
        }

        public void Serialize(System.IO.Stream serializationStream, object graph)
        {
            formatter.Serialize(serializationStream, graph);
        }

        public XmlSerializer Formatter
        {
            get
            {
                return formatter;
            }
        }
    }
}
