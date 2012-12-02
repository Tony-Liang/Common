using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace LCW.Framework.Common.Util
{
    public class NewtonsoftJsonHelper
    {
        public static JsonConverter IsoDateTime(string format="yyyy/MM/dd HH:mm:ss")
        {
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            timeConverter.DateTimeFormat = format;
            return timeConverter;
        }
    }
}
