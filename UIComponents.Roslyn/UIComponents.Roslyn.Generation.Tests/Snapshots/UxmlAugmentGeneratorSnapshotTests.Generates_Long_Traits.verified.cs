﻿//HintName: LongComponentWithUsing.Uxml.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class LongComponentWithUsing
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.1")]
    public new partial class UxmlFactory : UxmlFactory<LongComponentWithUsing, UxmlTraits> {}

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.1")]
    public new partial class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlLongAttributeDescription m_FieldTrait = new UxmlLongAttributeDescription { name = "field-trait" };
        UxmlLongAttributeDescription m_PropertyTrait = new UxmlLongAttributeDescription { name = "property-trait" };

        [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.1")]
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            ((LongComponentWithUsing)ve).FieldTrait = m_FieldTrait.GetValueFromBag(bag, cc);
            ((LongComponentWithUsing)ve).PropertyTrait = m_PropertyTrait.GetValueFromBag(bag, cc);
        }
    }
}
