using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.CodeDom.Compiler;

namespace LCW.Framework.Common.CodeCompiler
{
    public interface ICodePrivoder
    {
        event CompilerHandler ErrorHandler;

        IList<string> ReferencedAssemblies { get; }

        string GenerateCode { get; }

        void init();

        bool Debug();

        Object Start(string ClassName);
    }

    public delegate void CompilerHandler(CompilerErrorCollection e);
}
