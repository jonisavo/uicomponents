﻿//HintName: CallbackHandler.RegistersEventCallbackAttribute.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using UIComponents.InterfaceModifiers;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class CallbackHandler
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.4")]
    protected override void UIC_RegisterEventCallbacks()
    {
        RegisterCallback<MyEvent>(MyEventHappened);
    }

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.4")]
    protected override void UIC_UnregisterEventCallbacks()
    {
        UnregisterCallback<MyEvent>(MyEventHappened);
    }
}
