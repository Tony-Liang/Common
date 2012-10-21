using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LCW.Framework.Common.Util
{
    public class TimeSpanParseRuleAttribute:Attribute
    {
        public TimeSpanParseRule Rule { get; set; }
    }

    public enum TimeSpanParseRule
    {
        Milliseconds,
        Seconds,
        Minutes,
        Hours
    }
}
