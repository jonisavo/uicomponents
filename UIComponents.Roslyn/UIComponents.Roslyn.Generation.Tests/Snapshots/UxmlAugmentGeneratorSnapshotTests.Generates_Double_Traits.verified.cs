﻿//HintName: DoubleComponentWithUsing.Uxml.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class DoubleComponentWithUsing
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.3")]
    public new partial class UxmlFactory : UxmlFactory<DoubleComponentWithUsing, UxmlTraits> {}

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.3")]
    public new partial class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlDoubleAttributeDescription m_FieldTrait = new UxmlDoubleAttributeDescription { name = "field-trait" };
        UxmlDoubleAttributeDescription m_PropertyTrait = new UxmlDoubleAttributeDescription { name = "property-trait" };

        [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.3")]
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            ((DoubleComponentWithUsing)ve).FieldTrait = m_FieldTrait.GetValueFromBag(bag, cc);
            ((DoubleComponentWithUsing)ve).PropertyTrait = m_PropertyTrait.GetValueFromBag(bag, cc);
        }
    }
}
