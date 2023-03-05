//HintName: PriorityEffectComponent.Effects.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using UIComponents;
using System;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class PriorityEffectComponent
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.0")]
    private static UIComponentEffectAttribute[] UIC_EffectAttributes;

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.0")]
    private static void UIC_InitializeEffectAttributes()
    {
        UIC_EffectAttributes = new [] {
            new TestEffectAttribute() { Priority = 8 },
            new TestEffectWithArgumentsAttribute(8, "Hello world") { Name = "John Smith", Number = 3.14, Priority = -5 }
        };
        Array.Sort(UIC_EffectAttributes);
    }

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.0")]
    protected override void UIC_ApplyEffects()
    {
        if (UIC_EffectAttributes == null)
            UIC_InitializeEffectAttributes();

        foreach (var effect in UIC_EffectAttributes)
            effect.Apply(this);
    }
}
