﻿//HintName: IntComponentWithUsing.Uxml.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using UIComponents;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class IntComponentWithUsing
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.3")]
    public new partial class UxmlFactory : UxmlFactory<IntComponentWithUsing, UxmlTraits> {}

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.3")]
    public new partial class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlIntAttributeDescription m_FieldTrait = new UxmlIntAttributeDescription { name = "field-trait" };
        UxmlIntAttributeDescription m_PropertyTrait = new UxmlIntAttributeDescription { name = "property-trait" };

        [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.3")]
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            var element = (IntComponentWithUsing) ve;

            element.FieldTrait = m_FieldTrait.GetValueFromBag(bag, cc);
            element.PropertyTrait = m_PropertyTrait.GetValueFromBag(bag, cc);
        }
    }
}
