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
    /// <code>
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
    ///     
    ///     [Query(Name = "first-name")]
    ///     [Query(Name = "second-name")]
    ///     public List&lt;VisualElement&gt; MultipleQueries;
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = false)]
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
    }
}
