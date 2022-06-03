using System;

namespace UIComponents.Experimental
{
    /// <summary>
    /// An attribute for specifying the name of a VisualElement, which is then
    /// automatically queried in the UIComponent constructor.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class QueryAttribute : Attribute
    {
        /// <summary>
        /// The name used to query for a VisualElement.
        /// </summary>
        public readonly string Name;

        public QueryAttribute(string name)
        {
            Name = name;
        }
    }
}