using System;

namespace UIComponents.Internal
{
    /// <summary>
    /// Internal utilities for types.
    /// </summary>
    internal static class TypeUtils
    {
        /// <summary>
        /// Returns a concrete type that is not an array or List.
        /// </summary>
        /// <param name="type">Array, List or concrete type</param>
        /// <returns>Concrete type</returns>
        public static Type GetConcreteType(Type type)
        {
            if (type.IsArray)
                return type.GetElementType();

            if (CollectionUtils.TypeQualifiesAsList(type))
                return type.GenericTypeArguments[0];

            return type;
        }
    }
}