﻿//HintName: FloatComponentWithUsing.Uxml.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class FloatComponentWithUsing
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "0.26.0")]
    public new partial class UxmlFactory : UxmlFactory<FloatComponentWithUsing, UxmlTraits> {}

    [GeneratedCode("UIComponents.Roslyn.Generation", "0.26.0")]
    public new partial class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlFloatAttributeDescription m_FieldTrait = new UxmlFloatAttributeDescription { name = "field-trait" };
        UxmlFloatAttributeDescription m_PropertyTrait = new UxmlFloatAttributeDescription { name = "property-trait" };

        [GeneratedCode("UIComponents.Roslyn.Generation", "0.26.0")]
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            ((FloatComponentWithUsing)ve).FieldTrait = m_FieldTrait.GetValueFromBag(bag, cc);
            ((FloatComponentWithUsing)ve).PropertyTrait = m_PropertyTrait.GetValueFromBag(bag, cc);
        }
    }
}
