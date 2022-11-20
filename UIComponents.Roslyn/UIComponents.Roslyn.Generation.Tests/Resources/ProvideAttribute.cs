using System.Diagnostics.CodeAnalysis;

namespace UIComponents
{
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class ProvideAttribute : Attribute
    {
        public Type? CastFrom { get; set; }
    }
}
