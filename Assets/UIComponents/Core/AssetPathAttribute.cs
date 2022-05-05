using System;
using JetBrains.Annotations;

namespace UIComponents
{
    /// <summary>
    /// If present, specifies a base path of assets configured for a UIComponent.
    /// Multiple asset paths can be specified.
    /// </summary>
    /// <example>
    /// <code>
    /// // This example uses Resources.
    /// [AssetPath("Components/MyComponent")]
    /// [Layout("MyComponent")] // resolves to Components/MyComponent/MyComponent(.uxml)
    /// [Stylesheet("MyComponent.style")] // resolves to Components/MyComponent/MyComponent.style(.uss)
    /// public class MyComponent : UIComponent {}
    /// </code>
    /// </example>
    /// <example>
    /// <code>
    /// // This example uses Resources.
    /// [AssetPath("Components")] // contains .uxml files
    /// [AssetPath("Styles")] // contains .uss files
    /// public abstract class MyComponentBase : UIComponent {}
    /// 
    /// [Layout("MyComponent/MyComponent")] // resolves to Components/MyComponent/MyComponent(.uxml)
    /// [Stylesheet("MyComponent/MyComponent.style)] // resolves to Components/MyComponent/MyComponent.style(.uss)
    /// [Stylesheet("Common")] // resolves to Styles/Common(.uss)
    /// public class MyComponent : MyComponentBase {}
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    [BaseTypeRequired(typeof(UIComponent))]
    public class AssetPathAttribute : Attribute
    {
        public readonly string Path;
        
        public AssetPathAttribute(string path)
        {
            Path = path;
        }
    }
}