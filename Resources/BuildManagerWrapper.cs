using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Compilation;
using System.IO;
using System.Collections;

namespace LCW.Framework.Common.Resources
{
    internal sealed class BuildManagerWrapper : IBuildManager
    {
        bool IBuildManager.FileExists(string virtualPath)
        {
            return BuildManager.GetObjectFactory(virtualPath, false) != null;
        }

        Type IBuildManager.GetCompiledType(string virtualPath)
        {
            return BuildManager.GetCompiledType(virtualPath);
        }

        ICollection IBuildManager.GetReferencedAssemblies()
        {
            return BuildManager.GetReferencedAssemblies();
        }

        Stream IBuildManager.ReadCachedFile(string fileName)
        {
            return BuildManager.ReadCachedFile(fileName);
        }

        Stream IBuildManager.CreateCachedFile(string fileName)
        {
            return BuildManager.CreateCachedFile(fileName);
        }
    }
}
