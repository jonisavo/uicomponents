using System;
using JetBrains.Annotations;
using UnityEngine.TestTools;

namespace UIComponents
{
    /// <summary>
    /// Specifies the path to a .uss stylesheet file used by a UIComponent.
    /// When used without a path, convention-based resolution is used:
    /// the component's class name with a ".style" suffix becomes the asset name.
    /// </summary>
    /// <seealso cref="AssetPrefixAttribute"/>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    [BaseTypeRequired(typeof(UIComponent))]
    [ExcludeFromCoverage]
    public sealed class StylesheetAttribute : Attribute
    {
        public readonly string Path;

        public StylesheetAttribute(string path)
        {
            Path = path;
        }

        public StylesheetAttribute() : this(null) {}
    }
}
