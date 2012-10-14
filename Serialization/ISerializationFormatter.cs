using System;

namespace LCW.Framework.Common.Serialization
{
  public interface ISerializationFormatter
  {
    object Deserialize(System.IO.Stream serializationStream);

    void Serialize(System.IO.Stream serializationStream, object graph);
  }
}