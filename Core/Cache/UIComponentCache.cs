using System;

namespace UIComponents.Cache
{
    /// <summary>
    /// A cache for the attributes and fields of a UIComponent.
    /// </summary>
    public readonly struct UIComponentCache
    {
        public readonly FieldCache FieldCache;

        public UIComponentCache(Type componentType)
        {
            FieldCache = new FieldCache(componentType);
        }
    }
}
