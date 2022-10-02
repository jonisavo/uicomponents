using System.Diagnostics.CodeAnalysis;

namespace UIComponents.Experimental
{
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class TraitAttribute : Attribute
    {
        public string Name { get; set; }
        
        public object DefaultValue { get; set; }
    }
}
