﻿//HintName: StringComponentWithUsing.Uxml.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class StringComponentWithUsing
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "0.26.0")]
    public new partial class UxmlFactory : UxmlFactory<StringComponentWithUsing, UxmlTraits> {}

    [GeneratedCode("UIComponents.Roslyn.Generation", "0.26.0")]
    public new partial class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlStringAttributeDescription m_FieldTrait = new UxmlStringAttributeDescription { name = "field-trait" };
        UxmlStringAttributeDescription m_PropertyTrait = new UxmlStringAttributeDescription { name = "property-trait" };

        [GeneratedCode("UIComponents.Roslyn.Generation", "0.26.0")]
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            ((StringComponentWithUsing)ve).FieldTrait = m_FieldTrait.GetValueFromBag(bag, cc);
            ((StringComponentWithUsing)ve).PropertyTrait = m_PropertyTrait.GetValueFromBag(bag, cc);
        }
    }
}
