﻿//HintName: Parent.PTwo.PThree.PFour.SecondNestedComponent.Query.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using System.Collections.Generic;
using UnityEngine.UIElements;
using UIComponents;
using System.CodeDom.Compiler;

public partial class Parent 
{
private partial class PTwo 
{
protected partial class PThree 
{
internal partial class PFour 
{
private partial class SecondNestedComponent
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.9")]
    protected override void UIC_PopulateQueryFields()
    {
        // component
        var UIC_componentList = new List<FirstNestedComponent>();
        this.Query<FirstNestedComponent>(null, (string) null).ToList(UIC_componentList);
        if (UIC_componentList.Count == 0)
            Logger.LogError("Query (component): No instances of FirstNestedComponent found", this);
        if (UIC_componentList.Count > 0)
            component = UIC_componentList[0];
    }
}
}
}
}
}
