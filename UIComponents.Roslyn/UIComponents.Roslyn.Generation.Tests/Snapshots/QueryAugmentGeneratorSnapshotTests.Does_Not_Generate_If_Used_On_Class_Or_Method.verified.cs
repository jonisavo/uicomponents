//HintName: InvalidUsageQueryComponent.Query.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using System.Collections.Generic;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class InvalidUsageQueryComponent
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.3")]
    protected override void UIC_PopulateQueryFields()
    {
        // element
        var UIC_elementList = new List<UnityEngine.UIElements.VisualElement>();
        this.Query<UnityEngine.UIElements.VisualElement>("valid-usage", (string) null).ToList(UIC_elementList);
        if (UIC_elementList.Count == 0)
            Logger.LogError("Query (element): No instances of UnityEngine.UIElements.VisualElement found", this);
        if (UIC_elementList.Count > 0)
            element = UIC_elementList[0];
    }
}
