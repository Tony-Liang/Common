using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Collections.Specialized;
using System.Reflection;

namespace LCW.Framework.Common.CodeCompiler
{
    internal class CSharpBuilder : ICodePrivoder
    {
        #region ICodePrivoder 成员
        public CSharpBuilder()
        {

        }

        public event CompilerHandler ErrorHandler;
        private CompilerResults result;

        public bool Build(string generateCode)
        {
            return Build(generateCode, null);
        }

        public bool Build(string generateCode, params string[] referenceAssemblies)
        {
            return init(generateCode, referenceAssemblies);
        }

        public Assembly Compile(string generateCode)
        {
            return Compile(generateCode, null);
        }

        public Assembly Compile(string generateCode, params string[] referenceAssemblies)
        {
            Assembly assmbly = null;
            if (init(generateCode, referenceAssemblies))
            {
                assmbly = result.CompiledAssembly;
            }
            return assmbly;
        }

        public object CreateInstance(string typeName, string generateCode, params string[] referenceAssemblies)
        {
            object temp = null;
            Assembly assmbly = Compile(generateCode, referenceAssemblies);
            if (assmbly != null)
            {
                temp = assmbly.CreateInstance(typeName);
            }
            return temp;
        }

        private bool init(string generateCode, params string[] referenceAssemblies)
        {
            bool flag = false;
            result = null;
            using (CSharpCodeProvider provider = new CSharpCodeProvider())
            {
                ICodeCompiler objICodeCompiler = provider.CreateCompiler();
                CompilerParameters objCompilerParameters = new CompilerParameters();
                if (referenceAssemblies != null)
                    objCompilerParameters.ReferencedAssemblies.AddRange(referenceAssemblies);
                objCompilerParameters.GenerateExecutable = false;
                objCompilerParameters.GenerateInMemory = true;
                result = objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters, generateCode);
            }
            if (result != null)
            {
                if (result.Errors.Count > 0 && ErrorHandler != null)
                {
                    ErrorHandler(result.Errors);
                }
                else
                {
                    flag = true;
                }
            }
            return flag;
        }
        #endregion
    }
}
