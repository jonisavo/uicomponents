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

        public UIComponentCache(Type componentType)
        {
            Attributes = componentType.GetCustomAttributes(true);

            LayoutAttribute = null;
            StylesheetAttributes = new List<StylesheetAttribute>();
            AssetPathAttributes = new List<AssetPathAttribute>();

            LayoutAttribute = GetSingleAttribute<LayoutAttribute>();
            PopulateAttributeList(StylesheetAttributes);
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
    }
}