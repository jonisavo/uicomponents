//HintName: MyComponentWithTraits.Uxml.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class MyComponentWithTraits
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "0.26.0")]
    public new partial class UxmlFactory : UxmlFactory<MyComponentWithTraits, UxmlTraits> {}

    [GeneratedCode("UIComponents.Roslyn.Generation", "0.26.0")]
    public new partial class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlDoubleAttributeDescription m_Trait = new UxmlDoubleAttributeDescription { name = "double-value" };

        [GeneratedCode("UIComponents.Roslyn.Generation", "0.26.0")]
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            ((MyComponentWithTraits)ve).Trait = m_Trait.GetValueFromBag(bag, cc);
        }
    }
}
