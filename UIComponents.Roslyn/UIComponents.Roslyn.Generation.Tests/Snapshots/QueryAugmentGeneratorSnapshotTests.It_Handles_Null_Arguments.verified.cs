﻿//HintName: NullQueryComponent.Query.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using System.Collections.Generic;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class NullQueryComponent
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.3")]
    protected override void UIC_PopulateQueryFields()
    {
        // elements
        var UIC_elementsList = new List<UnityEngine.UIElements.VisualElement>();
        this.Query<UnityEngine.UIElements.VisualElement>(null, (string) null).ToList(UIC_elementsList);
        if (UIC_elementsList.Count == 0)
            Logger.LogError("Query (elements): No instances of UnityEngine.UIElements.VisualElement found", this);
        elements = UIC_elementsList;
    }
}
