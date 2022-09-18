using System;
using JetBrains.Annotations;

namespace UIComponents
{
    /// <summary>
    /// Specifies the path to the .uxml layout file used by a UIComponent.
    /// </summary>
    /// <seealso cref="AssetPathAttribute"/>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    [BaseTypeRequired(typeof(UIComponent))]
    public sealed class LayoutAttribute : PathAttribute
    {
        public LayoutAttribute(string path)
        {
            Path = path;
        }
    }
}
