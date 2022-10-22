using System.Diagnostics.CodeAnalysis;

namespace UIComponents
{
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AssetPrefixAttribute : Attribute
    {
        public readonly string Path;

        public AssetPrefixAttribute(string path)
        {
            Path = path;
        }
    }
}
