using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Razor;
using System.Reflection;
using System.IO;
using Microsoft.CSharp;
using System.CodeDom.Compiler;

namespace LCW.Framework.Common.CodeCompiler
{
    public class RazorEngine<T> : MarshalByRefObject
        where T : RazorTemplateBase
    {
        internal RazorEngine()
        {

        }

        public RazorTemplateEngine InitializeRazorEngine(Type baseClassType, string namespaceOfGeneratedClass, string generatedClassName)
        {
            RazorEngineHost host = new RazorEngineHost(new CSharpRazorCodeLanguage());
            host.DefaultBaseClass = baseClassType.FullName;
            host.DefaultClassName = generatedClassName;
            host.DefaultNamespace = namespaceOfGeneratedClass;
            host.NamespaceImports.Add("System");
            host.NamespaceImports.Add("System.Text");
            host.NamespaceImports.Add("System.Collections.Generic");
            host.NamespaceImports.Add("System.Linq");
            host.NamespaceImports.Add("System.IO");
            return new RazorTemplateEngine(host);
        }

        public RazorTemplateEngine InitializeRazorEngine<T>(string namespaceOfGeneratedClass, string generatedClassName)
        {
            return InitializeRazorEngine(typeof(T), namespaceOfGeneratedClass, generatedClassName);
        }

        public Assembly ParseAndCompileTemplate(Type baseClassType,string namespaceOfGeneratedClass, string generatedClassName, string[] ReferencedAssemblies, TextReader sourceCodeReader)
        {
            RazorTemplateEngine engine = InitializeRazorEngine(baseClassType, namespaceOfGeneratedClass, generatedClassName);
            GeneratorResults razorResults = engine.GenerateCode(sourceCodeReader);

            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            CodeGeneratorOptions options = new CodeGeneratorOptions();

            string generatedCode = null;
            using (StringWriter writer = new StringWriter())
            {
                codeProvider.GenerateCodeFromCompileUnit(razorResults.GeneratedCode, writer, options);
                generatedCode = writer.GetStringBuilder().ToString();
            }

            var outputAssemblyName = Path.GetTempPath() + Guid.NewGuid().ToString("N") + ".dll";
            CompilerParameters compilerParameters = new CompilerParameters(ReferencedAssemblies, outputAssemblyName);
            compilerParameters.ReferencedAssemblies.Add("System.dll");
            compilerParameters.ReferencedAssemblies.Add("System.Core.dll");
            compilerParameters.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().CodeBase.Substring(8));
            compilerParameters.GenerateInMemory = false;

            CompilerResults compilerResults = codeProvider.CompileAssemblyFromDom(compilerParameters, razorResults.GeneratedCode);
            if (compilerResults.Errors.Count > 0)
            {
                var compileErrors = new StringBuilder();
                foreach (System.CodeDom.Compiler.CompilerError compileError in compilerResults.Errors)
                    compileErrors.Append(String.Format("Line: {0}\t Col: {1}\t Error: {2}\r\n", compileError.Line, compileError.Column, compileError.ErrorText));

                throw new Exception(compileErrors.ToString() + generatedCode);
            }

            return compilerResults.CompiledAssembly;
        }

        public Assembly ParseAndCompileTemplate<T>(string namespaceOfGeneratedClass, string generatedClassName, string[] ReferencedAssemblies, TextReader sourceCodeReader)
        {
            return ParseAndCompileTemplate(typeof(T), namespaceOfGeneratedClass, generatedClassName, ReferencedAssemblies, sourceCodeReader);
        }
    }
}
