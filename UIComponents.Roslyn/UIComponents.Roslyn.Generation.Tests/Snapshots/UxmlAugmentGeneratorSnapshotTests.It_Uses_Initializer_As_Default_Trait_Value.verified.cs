﻿//HintName: Test.Uxml.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using UIComponents;
using UnityEngine;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class Test
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.7")]
    public new partial class UxmlFactory : UxmlFactory<Test, UxmlTraits> {}

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.7")]
    public new partial class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlIntAttributeDescription m_Number = new UxmlIntAttributeDescription { name = "number" };
        UxmlColorAttributeDescription m_color = new UxmlColorAttributeDescription { name = "color" };
        UxmlStringAttributeDescription m_MyMessage = new UxmlStringAttributeDescription { name = "my-message" };

        [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.7")]
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);

            var element = (Test) ve;

            m_Number.defaultValue = 0;
            m_color.defaultValue = Color.red;
            m_MyMessage.defaultValue = "Hello world!";
            element.Number = m_Number.GetValueFromBag(bag, cc);
            element.color = m_color.GetValueFromBag(bag, cc);
            element.MyMessage = m_MyMessage.GetValueFromBag(bag, cc);
        }
    }
}
