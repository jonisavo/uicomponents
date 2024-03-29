//HintName: SecondSubclassQueryComponent.Query.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using System.Collections.Generic;
using UnityEngine.UIElements;
using UIComponents;
using System.CodeDom.Compiler;

public partial class SecondSubclassQueryComponent
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.9")]
    protected override void UIC_PopulateQueryFields()
    {
        // subclassQueryComponent
        var UIC_subclassQueryComponentList = new List<SubclassQueryComponent>();
        this.Query<SubclassQueryComponent>(null, "asdf").ToList(UIC_subclassQueryComponentList);
        if (UIC_subclassQueryComponentList.Count == 0)
            Logger.LogError("Query (subclassQueryComponent): No instances of SubclassQueryComponent found", this);
        if (UIC_subclassQueryComponentList.Count > 0)
            subclassQueryComponent = UIC_subclassQueryComponentList[0];

        // subclassElements
        var UIC_subclassElementsList = new List<VisualElement>();
        this.Query<VisualElement>("foo", "bar").ToList(UIC_subclassElementsList);
        if (UIC_subclassElementsList.Count == 0)
            Logger.LogError("Query (subclassElements): No instances of VisualElement found", this);
        subclassElements = new VisualElement[UIC_subclassElementsList.Count];
        for (var i = 0; i < UIC_subclassElementsList.Count; i++)
            subclassElements[i] = UIC_subclassElementsList[i];

        // subclassList
        var UIC_subclassListList = new List<VisualElement>();
        this.Query<VisualElement>(null, (string) null).ToList(UIC_subclassListList);
        if (UIC_subclassListList.Count == 0)
            Logger.LogError("Query (subclassList): No instances of VisualElement found", this);
        subclassList = UIC_subclassListList;

        // baseQueryComponent
        var UIC_baseQueryComponentList = new List<BaseQueryComponent>();
        this.Query<BaseQueryComponent>("fourth-uxml-name", "third-class-name").ToList(UIC_baseQueryComponentList);
        if (UIC_baseQueryComponentList.Count == 0)
            Logger.LogError("Query (baseQueryComponent): No instances of BaseQueryComponent found", this);
        if (UIC_baseQueryComponentList.Count > 0)
            baseQueryComponent = UIC_baseQueryComponentList[0];

        // baseElement
        var UIC_baseElementList = new List<VisualElement>();
        this.Query<VisualElement>(null, (string) null).ToList(UIC_baseElementList);
        if (UIC_baseElementList.Count == 0)
            Logger.LogError("Query (baseElement): No instances of VisualElement found", this);
        if (UIC_baseElementList.Count > 0)
            baseElement = UIC_baseElementList[0];

        // anotherBaseElement
        var UIC_anotherBaseElementList = new List<VisualElement>();
        this.Query<VisualElement>("uxml-name", (string) null).ToList(UIC_anotherBaseElementList);
        if (UIC_anotherBaseElementList.Count == 0)
            Logger.LogError("Query (anotherBaseElement): No instances of VisualElement found", this);
        if (UIC_anotherBaseElementList.Count > 0)
            anotherBaseElement = UIC_anotherBaseElementList[0];
    }
}
