//HintName: ParentClass.NestedComponent.Query.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using System.Collections.Generic;
using UnityEngine.UIElements;
using UIComponents;
using System.CodeDom.Compiler;

namespace UILibrary.Components
{
public partial class ParentClass 
{
public partial class NestedComponent
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.6")]
    protected override void UIC_PopulateQueryFields()
    {
        // field
        var UIC_fieldList = new List<VisualElement>();
        this.Query<VisualElement>(null, (string) null).ToList(UIC_fieldList);
        if (UIC_fieldList.Count == 0)
            Logger.LogError("Query (field): No instances of VisualElement found", this);
        if (UIC_fieldList.Count > 0)
            field = UIC_fieldList[0];

        // elements
        var UIC_elementsList = new List<VisualElement>();
        this.Query<VisualElement>("uxml-name", "class-name").ToList(UIC_elementsList);
        if (UIC_elementsList.Count == 0)
            Logger.LogError("Query (elements): No instances of VisualElement found", this);
        elements = UIC_elementsList;
    }
}
}
}
