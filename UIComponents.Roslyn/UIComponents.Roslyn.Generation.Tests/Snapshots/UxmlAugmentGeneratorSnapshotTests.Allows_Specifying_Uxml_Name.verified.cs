﻿//HintName: CustomNamespaceComponent.Uxml.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class CustomNamespaceComponent
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.4")]
    public new partial class UxmlFactory : UxmlFactory<CustomNamespaceComponent, UxmlTraits> {}

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.4")]
    public new partial class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlIntAttributeDescription m_FieldTrait = new UxmlIntAttributeDescription { name = "custom-trait-name" };
        UxmlFloatAttributeDescription m_PropertyTrait = new UxmlFloatAttributeDescription { name = "my-property" };

        [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.4")]
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            ((CustomNamespaceComponent)ve).FieldTrait = m_FieldTrait.GetValueFromBag(bag, cc);
            ((CustomNamespaceComponent)ve).PropertyTrait = m_PropertyTrait.GetValueFromBag(bag, cc);
        }
    }
}
