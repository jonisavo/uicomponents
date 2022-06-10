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

        public FieldCache(Type type)
        {
            var fieldInfos = type.GetFields(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            
            QueryAttributes = new Dictionary<FieldInfo, QueryAttribute[]>();

            for (var i = 0; i < fieldInfos.Length; i++)
            {
                var queryAttributes = (QueryAttribute[]) fieldInfos[i].GetCustomAttributes<QueryAttribute>();

                if (queryAttributes.Length > 0)
                    QueryAttributes[fieldInfos[i]] = queryAttributes;
            }
        }
    }
}