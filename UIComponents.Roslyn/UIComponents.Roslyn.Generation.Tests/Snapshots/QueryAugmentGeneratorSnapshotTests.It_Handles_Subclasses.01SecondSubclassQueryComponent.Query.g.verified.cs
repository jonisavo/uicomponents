﻿//HintName: SecondSubclassQueryComponent.Query.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using System.Collections.Generic;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class SecondSubclassQueryComponent
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "0.26.0")]
    protected override void UIC_PopulateQueryFields()
    {
        var UIC_subclassQueryComponentList = new List<SubclassQueryComponent>();
        this.Query<SubclassQueryComponent>(null, "asdf").ToList(UIC_subclassQueryComponentList);
        if (UIC_subclassQueryComponentList.Count > 0)
             subclassQueryComponent = UIC_subclassQueryComponentList[0];
        var UIC_subclassElementsList = new List<UnityEngine.UIElements.VisualElement>();
        this.Query<UnityEngine.UIElements.VisualElement>("foo", "bar").ToList(UIC_subclassElementsList);
        subclassElements = new UnityEngine.UIElements.VisualElement[UIC_subclassElementsList.Count];
        for (var i = 0; i < UIC_subclassElementsList.Count; i++)
            subclassElements[i] = UIC_subclassElementsList[i];
        var UIC_subclassListList = new List<UnityEngine.UIElements.VisualElement>();
        this.Query<UnityEngine.UIElements.VisualElement>(null, (string) null).ToList(UIC_subclassListList);
        subclassList = UIC_subclassListList;
        var UIC_baseQueryComponentList = new List<BaseQueryComponent>();
        this.Query<BaseQueryComponent>("fourth-uxml-name", "third-class-name").ToList(UIC_baseQueryComponentList);
        if (UIC_baseQueryComponentList.Count > 0)
             baseQueryComponent = UIC_baseQueryComponentList[0];
        var UIC_baseElementList = new List<UnityEngine.UIElements.VisualElement>();
        this.Query<UnityEngine.UIElements.VisualElement>(null, (string) null).ToList(UIC_baseElementList);
        if (UIC_baseElementList.Count > 0)
             baseElement = UIC_baseElementList[0];
        var UIC_anotherBaseElementList = new List<UnityEngine.UIElements.VisualElement>();
        this.Query<UnityEngine.UIElements.VisualElement>("uxml-name", (string) null).ToList(UIC_anotherBaseElementList);
        if (UIC_anotherBaseElementList.Count > 0)
             anotherBaseElement = UIC_anotherBaseElementList[0];
    }
}
