﻿//HintName: StringComponentWithUsing.Uxml.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using UIComponents;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class StringComponentWithUsing
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.1")]
    public new partial class UxmlFactory : UxmlFactory<StringComponentWithUsing, UxmlTraits> {}

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.1")]
    public new partial class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlStringAttributeDescription m_FieldTrait = new UxmlStringAttributeDescription { name = "field-trait" };
        UxmlStringAttributeDescription m_PropertyTrait = new UxmlStringAttributeDescription { name = "property-trait" };

        [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.1")]
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            var element = (StringComponentWithUsing) ve;

            element.FieldTrait = m_FieldTrait.GetValueFromBag(bag, cc);
            element.PropertyTrait = m_PropertyTrait.GetValueFromBag(bag, cc);
        }
    }
}
