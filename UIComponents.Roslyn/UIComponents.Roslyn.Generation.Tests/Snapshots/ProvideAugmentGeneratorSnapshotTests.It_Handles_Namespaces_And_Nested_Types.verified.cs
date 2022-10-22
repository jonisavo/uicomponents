﻿//HintName: ParentClass.NestedProvideComponent.Provide.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using System;
using UIComponents;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

namespace Components
{
public partial class ParentClass 
{
private partial class NestedProvideComponent
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "0.26.0")]
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

    [GeneratedCode("UIComponents.Roslyn.Generation", "0.26.0")]
    protected override void UIC_PopulateProvideFields()
    {
        UIC_SetProvideField<Dependencies.IDependency, Dependencies.IDependency>(ref Dependency, "Dependency");
        UIC_SetProvideField<Dependencies.Dependency, IDependency>(ref ConcreteDependency, "ConcreteDependency");
        UIC_SetProvideField<Components.ParentClass.IOtherDependency, Components.ParentClass.IOtherDependency>(ref OtherDependency, "OtherDependency");
    }
}
}
}