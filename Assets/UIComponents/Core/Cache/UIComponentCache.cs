using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace UIComponents.Cache
{
    /// <summary>
    /// A cache for the attributes and fields of a UIComponent.
    /// </summary>
    public readonly struct UIComponentCache
    {
        public readonly object[] Attributes;
        public readonly List<UIComponentEffectAttribute> EffectAttributes;
        public readonly FieldCache FieldCache;

        public UIComponentCache(Type componentType)
        {
            Attributes = componentType.GetCustomAttributes(true);
            FieldCache = new FieldCache(componentType);
            
            EffectAttributes = new List<UIComponentEffectAttribute>();
            
            PopulateAttributeList(EffectAttributes);
            
            EffectAttributes.Sort((first, second) => second.CompareTo(first));
        }

        private void PopulateAttributeList<T>(ICollection<T> list) where T : Attribute
        {
            for (var i = 0; i < Attributes.Length; i++)
                if (Attributes[i] is T castAttribute)
                    list.Add(castAttribute);
        }
    }
}
