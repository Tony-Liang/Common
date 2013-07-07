using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LCW.Framework.Common.Resources
{
    interface IBuildManager
    {
        bool FileExists(string virtualPath);
        Type GetCompiledType(string virtualPath);
        ICollection GetReferencedAssemblies();
        Stream ReadCachedFile(string fileName);
        Stream CreateCachedFile(string fileName);
    }
}
