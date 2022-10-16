using System.Diagnostics.CodeAnalysis;

namespace UIComponents
{
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AssetPathAttribute : Attribute
    {
        public readonly string Path;

        public AssetPathAttribute(string path)
        {
            Path = path;
        }
    }
}
