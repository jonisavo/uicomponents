//HintName: OtherCallbackHandler.RegistersCallbackAttribute.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class OtherCallbackHandler
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.3")]
    protected override void UIC_RegisterCallbacks()
    {
        RegisterCallback<MyEvent>(OnMyEvent);
        RegisterCallback<MyOtherEvent>(OnMyOtherEvent);
    }

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.3")]
    protected override void UIC_UnregisterCallbacks()
    {
        UnregisterCallback<MyEvent>(OnMyEvent);
        UnregisterCallback<MyOtherEvent>(OnMyOtherEvent);
    }
}
