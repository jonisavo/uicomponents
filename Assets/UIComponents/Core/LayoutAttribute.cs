using System;
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
