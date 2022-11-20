using System;
using JetBrains.Annotations;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace UIComponents
{
    /// <summary>
    /// When applied to a partial class, this attribute will generate
    /// a UxmlFactory implementation automatically.
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
    [BaseTypeRequired(typeof(VisualElement))]
    [MeansImplicitUse]
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
