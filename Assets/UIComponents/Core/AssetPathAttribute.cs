using System;
using System.Diagnostics;
using JetBrains.Annotations;
using UnityEngine.TestTools;

namespace UIComponents
{
    /// <summary>
    /// If present, specifies a base path of assets configured for a UIComponent.
    /// </summary>
    /// <example>
    /// <code>
    /// // This example uses Resources.
    /// [AssetPath("Components/MyComponent/")]
    /// [Layout("MyComponent")] // resolves to Components/MyComponent/MyComponent(.uxml)
    /// [Stylesheet("MyComponent.style")] // resolves to Components/MyComponent/MyComponent.style(.uss)
    /// public class MyComponent : UIComponent {}
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    [BaseTypeRequired(typeof(UIComponent))]
    [Conditional("UICOMPONENTS_INCLUDE_ATTRIBUTES")]
    [ExcludeFromCoverage]
    public sealed class AssetPathAttribute : Attribute
    {
        public readonly string Path;
        
        public AssetPathAttribute(string path)
        {
            Path = path;
        }
    }
}
