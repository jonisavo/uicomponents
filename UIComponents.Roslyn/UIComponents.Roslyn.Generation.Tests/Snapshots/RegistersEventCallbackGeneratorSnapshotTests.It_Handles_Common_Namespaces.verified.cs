//HintName: MyComponent.RegistersEventCallbackAttribute.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using UIComponents;
using MyLibrary.Generation.EventInterfaces;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

namespace MyLibrary.Components
{
public partial class MyComponent
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.2")]
    protected override void UIC_RegisterEventCallbacks()
    {
        RegisterCallback<Events.MyEvent>(OnEvent);
    }

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.2")]
    protected override void UIC_UnregisterEventCallbacks()
    {
        UnregisterCallback<Events.MyEvent>(OnEvent);
    }
}
}
