using System;
using JetBrains.Annotations;
using UnityEngine.TestTools;

namespace UIComponents
{
    /// <summary>
    /// Specifies a convention root for assets configured for a UIComponent.
    /// Unlike <see cref="AssetPrefixAttribute"/>, this acts as a convention
    /// root hint rather than a raw path prefix. Used by <see cref="LayoutAttribute"/>
    /// and <see cref="StylesheetAttribute"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    [BaseTypeRequired(typeof(UIComponent))]
    [ExcludeFromCoverage]
    public sealed class AssetRootAttribute : Attribute
    {
        public readonly string Path;

        public AssetRootAttribute(string path)
        {
            Path = path;
        }
    }
}
