using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.CodeCompiler
{
    public abstract class RazorTemplateBase : MarshalByRefObject
    {       
        public virtual void Execute() { }

        public virtual void Write(object value) { }

        public virtual void WriteLiteral(object value) { }
    }

    public class RazorTemplateBase<T> : RazorTemplateBase
    {       
        public T Model { get; set; }
    }
}
