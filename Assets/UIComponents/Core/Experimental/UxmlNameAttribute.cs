using System;
using System.Diagnostics;
using JetBrains.Annotations;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace UIComponents.Experimental
{
    /// <summary>
    /// When used in Unity 2021.2 or later, this attribute will generate
    /// a UxmlFactory implementation automatically when
    /// applied to a partial class.
    /// </summary>
    /// <example>
    /// <code>
    /// [UxmlName("ListHeader")]
    /// public partial class ListHeaderComponent : UIComponent {}
    ///
    /// // This generates:
    ///
    /// public partial class ListHeaderComponent
    /// {
    ///     public new class UxmlFactory : UxmlFactory&lt;ListHeaderComponent&gt;
    ///     {
    ///         public override string uxmlName
    ///         {
    ///             get { return "ListHeader"; }
    ///         }
    ///
    ///         public override string uxmlQualifiedName
    ///         {
    ///             get { return uxmlNamespace + "." + uxmlName; }
    ///         }
    ///     }
    /// }
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    [Conditional("UNITY_EDITOR")]
    [BaseTypeRequired(typeof(VisualElement))]
    [ExcludeFromCoverage]
    public sealed class UxmlNameAttribute : Attribute
    {
        /// <summary>
        /// The custom UXML name.
        /// </summary>
        public readonly string Name;

        public UxmlNameAttribute(string name)
        {
            Name = name;
        }
    }
}
