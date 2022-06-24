using System;
using System.Collections.Generic;
using System.Reflection;
using UIComponents.Experimental;

namespace UIComponents.Cache
{
    /// <summary>
    /// A cache for storing UIComponent field information.
    /// </summary>
    public readonly struct FieldCache
    {
        public readonly Dictionary<FieldInfo, QueryAttribute[]> QueryAttributes;
        public readonly Dictionary<FieldInfo, ProvideAttribute> ProvideAttributes;

        public FieldCache(Type type)
        {
            var fieldInfos = type.GetFields(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            
            QueryAttributes = new Dictionary<FieldInfo, QueryAttribute[]>();
            ProvideAttributes = new Dictionary<FieldInfo, ProvideAttribute>();

            for (var i = 0; i < fieldInfos.Length; i++)
            {
                var queryAttributes = (QueryAttribute[]) fieldInfos[i].GetCustomAttributes<QueryAttribute>();
                var provideAttribute = fieldInfos[i].GetCustomAttribute<ProvideAttribute>();

                if (queryAttributes.Length > 0)
                    QueryAttributes[fieldInfos[i]] = queryAttributes;

                if (provideAttribute != null)
                    ProvideAttributes[fieldInfos[i]] = provideAttribute;
            }
        }
    }
}