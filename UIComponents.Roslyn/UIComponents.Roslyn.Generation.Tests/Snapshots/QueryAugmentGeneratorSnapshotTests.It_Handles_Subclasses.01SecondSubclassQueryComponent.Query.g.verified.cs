﻿//HintName: SecondSubclassQueryComponent.Query.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using System.Collections.Generic;
using UnityEngine.UIElements;

public partial class SecondSubclassQueryComponent
{
    protected override void PopulateQueryFields()
    {
        subclassQueryComponent = this.Query<SubclassQueryComponent>(null, "asdf").First();
        var subclassElementsList = new List<UnityEngine.UIElements.VisualElement>();
        this.Query<UnityEngine.UIElements.VisualElement>("foo", "bar").ToList(subclassElementsList);
        subclassElements = new UnityEngine.UIElements.VisualElement[subclassElementsList.Count];
        for (var i = 0; i < subclassElementsList.Count; i++)
            subclassElements[i] = subclassElementsList[i];
        subclassList = this.Query<UnityEngine.UIElements.VisualElement>(null, (string) null).ToList();
        baseQueryComponent = this.Query<BaseQueryComponent>("fourth-uxml-name", "third-class-name").First();
        baseElement = this.Query<UnityEngine.UIElements.VisualElement>(null, (string) null).First();
        anotherBaseElement = this.Query<UnityEngine.UIElements.VisualElement>("uxml-name", (string) null).First();
    }
}
