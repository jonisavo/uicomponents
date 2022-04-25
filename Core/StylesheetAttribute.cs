using System;
using JetBrains.Annotations;

namespace UIComponents.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    [BaseTypeRequired(typeof(UIComponent))]
    public class StylesheetAttribute : PathAttribute
    {
        public StylesheetAttribute(string path)
        {
            Path = path;
        }
    }
}