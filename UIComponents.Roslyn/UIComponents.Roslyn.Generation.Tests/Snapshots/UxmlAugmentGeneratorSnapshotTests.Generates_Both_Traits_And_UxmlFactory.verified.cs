﻿//HintName: ComponentWithUxmlNameAndTraits.Uxml.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using UIComponents;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class ComponentWithUxmlNameAndTraits
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.5")]
    public new partial class UxmlFactory : UxmlFactory<ComponentWithUxmlNameAndTraits, UxmlTraits>
    {
        [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.5")]
        public override string uxmlName
        {
            get { return "AwesomeUxmlName"; }
        }

        [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.5")]
        public override string uxmlQualifiedName
        {
            get { return uxmlNamespace + "." + uxmlName; }
        }
    }

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.5")]
    public new partial class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlBoolAttributeDescription m_Value = new UxmlBoolAttributeDescription { name = "value" };

        [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.5")]
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            var element = (ComponentWithUxmlNameAndTraits) ve;

            m_Value.defaultValue = true;
            element.Value = m_Value.GetValueFromBag(bag, cc);
        }
    }
}
