using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace UIComponents.Internal
{
    internal static class CollectionUtils
    {
        public static Array CreateArrayOfType(Type elementType, List<VisualElement> elements)
        {
            var array = Array.CreateInstance(elementType, elements.Count);
            
            for (var i = 0; i < elements.Count; i++)
                array.SetValue(elements[i], i);

            return array;
        }

        public static IList CreateListOfType(Type elementType, List<VisualElement> elements)
        {
            var list = (IList) Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));
            
            for (var i = 0; i < elements.Count; i++)
                list.Add(elements[i]);

            return list;
        }
        
        public static bool TypeQualifiesAsList(Type elementType)
        {
            return typeof(IList).IsAssignableFrom(elementType) &&
                   elementType.IsGenericType &&
                   elementType.GenericTypeArguments.Length == 1;
        }
    }
}