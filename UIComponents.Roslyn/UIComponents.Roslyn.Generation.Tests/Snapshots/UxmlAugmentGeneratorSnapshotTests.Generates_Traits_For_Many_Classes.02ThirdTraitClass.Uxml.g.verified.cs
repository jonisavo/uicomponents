﻿//HintName: ThirdTraitClass.Uxml.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using UIComponents;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class ThirdTraitClass
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.4")]
    public new partial class UxmlFactory : UxmlFactory<ThirdTraitClass, UxmlTraits> {}

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.4")]
    public new partial class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlStringAttributeDescription m_Name = new UxmlStringAttributeDescription { name = "name" };

        [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.4")]
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            var element = (ThirdTraitClass) ve;

            element.Name = m_Name.GetValueFromBag(bag, cc);
        }
    }
}
