//HintName: ConsumerClass.Dependencies.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using System.Collections.Generic;
using UIComponents.DependencyInjection;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class ConsumerClass
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.2")]
    private static readonly IDependency[] UIC_Dependencies = new IDependency[] {
        new Dependency<IMyDependency>((UIComponents.Scope) 0, () => new MyDependency()),
        new Dependency<ISecondDependency>((UIComponents.Scope) 1, () => new SecondDependency())
    };

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.2")]
    public IEnumerable<IDependency> GetDependencies()
    {
        return UIC_Dependencies;
    }
}
