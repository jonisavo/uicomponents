//HintName: MyComponent.Query.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using System.Collections.Generic;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

namespace MyLibrary.GUI
{
public partial class MyComponent
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.1")]
    protected override void UIC_PopulateQueryFields()
    {
        var UIC_elementList = new List<Core.Elements.MyElement>();
        this.Query<Core.Elements.MyElement>(null, (string) null).ToList(UIC_elementList);
        if (UIC_elementList.Count > 0)
             element = UIC_elementList[0];
    }
}
}
