using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace UIComponents.Experimental
{
    /// <summary>
    /// An attribute for specifying the name of a VisualElement, which is then
    /// automatically queried and populated in the UIComponent constructor.
    /// If the field type is an array or List, the queried element will be
    /// placed into it.
    /// </summary>
    /// <seealso cref="QueryClassAttribute"/>
    [AttributeUsage(AttributeTargets.Field)]
    public class QueryAttribute : QueryAttributeBase
    {
        private readonly string _name;
        
        public QueryAttribute(string name)
        {
            _name = name;
        }

        public override void Query(VisualElement root, List<VisualElement> results)
        {
            root.Query(_name).ToList(results);
        }
    }
}