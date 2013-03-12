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
    public class CSharpBuilder:ICodePrivoder
    {
        public CSharpBuilder(string generateCode)
        {
            this.generateCode = generateCode;
            referenceAssemblies = new List<string>();
        }
        private CSharpCodeProvider provider;
        private ICodeCompiler objICodeCompiler;
        private CompilerParameters objCompilerParameters;

        private IList<string> referenceAssemblies;
        public IList<string> ReferencedAssemblies
        {
            get
            {
                return referenceAssemblies;
            }
            set
            {
                referenceAssemblies = value;
            }
        }

        private string generateCode;
        public string GenerateCode
        {
            get
            {
                return generateCode;
            }
            set
            {
                generateCode = value;
            }
        }

        public void init()
        {
            provider = new CSharpCodeProvider();
            objICodeCompiler = provider.CreateCompiler();
            objCompilerParameters = new CompilerParameters();
            objCompilerParameters.ReferencedAssemblies.AddRange(referenceAssemblies.ToArray<string>());
            objCompilerParameters.GenerateExecutable = false;
            objCompilerParameters.GenerateInMemory = true;
        }

        private CompilerResults CompilerStart()
        {
            return objICodeCompiler.CompileAssemblyFromSource(objCompilerParameters, generateCode);
        }

        private void Building(Action<bool, CompilerResults> action)
        {
            CompilerResults result=CompilerStart();
            if (result.Errors.HasErrors)
            {
                if (ErrorHandler != null)
                    ErrorHandler(result.Errors);
                action(false, result);
            }
            action(true, result);
        }

        public bool Debug()
        {
            bool flag = false;
            Building((f,obj) =>
            {
                if (f)
                    flag = true;
            });
            return flag;
        }

        public event CompilerHandler ErrorHandler;

        public object Start(string ClassName)
        {
            object obj=null;
            Building((f,result) =>
            {
                if (f)
                {
                    try
                    {
                        Assembly objAssembly = result.CompiledAssembly;
                        obj = objAssembly.CreateInstance(ClassName);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            });
            return obj;
        }
    }
}
