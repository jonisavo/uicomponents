﻿//HintName: MyEnumComponentWithUsing.Uxml.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using UIComponents;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class MyEnumComponentWithUsing
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.0")]
    public new partial class UxmlFactory : UxmlFactory<MyEnumComponentWithUsing, UxmlTraits> {}

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.0")]
    public new partial class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlEnumAttributeDescription<MyEnum> m_FieldTrait = new UxmlEnumAttributeDescription<MyEnum> { name = "field-trait" };
        UxmlEnumAttributeDescription<MyEnum> m_PropertyTrait = new UxmlEnumAttributeDescription<MyEnum> { name = "property-trait" };

        [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.0")]
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            var element = (MyEnumComponentWithUsing) ve;

            element.FieldTrait = m_FieldTrait.GetValueFromBag(bag, cc);
            element.PropertyTrait = m_PropertyTrait.GetValueFromBag(bag, cc);
        }
    }
}
