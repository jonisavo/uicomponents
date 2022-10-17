﻿//HintName: InvalidProvideComponent.Provide.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using System;
using UIComponents;
using UnityEngine.UIElements;

public partial class InvalidProvideComponent
{
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

    protected override void PopulateProvideFields()
    {
        UIC_SetProvideField<IDependency, IDependency>(ref Dependency, "Dependency");
    }
}
