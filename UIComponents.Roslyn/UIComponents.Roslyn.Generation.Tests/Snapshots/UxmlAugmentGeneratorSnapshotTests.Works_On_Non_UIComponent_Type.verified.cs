﻿//HintName: NonUIComponentClass.Uxml.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class NonUIComponentClass
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.2")]
    public new partial class UxmlFactory : UxmlFactory<NonUIComponentClass, UxmlTraits> {}

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.2")]
    public new partial class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlIntAttributeDescription m_FieldTrait = new UxmlIntAttributeDescription { name = "custom-trait-name" };
        UxmlFloatAttributeDescription m_PropertyTrait = new UxmlFloatAttributeDescription { name = "my-property" };

        [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.2")]
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            ((NonUIComponentClass)ve).FieldTrait = m_FieldTrait.GetValueFromBag(bag, cc);
            ((NonUIComponentClass)ve).PropertyTrait = m_PropertyTrait.GetValueFromBag(bag, cc);
        }
    }
}
