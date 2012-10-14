using System;

namespace LCW.Framework.Common.Serialization
{
  public static class SerializationFormatterFactory
  {
    public static ISerializationFormatter GetFormatter<T>(SerializationType type)
    {
        ISerializationFormatter obj=null;
        switch (type)
        {
            case SerializationType.BinaryFormatter: 
                obj = new BinaryFormatterWrapper(); 
                break;
            case SerializationType.XmlSerializer:
                obj =new XmlSerializerWrapper<T>();
                break;
            case SerializationType.NetDataContractSerializer:
                obj =new NetDataContractSerializerWrapper();
                break;
            case SerializationType.Json:
                obj = new DataContractJsonSerializer<T>();
                break;
            default:
                obj = new BinaryFormatterWrapper(); 
                break;
        }
        return obj;
    }
  }

  public enum SerializationType
  {
      BinaryFormatter,
      XmlSerializer,
      NetDataContractSerializer,
      Json
  }
}