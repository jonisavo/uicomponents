using System;

namespace UIComponents
{
    /// <summary>
    /// Default pass-through asset catalog. Returns generator-computed paths unchanged.
    /// </summary>
    public class DefaultAssetCatalog : IAssetCatalog
    {
        public string ResolveLayoutPath(Type componentType, string path)
        {
            return path;
        }

        public string ResolveStylesheetPath(Type componentType, string path)
        {
            return path;
        }

        public string ResolveSharedStylesheetPath(string name, string path)
        {
            return path;
        }
    }
}
