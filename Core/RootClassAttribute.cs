using System;

namespace UIComponents
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RootClassAttribute : UIComponentEffectAttribute
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