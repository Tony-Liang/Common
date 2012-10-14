using System;
using System.Runtime.Serialization.Formatters.Binary;

namespace LCW.Framework.Common.Serialization
{
  public class BinaryFormatterWrapper : ISerializationFormatter
  {
    private BinaryFormatter formatter = new BinaryFormatter();

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

    public BinaryFormatter Formatter
    {
      get
      {
        return formatter;
      }
    }
  }
}