using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace UIComponents.Experimental
{
    /// <summary>
    /// An attribute for specifying the class names of VisualElements, which are then
    /// automatically queried and populated in the UIComponent constructor.
    /// If the field is not an array or List, the first element is used.
    /// Multiple attributes can be specified for the same field.
    /// </summary>
    /// <seealso cref="QueryAttribute"/>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public class QueryClassAttribute : QueryAttributeBase
    {
        private readonly string _className;
        
        public QueryClassAttribute(string className)
        {
            _className = className;
        }

        public override void Query(VisualElement root, List<VisualElement> results)
        {
            root.Query(null, _className).ToList(results);
        }
    }
}