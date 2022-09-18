using System;
using JetBrains.Annotations;

namespace UIComponents
{
    /// <summary>
    /// Adds a USS class to the root element of a UIComponent.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    [BaseTypeRequired(typeof(UIComponent))]
    public sealed class RootClassAttribute : UIComponentEffectAttribute
    {
        private readonly string _className;
        
        public RootClassAttribute(string className)
        {
            _className = className;
        }
        
        public override void Apply(UIComponent component)
        {
            component.AddToClassList(_className);
        }
    }
}
