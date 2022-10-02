﻿//HintName: ComponentWithDefaultValueTraits.UxmlTraits.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using UnityEngine.UIElements;

public partial class ComponentWithDefaultValueTraits
{
    public new partial class UxmlFactory : UxmlFactory<ComponentWithDefaultValueTraits, UxmlTraits> {}

    public new partial class UxmlTraits : VisualElement.UxmlTraits
    {
        UxmlStringAttributeDescription m_Description = new UxmlStringAttributeDescription { name = "description" };
        UxmlIntAttributeDescription m_Lives = new UxmlIntAttributeDescription { name = "lives" };
        UxmlEnumAttributeDescription<Some.Place.Where.Enum.Is.TheEnum> m_MyValue = new UxmlEnumAttributeDescription<Some.Place.Where.Enum.Is.TheEnum> { name = "custom-value" };

        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {
            base.Init(ve, bag, cc);
            m_Description.defaultValue = "Description not set.";
            m_Lives.defaultValue = 3;
            m_MyValue.defaultValue = (Some.Place.Where.Enum.Is.TheEnum) 1;
            ((ComponentWithDefaultValueTraits)ve).Description = m_Description.GetValueFromBag(bag, cc);
            ((ComponentWithDefaultValueTraits)ve).Lives = m_Lives.GetValueFromBag(bag, cc);
            ((ComponentWithDefaultValueTraits)ve).MyValue = m_MyValue.GetValueFromBag(bag, cc);
        }
    }
}
