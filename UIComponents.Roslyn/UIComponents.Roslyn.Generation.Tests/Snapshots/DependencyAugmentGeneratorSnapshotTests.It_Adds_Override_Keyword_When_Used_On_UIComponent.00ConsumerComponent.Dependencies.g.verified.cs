﻿//HintName: ConsumerComponent.Dependencies.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using UIComponents;
using UIComponents.DependencyInjection;
using System.Collections.Generic;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class ConsumerComponent
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.6")]
    private static readonly IDependency[] UIC_Dependencies = new IDependency[] {
        UIComponents.DependencyInjection.Dependency.SingletonFor<UIComponents.IAssetResolver, UIComponents.ResourcesAssetResolver>(),
        UIComponents.DependencyInjection.Dependency.SingletonFor<UIComponents.ILogger, UIComponents.DebugLogger>(),
        UIComponents.DependencyInjection.Dependency.SingletonFor<IMyDependency, MyDependency>()
    };

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.6")]
    public override IEnumerable<IDependency> GetDependencies()
    {
        return UIC_Dependencies;
    }
}
