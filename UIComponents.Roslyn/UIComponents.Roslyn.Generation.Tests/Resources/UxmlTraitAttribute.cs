using System.Diagnostics.CodeAnalysis;

namespace UIComponents
{
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class UxmlTraitAttribute : Attribute
    {
        public string? Name { get; set; }
        
        public object? DefaultValue { get; set; }
    }
}
