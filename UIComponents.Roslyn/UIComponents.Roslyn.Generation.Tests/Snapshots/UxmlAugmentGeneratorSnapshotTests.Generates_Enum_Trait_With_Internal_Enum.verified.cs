﻿//HintName: OwnEnumComponent.Uxml.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using UIComponents;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class OwnEnumComponent
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.4")]
    public new partial class UxmlFactory : UxmlFactory<OwnEnumComponent, UxmlTraits> {}

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.4")]
    public new partial class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlEnumAttributeDescription<OwnEnumComponent.OwnEnum> m_FieldTrait = new UxmlEnumAttributeDescription<OwnEnumComponent.OwnEnum> { name = "field-trait" };
        UxmlEnumAttributeDescription<OwnEnumComponent.OwnEnum> m_PropertyTrait = new UxmlEnumAttributeDescription<OwnEnumComponent.OwnEnum> { name = "property-trait" };

        [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.4")]
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            var element = (OwnEnumComponent) ve;

            element.FieldTrait = m_FieldTrait.GetValueFromBag(bag, cc);
            element.PropertyTrait = m_PropertyTrait.GetValueFromBag(bag, cc);
        }
    }
}
