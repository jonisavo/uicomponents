using System;
using System.Diagnostics;
using JetBrains.Annotations;
using UnityEngine.TestTools;

namespace UIComponents
{
    /// <summary>
    /// Specifies the path to the .uxml layout file used by a UIComponent.
    /// </summary>
    /// <seealso cref="AssetPrefixAttribute"/>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    [BaseTypeRequired(typeof(UIComponent))]
    [Conditional("UICOMPONENTS_INCLUDE_ATTRIBUTES")]
    [ExcludeFromCoverage]
    public sealed class LayoutAttribute : Attribute
    {
        public readonly string Path;
        
        public LayoutAttribute(string path)
        {
            Path = path;
        }
    }
}
