﻿//HintName: NullQueryComponent.Query.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using System.Collections.Generic;
using UnityEngine.UIElements;
using UIComponents;
using System.CodeDom.Compiler;

public partial class NullQueryComponent
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.3")]
    protected override void UIC_PopulateQueryFields()
    {
        // elements
        var UIC_elementsList = new List<VisualElement>();
        this.Query<VisualElement>(null, (string) null).ToList(UIC_elementsList);
        if (UIC_elementsList.Count == 0)
            Logger.LogError("Query (elements): No instances of VisualElement found", this);
        elements = UIC_elementsList;
    }
}
