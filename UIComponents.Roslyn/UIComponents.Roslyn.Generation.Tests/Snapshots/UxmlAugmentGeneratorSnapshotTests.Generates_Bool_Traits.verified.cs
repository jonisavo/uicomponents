﻿//HintName: BoolComponentWithUsing.Uxml.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using UIComponents;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class BoolComponentWithUsing
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.6")]
    public new partial class UxmlFactory : UxmlFactory<BoolComponentWithUsing, UxmlTraits> {}

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.6")]
    public new partial class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlBoolAttributeDescription m_FieldTrait = new UxmlBoolAttributeDescription { name = "field-trait" };
        UxmlBoolAttributeDescription m_PropertyTrait = new UxmlBoolAttributeDescription { name = "property-trait" };

        [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.6")]
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            var element = (BoolComponentWithUsing) ve;

            element.FieldTrait = m_FieldTrait.GetValueFromBag(bag, cc);
            element.PropertyTrait = m_PropertyTrait.GetValueFromBag(bag, cc);
        }
    }
}
