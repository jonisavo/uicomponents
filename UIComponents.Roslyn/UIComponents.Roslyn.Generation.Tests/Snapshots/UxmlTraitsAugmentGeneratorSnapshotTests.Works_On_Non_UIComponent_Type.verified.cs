﻿//HintName: NonUIComponentClass.UxmlTraits.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using UnityEngine.UIElements;

public partial class NonUIComponentClass
{
    public new partial class UxmlFactory : UxmlFactory<NonUIComponentClass, UxmlTraits> {}

    public new partial class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlIntAttributeDescription m_FieldTrait = new UxmlIntAttributeDescription { name = "custom-trait-name" };
        UxmlFloatAttributeDescription m_PropertyTrait = new UxmlFloatAttributeDescription { name = "my-property" };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            ((NonUIComponentClass)ve).FieldTrait = m_FieldTrait.GetValueFromBag(bag, cc);
            ((NonUIComponentClass)ve).PropertyTrait = m_PropertyTrait.GetValueFromBag(bag, cc);
        }
    }
}
