﻿//HintName: LongTraitNameComponent.Uxml.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using UIComponents;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class LongTraitNameComponent
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.5")]
    public new partial class UxmlFactory : UxmlFactory<LongTraitNameComponent, UxmlTraits> {}

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.5")]
    public new partial class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlIntAttributeDescription m_HereIsALongMemberNameWithALotOfComplexity123Test_Hello___WorldA = new UxmlIntAttributeDescription { name = "here-is-a-long-member-name-with-a-lot-of-complexity123-test-hello-world-a" };

        [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.5")]
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            var element = (LongTraitNameComponent) ve;

            element.HereIsALongMemberNameWithALotOfComplexity123Test_Hello___WorldA = m_HereIsALongMemberNameWithALotOfComplexity123Test_Hello___WorldA.GetValueFromBag(bag, cc);
        }
    }
}
