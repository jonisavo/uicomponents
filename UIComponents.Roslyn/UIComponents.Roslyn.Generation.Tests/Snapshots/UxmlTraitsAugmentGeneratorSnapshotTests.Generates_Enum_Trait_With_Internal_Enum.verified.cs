﻿//HintName: OwnEnumComponent.UxmlTraits.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using UnityEngine.UIElements;

public partial class OwnEnumComponent
{
    public new partial class UxmlFactory : UxmlFactory<OwnEnumComponent, UxmlTraits> {}

    public new partial class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlEnumAttributeDescription<OwnEnumComponent.OwnEnum> m_FieldTrait = new UxmlEnumAttributeDescription<OwnEnumComponent.OwnEnum> { name = "fieldtrait" };
        UxmlEnumAttributeDescription<OwnEnumComponent.OwnEnum> m_PropertyTrait = new UxmlEnumAttributeDescription<OwnEnumComponent.OwnEnum> { name = "propertytrait" };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            ((OwnEnumComponent)ve).FieldTrait = m_FieldTrait.GetValueFromBag(bag, cc);
            ((OwnEnumComponent)ve).PropertyTrait = m_PropertyTrait.GetValueFromBag(bag, cc);
        }
    }
}
