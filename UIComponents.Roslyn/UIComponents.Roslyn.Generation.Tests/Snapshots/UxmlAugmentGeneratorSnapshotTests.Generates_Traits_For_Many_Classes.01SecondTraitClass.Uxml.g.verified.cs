﻿//HintName: SecondTraitClass.Uxml.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class SecondTraitClass
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.2")]
    public new partial class UxmlFactory : UxmlFactory<SecondTraitClass, UxmlTraits> {}

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.2")]
    public new partial class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlBoolAttributeDescription m_Enabled = new UxmlBoolAttributeDescription { name = "enabled" };
        UxmlLongAttributeDescription m_SomeValue = new UxmlLongAttributeDescription { name = "secret" };

        [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.2")]
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            m_Enabled.defaultValue = true;
            ((SecondTraitClass)ve).Enabled = m_Enabled.GetValueFromBag(bag, cc);
            ((SecondTraitClass)ve).SomeValue = m_SomeValue.GetValueFromBag(bag, cc);
        }
    }
}
