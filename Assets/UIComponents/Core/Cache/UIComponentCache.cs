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
        public readonly LayoutAttribute LayoutAttribute;
        public readonly List<StylesheetAttribute> StylesheetAttributes;
        public readonly List<AssetPathAttribute> AssetPathAttributes;
        public readonly List<UIComponentEffectAttribute> EffectAttributes;
        public readonly FieldCache FieldCache;

        public UIComponentCache(Type componentType)
        {
            Attributes = componentType.GetCustomAttributes(true);
            FieldCache = new FieldCache(componentType);

            LayoutAttribute = null;
            StylesheetAttributes = new List<StylesheetAttribute>();
            AssetPathAttributes = new List<AssetPathAttribute>();
            EffectAttributes = new List<UIComponentEffectAttribute>();

            LayoutAttribute = GetSingleAttribute<LayoutAttribute>();
            PopulateAttributeListParentsFirst(componentType, StylesheetAttributes);
            PopulateAttributeList(AssetPathAttributes);
            PopulateAttributeList(EffectAttributes);
            
            EffectAttributes.Sort((first, second) => second.CompareTo(first));
        }
        
        [CanBeNull]
        private T GetSingleAttribute<T>() where T : Attribute
        {
            for (var i = 0; i < Attributes.Length; i++)
                if (Attributes[i] is T castAttribute)
                    return castAttribute;

            return null;
        }
        
        private void PopulateAttributeList<T>(ICollection<T> list) where T : Attribute
        {
            for (var i = 0; i < Attributes.Length; i++)
                if (Attributes[i] is T castAttribute)
                    list.Add(castAttribute);
        }

        /// <summary>
        /// GetCustomAttributes for the given type, but get the
        /// attributes from the base types first.
        /// </summary>
        private static void PopulateAttributeListParentsFirst<T>(
            Type componentType, ICollection<T> list) where T : Attribute
        {
            var uiComponentType = typeof(UIComponent);
            var baseTypeList = new List<Type>();
            
            while (componentType != null && componentType != uiComponentType)
            {
                baseTypeList.Insert(0, componentType);
                componentType = componentType.BaseType;
            }
            
            foreach (var baseType in baseTypeList)
            {
                var attributes = (T[]) baseType.GetCustomAttributes(typeof(T), false);
                for (var i = 0; i < attributes.Length; i++)
                    list.Add(attributes[i]);
            }
        }
    }
}