using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine.TestTools;
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
    /// public partial class ComponentWithQueries : UIComponent
    /// {
    ///     [Query(Name = "hello-world-label")]
    ///     private Label HelloWorldLabel;
    ///
    ///     [Query(Class = "red")]
    ///     private Label[] RedLabels;
    ///
    ///     [Query]
    ///     private Label FirstLabel;
    ///     
    ///     [Query]
    ///     public Label[] AllLabels;
    /// }
    /// </example>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    [Conditional("UICOMPONENTS_INCLUDE_ATTRIBUTES")]
    [ExcludeFromCoverage]
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
