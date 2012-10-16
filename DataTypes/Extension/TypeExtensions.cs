using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.DataTypes.Extension
{
    public static class TypeExtensions
    {
        public static void ThrowIfNull(this object Item, string Name)
        {
            if (Item.IsNull())
                throw new ArgumentNullException(Name);
        }

        public static bool IsNull(this object Object)
        {
            return Object == null;
        }
    }
}
