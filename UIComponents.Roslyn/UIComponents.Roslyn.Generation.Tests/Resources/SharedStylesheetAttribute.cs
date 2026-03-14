using System.Diagnostics.CodeAnalysis;

namespace UIComponents
{
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class SharedStylesheetAttribute : Attribute
    {
        public readonly string Name;

        public SharedStylesheetAttribute(string name)
        {
            Name = name;
        }
    }
}
