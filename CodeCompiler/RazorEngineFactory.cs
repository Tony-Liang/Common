using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Razor;
using System.Reflection;

namespace LCW.Framework.Common.CodeCompiler
{
    public class RazorEngineFactory<T> where T : RazorTemplateBase
    {
        private static RazorEngineFactory<T> Current;

        private AppDomain LocalAppDomain;      

        public static RazorEngine<T> CreateRazorHostInAppDomain()
        {
            if (Current == null)
                Current = new RazorEngineFactory<T>();

            return Current.GetRazorHostInAppDomain();
        }

        public static void UnloadRazorHostInAppDomain()
        {
            if (Current != null)
                Current.UnloadHost();
            Current = null;
        }

        public RazorEngine<T> GetRazorHostInAppDomain()
        {
            LocalAppDomain = CreateAppDomain(null);
            if (LocalAppDomain == null)
                return null;

            /// Create the instance inside of the new AppDomain
            /// Note: remote domain uses local EXE's AppBasePath!!!
            RazorEngine<T> host = null;

            try
            {
                Assembly ass = Assembly.GetExecutingAssembly();

                string AssemblyPath = ass.Location;

                host = (RazorEngine<T>)LocalAppDomain.CreateInstanceFrom(AssemblyPath,
                                                       typeof(RazorEngine<T>).FullName).Unwrap();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return host;
        }

        private AppDomain CreateAppDomain(string appDomainName)
        {
            if (appDomainName == null)
                appDomainName = "RazorHost_" + Guid.NewGuid().ToString("n");

            AppDomainSetup setup = new AppDomainSetup();

            // *** Point at current directory
            setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;

            AppDomain localDomain = AppDomain.CreateDomain(appDomainName, null, setup);

            return localDomain;
        }

        /// <summary>
        /// Allow unloading of the created AppDomain to release resources
        /// All internal resources in the AppDomain are released including
        /// in memory compiled Razor assemblies.
        /// </summary>
        public void UnloadHost()
        {
            if (this.LocalAppDomain != null)
            {
                AppDomain.Unload(this.LocalAppDomain);
                this.LocalAppDomain = null;
            }
        }
    }
}
