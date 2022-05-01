using System;
using JetBrains.Annotations;

namespace UIComponents.Core
{
    /// <summary>
    /// Specifies the path to the .uxml layout file used by a UIComponent.
    /// <seealso cref="AssetPathAttribute"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    [BaseTypeRequired(typeof(UIComponent))]
    public class LayoutAttribute : PathAttribute
    {
        public LayoutAttribute(string path)
        {
            Path = path;
        }
    }
}