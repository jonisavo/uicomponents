﻿//HintName: CustomNamespaceComponent.Uxml.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using UIComponents;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

namespace Custom.Components
{
public partial class CustomNamespaceComponent
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.1")]
    public new partial class UxmlFactory : UxmlFactory<CustomNamespaceComponent, UxmlTraits> {}

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.1")]
    public new partial class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlEnumAttributeDescription<Custom.Components.CustomNamespaceComponent.OwnEnum> m_FieldTrait = new UxmlEnumAttributeDescription<Custom.Components.CustomNamespaceComponent.OwnEnum> { name = "field-trait" };
        UxmlEnumAttributeDescription<Custom.Components.CustomNamespaceComponent.OwnEnum> m_PropertyTrait = new UxmlEnumAttributeDescription<Custom.Components.CustomNamespaceComponent.OwnEnum> { name = "property-trait" };

        [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.1")]
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            var element = (CustomNamespaceComponent) ve;

            element.FieldTrait = m_FieldTrait.GetValueFromBag(bag, cc);
            element.PropertyTrait = m_PropertyTrait.GetValueFromBag(bag, cc);
        }
    }
}
}
