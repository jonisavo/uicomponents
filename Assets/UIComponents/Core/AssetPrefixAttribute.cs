using System;
using System.Diagnostics;
using JetBrains.Annotations;
using UnityEngine.TestTools;

namespace UIComponents
{
    /// <summary>
    /// If present, specifies a prefix for assets configured for a UIComponent.
    /// Used by <see cref="LayoutAttribute"/> and <see cref="StylesheetAttribute"/>.
    /// </summary>
    /// <example>
    /// <code>
    /// // This example uses Resources.
    /// [AssetPrefix("Components/MyComponent/")]
    /// [Layout("MyComponent")] // resolves to Components/MyComponent/MyComponent(.uxml)
    /// [Stylesheet("MyComponent.style")] // resolves to Components/MyComponent/MyComponent.style(.uss)
    /// public partial class MyComponent : UIComponent {}
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    [BaseTypeRequired(typeof(UIComponent))]
    [Conditional("UICOMPONENTS_INCLUDE_ATTRIBUTES")]
    [ExcludeFromCoverage]
    public sealed class AssetPrefixAttribute : Attribute
    {
        public readonly string Path;
        
        public AssetPrefixAttribute(string path)
        {
            Path = path;
        }
    }
}
