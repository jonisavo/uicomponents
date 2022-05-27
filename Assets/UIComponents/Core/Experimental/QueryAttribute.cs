using System;

namespace UIComponents.Experimental
{
    [AttributeUsage(AttributeTargets.Field)]
    public class QueryAttribute : Attribute
    {
        public readonly string Name;

        public QueryAttribute(string name)
        {
            Name = name;
        }
    }
}