using System;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using System.IO;

namespace LCW.Framework.Common.Serialization
{
    public class DataContractJsonSerializer<T> : ISerializationFormatter
    {
        private DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
        #region ISerializationFormatter 成员

        public object Deserialize(System.IO.Stream serializationStream)
        {
            return ser.ReadObject(serializationStream);
        }

        public void Serialize(System.IO.Stream serializationStream, object graph)
        {
            ser.WriteObject(serializationStream, graph);
        }

        #endregion
    }
}
