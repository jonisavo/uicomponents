//HintName: BaseClass.ClickHandler.RegistersEventCallbackAttribute.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using UIComponents.InterfaceModifiers;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class BaseClass 
{
private partial class ClickHandler
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.9")]
    protected override void UIC_RegisterEventCallbacks()
    {
        RegisterCallback<OnClickEvent>(OnClick);
    }

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.9")]
    protected override void UIC_UnregisterEventCallbacks()
    {
        UnregisterCallback<OnClickEvent>(OnClick);
    }
}
}
