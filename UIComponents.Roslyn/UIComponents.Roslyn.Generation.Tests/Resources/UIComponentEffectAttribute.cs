using System.Diagnostics.CodeAnalysis;

namespace UIComponents
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    [ExcludeFromCodeCoverage]
    public abstract class UIComponentEffectAttribute : Attribute
    {
        public virtual int Priority { get; set; } = 0;

        public abstract void Apply(UIComponent component);
    }
}
