using System.Diagnostics.CodeAnalysis;

namespace UIComponents
{
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class QueryAttribute : Attribute
    {
        public string Name { get; set; }
        public string Class { get; set; }

        public QueryAttribute(string uxmlName = null)
        {
            Name = uxmlName;
        }

        public QueryAttribute() {}
    }
}
