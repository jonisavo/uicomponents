﻿//HintName: ColorComponentWithUsing.Uxml.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using UnityEngine;
using UIComponents;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class ColorComponentWithUsing
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.5")]
    public new partial class UxmlFactory : UxmlFactory<ColorComponentWithUsing, UxmlTraits> {}

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.5")]
    public new partial class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlColorAttributeDescription m_FieldTrait = new UxmlColorAttributeDescription { name = "field-trait" };
        UxmlColorAttributeDescription m_PropertyTrait = new UxmlColorAttributeDescription { name = "property-trait" };

        [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.5")]
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            var element = (ColorComponentWithUsing) ve;

            element.FieldTrait = m_FieldTrait.GetValueFromBag(bag, cc);
            element.PropertyTrait = m_PropertyTrait.GetValueFromBag(bag, cc);
        }
    }
}
