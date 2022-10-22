using System;
using JetBrains.Annotations;

namespace UIComponents
{
    /// <summary>
    /// An attribute used to apply effects to a <see cref="UIComponent"/>.
    /// They are applied after the layout and assets have been loaded.
    /// Attributes with higher priority are applied first.
    /// </summary>
    [BaseTypeRequired(typeof(UIComponent))]
    public abstract class UIComponentEffectAttribute : Attribute, IComparable<UIComponentEffectAttribute>
    {
        /// <summary>
        /// The priority of the effect. Defaults to 0.
        /// </summary>
        public virtual int Priority { get; set; } = 0;

        public abstract void Apply(UIComponent component);
        
        public int CompareTo(UIComponentEffectAttribute other)
        {
            return other.Priority.CompareTo(Priority);
        }
    }
}
