using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine.UIElements;

namespace UIComponents
{
    /// <summary>
    /// An attribute for specifying a single query for a field.
    /// A name and class can be specified for each query.
    /// Queries are made in the UIComponent constructor.
    /// </summary>
    /// <example>
    /// [Layout("MyLayout")]
    /// public class ComponentWithQueries : UIComponent
    /// {
    ///     [Query(Name = "hello-world-label")]
    ///     private readonly Label HelloWorldLabel;
    ///
    ///     [Query(Class = "red")]
    ///     private readonly Label[] RedLabels;
    ///
    ///     [Query]
    ///     private readonly Label FirstLabel;
    ///     
    ///     [Query]
    ///     public readonly Label[] AllLabels;
    /// }
    /// </example>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    [MeansImplicitUse]
    public sealed class QueryAttribute : Attribute
    {
        /// <summary>
        /// USS name to query by.
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// USS class name to query by.
        /// </summary>
        public string Class { get; set; }

        public QueryAttribute(string name)
        {
            Name = name;
        }
        
        public QueryAttribute() {}

        public void Query(VisualElement root, List<VisualElement> results)
        {
            root.Query(Name, Class).ToList(results);
        }
    }
}
