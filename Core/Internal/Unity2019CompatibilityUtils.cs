#if !UNITY_2020_3_OR_NEWER
using System;
using System.Collections.Generic;
using System.Reflection;
using UIComponents.Experimental;
using UnityEngine.UIElements;

namespace UIComponents.Internal
{
    /// <summary>
    /// Various compatibility utilities only accessible in Unity 2019.
    /// </summary>
    internal static class Unity2019CompatibilityUtils
    {
        private static readonly Type[] OfTypeParameterTypes = { typeof(string), typeof(string) };
        private const BindingFlags PublicInstanceBindingFlags = BindingFlags.Public | BindingFlags.Instance;
        
        /// <summary>
        /// Ugly reflection hack to get querying by type to work on Unity 2019.
        /// </summary>
        /// <param name="queryAttribute">QueryAttribute to get name and class from</param>
        /// <param name="root">Root element</param>
        /// <param name="desiredType">Desired element type</param>
        /// <param name="results">Results list to populate</param>
        public static void QueryByDesiredType(
            QueryAttribute queryAttribute,
            VisualElement root,
            Type desiredType,
            List<VisualElement> results)
        {
            var ofTypeMethod = typeof(UQueryBuilder<VisualElement>).GetMethod(
                    "OfType",
                    PublicInstanceBindingFlags,
                    null, CallingConventions.Any, OfTypeParameterTypes, null)
                .MakeGenericMethod(desiredType);

            var query = root.Query(queryAttribute.Name, queryAttribute.Class);
            var newQuery = ofTypeMethod.Invoke(query, new object[]{ null, null });

            var toListMethod = typeof(UQueryBuilder<>).MakeGenericType(desiredType)
                .GetMethod(
                    "ToList",
                    PublicInstanceBindingFlags,
                    null,
                    CallingConventions.Any,
                    new []{ typeof(List<>).MakeGenericType(desiredType) },
                    null);

            var list = CollectionUtils.CreateListOfType(desiredType, results);

            toListMethod.Invoke(newQuery, new []{ list });
            
            foreach (var element in list)
                results.Add(element as VisualElement);
        }
    }
}
#endif