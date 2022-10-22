using System.Diagnostics.CodeAnalysis;

namespace UIComponents
{
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class LayoutAttribute : Attribute
    {
        public readonly string Path;

        public LayoutAttribute(string path)
        {
            Path = path;
        }   
    }
}
