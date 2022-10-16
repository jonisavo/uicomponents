using System;
using System.Collections.Generic;
using System.Reflection;
using UIComponents.Internal;
using UnityEngine.UIElements;

namespace UIComponents.Cache
{
    /// <summary>
    /// A cache for storing UIComponent field information.
    /// </summary>
    public readonly struct FieldCache
    {
        public readonly Dictionary<FieldInfo, ProvideAttribute> ProvideAttributes;
        
        private static readonly Type VisualElementType = typeof(VisualElement);

        public FieldCache(Type type)
        {
            var fieldInfos = type.GetFields(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            
            ProvideAttributes = new Dictionary<FieldInfo, ProvideAttribute>();

            for (var i = 0; i < fieldInfos.Length; i++)
            {
                var fieldInfo = fieldInfos[i];
                var concreteFieldType = TypeUtils.GetConcreteType(fieldInfo.FieldType);
                
                var fieldIsVisualElement = VisualElementType.IsAssignableFrom(concreteFieldType);
                
                if (!fieldIsVisualElement)
                    CheckForProvideAttribute(fieldInfo);
            }
        }

        private void CheckForProvideAttribute(FieldInfo fieldInfo)
        {
            var provideAttribute = fieldInfo.GetCustomAttribute<ProvideAttribute>();
            
            if (provideAttribute != null)
                ProvideAttributes[fieldInfo] = provideAttribute;
        }
    }
}
