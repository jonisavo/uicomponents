//HintName: MyClass.Provide.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using UIComponents;
using System;
using UnityEngine;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class MyClass
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.9")]
    private void UIC_SetProvideField<TField, TCastFrom>(ref TField value, string fieldName) where TField : class where TCastFrom : class
    {
        try
        {
            value = (TField) (object) Provide<TCastFrom>();
        }
        catch (MissingProviderException)
        {
            Debug.LogError($"Could not provide {typeof(TField).Name} to {fieldName}");
        }
        catch (InvalidCastException)
        {
            Debug.LogError($"Could not cast {typeof(TCastFrom).Name} to {typeof(TField).Name}");
        }
    }

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.9")]
    protected void UIC_PopulateProvideFields()
    {
        UIC_SetProvideField<IDependency, IDependency>(ref Dependency, "Dependency");
        UIC_SetProvideField<Dependency, IDependency>(ref ConcreteDependency, "ConcreteDependency");
    }
}
