﻿//HintName: ConsumerClass.Dependencies.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using UIComponents;
using UIComponents.DependencyInjection;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class ConsumerClass
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.4")]
    private static readonly IDependency[] UIC_Dependencies = new IDependency[] {
        UIComponents.DependencyInjection.Dependency.SingletonFor<IMyDependency, MyDependency>(),
        UIComponents.DependencyInjection.Dependency.SingletonFor<ISecondDependency, SecondDependency>()
    };

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.4")]
    public IEnumerable<IDependency> GetDependencies()
    {
        return UIC_Dependencies;
    }
}
