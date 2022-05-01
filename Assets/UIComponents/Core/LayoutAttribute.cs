using System;
using JetBrains.Annotations;

namespace UIComponents.Core
{
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