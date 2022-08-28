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
        public readonly Dictionary<FieldInfo, QueryAttribute[]> QueryAttributes;
        public readonly Dictionary<FieldInfo, ProvideAttribute> ProvideAttributes;
        
        private static readonly Type VisualElementType = typeof(VisualElement);

        public FieldCache(Type type)
        {
            var fieldInfos = type.GetFields(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            
            QueryAttributes = new Dictionary<FieldInfo, QueryAttribute[]>();
            ProvideAttributes = new Dictionary<FieldInfo, ProvideAttribute>();

            for (var i = 0; i < fieldInfos.Length; i++)
            {
                var fieldInfo = fieldInfos[i];
                var concreteFieldType = TypeUtils.GetConcreteType(fieldInfo.FieldType);
                
                var fieldIsVisualElement = VisualElementType.IsAssignableFrom(concreteFieldType);
                
                if (fieldIsVisualElement)
                    CheckForQueryAttributes(fieldInfo);
                else
                    CheckForProvideAttribute(fieldInfo);
            }
        }

        private void CheckForQueryAttributes(FieldInfo fieldInfo)
        {
            var queryAttributes = (QueryAttribute[]) fieldInfo.GetCustomAttributes<QueryAttribute>();
            
            if (queryAttributes.Length > 0)
                QueryAttributes[fieldInfo] = queryAttributes;
        }

        private void CheckForProvideAttribute(FieldInfo fieldInfo)
        {
            var provideAttribute = fieldInfo.GetCustomAttribute<ProvideAttribute>();
            
            if (provideAttribute != null)
                ProvideAttributes[fieldInfo] = provideAttribute;
        }
    }
}
