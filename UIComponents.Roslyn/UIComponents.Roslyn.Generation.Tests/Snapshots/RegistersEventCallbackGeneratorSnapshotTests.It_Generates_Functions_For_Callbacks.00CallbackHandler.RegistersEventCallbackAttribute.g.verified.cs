//HintName: CallbackHandler.RegistersEventCallbackAttribute.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using UIComponents.InterfaceModifiers;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class CallbackHandler
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.0")]
    protected override void UIC_RegisterEventCallbacks()
    {
        RegisterCallback<MyEvent>(OnMyEvent);
    }

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.0")]
    protected override void UIC_UnregisterEventCallbacks()
    {
        UnregisterCallback<MyEvent>(OnMyEvent);
    }
}
