using System;
using System.Runtime.Serialization;

namespace LCW.Framework.Common.Serialization
{
  public class NetDataContractSerializerWrapper : ISerializationFormatter
  {
    private NetDataContractSerializer formatter =new NetDataContractSerializer();

    #region ISerializationFormatter Members

    public object Deserialize(System.IO.Stream serializationStream)
    {
      return formatter.Deserialize(serializationStream);
    }

    public void Serialize(System.IO.Stream serializationStream, object graph)
    {
      formatter.Serialize(serializationStream, graph);
    }

    #endregion

    public NetDataContractSerializer Formatter
    {
      get
      {
        return formatter;
      }
    }
  }
}