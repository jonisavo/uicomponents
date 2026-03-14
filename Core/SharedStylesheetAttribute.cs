using System;
using JetBrains.Annotations;
using UnityEngine.TestTools;

namespace UIComponents
{
    /// <summary>
    /// Specifies a shared stylesheet to load by logical name.
    /// Unlike <see cref="StylesheetAttribute"/>, this always requires
    /// an explicit name and does not support convention-based resolution.
    /// </summary>
    /// <seealso cref="StylesheetAttribute"/>
    /// <seealso cref="AssetPrefixAttribute"/>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    [BaseTypeRequired(typeof(UIComponent))]
    [ExcludeFromCoverage]
    public sealed class SharedStylesheetAttribute : Attribute
    {
        public readonly string Name;

        public SharedStylesheetAttribute(string name)
        {
            Name = name;
        }
    }
}
