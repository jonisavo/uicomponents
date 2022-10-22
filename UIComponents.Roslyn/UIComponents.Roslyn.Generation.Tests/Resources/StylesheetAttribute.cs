using System.Diagnostics.CodeAnalysis;

namespace UIComponents
{
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class StylesheetAttribute : Attribute
    {
        public readonly string Path;

        public StylesheetAttribute(string path)
        {
            Path = path;
        }
    }
}
