﻿//HintName: BasicQueryComponent.Query.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using System.Collections.Generic;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class BasicQueryComponent
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "0.26.0")]
    protected override void UIC_PopulateQueryFields()
    {
        var UIC_elementList = new List<UnityEngine.UIElements.VisualElement>();
        this.Query<UnityEngine.UIElements.VisualElement>(null, (string) null).ToList(UIC_elementList);
        if (UIC_elementList.Count > 0)
             element = UIC_elementList[0];
        var UIC_elementWithUxmlNameInConstructorList = new List<UnityEngine.UIElements.VisualElement>();
        this.Query<UnityEngine.UIElements.VisualElement>("uxml-name", (string) null).ToList(UIC_elementWithUxmlNameInConstructorList);
        if (UIC_elementWithUxmlNameInConstructorList.Count > 0)
             elementWithUxmlNameInConstructor = UIC_elementWithUxmlNameInConstructorList[0];
        var UIC_elementWithUxmlNameAsNameArgumentList = new List<UnityEngine.UIElements.VisualElement>();
        this.Query<UnityEngine.UIElements.VisualElement>("second-uxml-name", (string) null).ToList(UIC_elementWithUxmlNameAsNameArgumentList);
        if (UIC_elementWithUxmlNameAsNameArgumentList.Count > 0)
             elementWithUxmlNameAsNameArgument = UIC_elementWithUxmlNameAsNameArgumentList[0];
        var UIC_elementWithClassNameList = new List<UnityEngine.UIElements.VisualElement>();
        this.Query<UnityEngine.UIElements.VisualElement>(null, "class-name").ToList(UIC_elementWithClassNameList);
        if (UIC_elementWithClassNameList.Count > 0)
             elementWithClassName = UIC_elementWithClassNameList[0];
        var UIC_elementWithNameAndClassList = new List<UnityEngine.UIElements.VisualElement>();
        this.Query<UnityEngine.UIElements.VisualElement>("third-uxml-name", "second-class-name").ToList(UIC_elementWithNameAndClassList);
        if (UIC_elementWithNameAndClassList.Count > 0)
             elementWithNameAndClass = UIC_elementWithNameAndClassList[0];
        var UIC_elementPropertyList = new List<UnityEngine.UIElements.VisualElement>();
        this.Query<UnityEngine.UIElements.VisualElement>("fourth-uxml-name", "third-class-name").ToList(UIC_elementPropertyList);
        if (UIC_elementPropertyList.Count > 0)
             elementProperty = UIC_elementPropertyList[0];
    }
}