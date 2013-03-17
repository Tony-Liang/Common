using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;
using System.Reflection;

namespace LCW.Framework.Common.CodeCompiler
{
    public interface ICodePrivoder
    {
        event CompilerHandler ErrorHandler;
        bool Build(string generateCode);
        bool Build(string generateCode, params string[] referenceAssemblies);
        Assembly Compile(string generateCode);
        Assembly Compile(string generateCode, params string[] referenceAssemblies);
        object CreateInstance(string typeName, string generateCode, params string[] referenceAssemblies);
    }

    public delegate void CompilerHandler(CompilerErrorCollection e);
}
