﻿//HintName: Tests.OnCallbackComponent.RegistersEventCallbackAttribute.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using UIComponents;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

namespace UIComponents.Tests.Editor.Interfaces
{
public partial class Tests 
{
private partial class OnCallbackComponent
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.9")]
    protected override void UIC_RegisterEventCallbacks()
    {
        RegisterCallback<CallbackEvent>(OnCallback);
    }

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.9")]
    protected override void UIC_UnregisterEventCallbacks()
    {
        UnregisterCallback<CallbackEvent>(OnCallback);
    }
}
}
}
