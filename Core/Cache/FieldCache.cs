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
        public readonly Dictionary<FieldInfo, QueryAttributeBase[]> QueryAttributes;

        public FieldCache(Type type)
        {
            var fieldInfos = type.GetFields(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            
            QueryAttributes = new Dictionary<FieldInfo, QueryAttributeBase[]>();

            for (var i = 0; i < fieldInfos.Length; i++)
            {
                var queryAttributes = (QueryAttributeBase[]) fieldInfos[i].GetCustomAttributes<QueryAttributeBase>();

                if (queryAttributes.Length > 0)
                    QueryAttributes[fieldInfos[i]] = queryAttributes;
            }
        }
    }
}