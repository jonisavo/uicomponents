using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

namespace UIComponents.Experimental
{
    public abstract class QueryAttributeBase : Attribute
    {
        public abstract void Query(VisualElement root, List<VisualElement> results);
    }
}