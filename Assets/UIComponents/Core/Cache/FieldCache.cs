using System;
using System.Collections.Generic;
using System.Reflection;
using UIComponents.Experimental;

namespace UIComponents.Cache
{
    public readonly struct FieldCache
    {
        public readonly Dictionary<FieldInfo, QueryAttribute> QueryAttributes;

        public FieldCache(Type type)
        {
            var fieldInfos = type.GetFields(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            
            QueryAttributes = new Dictionary<FieldInfo, QueryAttribute>();

            for (var i = 0; i < fieldInfos.Length; i++)
            {
                var queryAttribute = fieldInfos[i].GetCustomAttribute<QueryAttribute>();
                if (queryAttribute != null)
                    QueryAttributes[fieldInfos[i]] = queryAttribute;
            }
        }
    }
}