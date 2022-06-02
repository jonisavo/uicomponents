using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace UIComponents.Cache
{
    public readonly struct UIComponentCache
    {
        public readonly object[] Attributes;
        public readonly LayoutAttribute LayoutAttribute;
        public readonly List<StylesheetAttribute> StylesheetAttributes;
        public readonly List<AssetPathAttribute> AssetPathAttributes;
        public readonly FieldCache FieldCache;

        public UIComponentCache(Type componentType)
        {
            Attributes = componentType.GetCustomAttributes(true);
            FieldCache = new FieldCache(componentType);

            LayoutAttribute = null;
            StylesheetAttributes = new List<StylesheetAttribute>();
            AssetPathAttributes = new List<AssetPathAttribute>();

            LayoutAttribute = GetSingleAttribute<LayoutAttribute>();
            PopulateAttributeListParentsFirst(componentType, StylesheetAttributes);
            PopulateAttributeList(AssetPathAttributes);
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