using System.Diagnostics.CodeAnalysis;

namespace UIComponents
{
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AssetRootAttribute : Attribute
    {
        public readonly string Path;

        public AssetRootAttribute(string path)
        {
            Path = path;
        }
    }
}
