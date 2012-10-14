﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using LCW.Framework.Common.Serialization;

namespace LCW.Framework.Common
{
    public static class ObjectCloner
    {
        /// <summary>
        /// Clones an object.
        /// </summary>
        /// <param name="obj">The object to clone.</param>
        /// <remarks>
        /// <para>The object to be cloned must be serializable.</para>
        /// <para>The serialization is performed using the formatter
        /// specified in the application's configuration file
        /// using the CslaSerializationFormatter key.</para>
        /// <para>The default is to use the 
        /// <see cref="System.Runtime.Serialization.Formatters.Binary.BinaryFormatter" />
        /// </para>. You may also choose to use the Microsoft .NET 3.0
        /// <see cref="System.Runtime.Serialization.NetDataContractSerializer">
        /// NetDataContractSerializer</see> provided as part of WCF.
        /// </remarks>
        public static object Clone(object obj)
        {
            using (MemoryStream buffer = new MemoryStream())
            {
                ISerializationFormatter formatter = null;
                  //SerializationFormatterFactory.GetFormatter<typeof(obj)>();
                formatter.Serialize(buffer, obj);
                buffer.Position = 0;
                object temp = formatter.Deserialize(buffer);
                return temp;
            }
        }
    }
}
