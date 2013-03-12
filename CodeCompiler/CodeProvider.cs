using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.CodeCompiler
{
    public class CodeProviderFactory
    {
        public static ICodePrivoder GetInstance(string code)
        {
            return new CSharpBuilder(code);
        }
    }
}
