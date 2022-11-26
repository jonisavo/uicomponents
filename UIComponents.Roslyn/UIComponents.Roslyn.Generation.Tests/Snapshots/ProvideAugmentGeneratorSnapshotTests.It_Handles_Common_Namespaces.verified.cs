//HintName: GuiComponent.Provide.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using UIComponents;
using MyLibrary.Core.Services;
using System;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

namespace MyLibrary.GUI
{
public partial class GuiComponent
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.5")]
    private void UIC_SetProvideField<TField, TCastFrom>(ref TField value, string fieldName) where TField : class where TCastFrom : class
    {
        try
        {
            value = (TField) (object) Provide<TCastFrom>();
        }
        catch (MissingProviderException)
        {
            Logger.LogError($"Could not provide {typeof(TField).Name} to {fieldName}", this);
        }
        catch (InvalidCastException)
        {
            Logger.LogError($"Could not cast {typeof(TCastFrom).Name} to {typeof(TField).Name}", this);
        }
    }

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.5")]
    protected override void UIC_PopulateProvideFields()
    {
        UIC_SetProvideField<IService, IService>(ref service, "service");
    }
}
}
