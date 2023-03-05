//HintName: BasicQueryComponent.Query.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using UnityEngine.UIElements;
using UIComponents;
using System.Collections.Generic;
using System.CodeDom.Compiler;

public partial class BasicQueryComponent
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.0")]
    protected override void UIC_PopulateQueryFields()
    {
        // element
        var UIC_elementList = new List<VisualElement>();
        this.Query<VisualElement>(null, (string) null).ToList(UIC_elementList);
        if (UIC_elementList.Count == 0)
            Logger.LogError("Query (element): No instances of VisualElement found", this);
        if (UIC_elementList.Count > 0)
            element = UIC_elementList[0];

        // elementWithUxmlNameInConstructor
        var UIC_elementWithUxmlNameInConstructorList = new List<VisualElement>();
        this.Query<VisualElement>("uxml-name", (string) null).ToList(UIC_elementWithUxmlNameInConstructorList);
        if (UIC_elementWithUxmlNameInConstructorList.Count == 0)
            Logger.LogError("Query (elementWithUxmlNameInConstructor): No instances of VisualElement found", this);
        if (UIC_elementWithUxmlNameInConstructorList.Count > 0)
            elementWithUxmlNameInConstructor = UIC_elementWithUxmlNameInConstructorList[0];

        // elementWithUxmlNameAsNameArgument
        var UIC_elementWithUxmlNameAsNameArgumentList = new List<VisualElement>();
        this.Query<VisualElement>("second-uxml-name", (string) null).ToList(UIC_elementWithUxmlNameAsNameArgumentList);
        if (UIC_elementWithUxmlNameAsNameArgumentList.Count == 0)
            Logger.LogError("Query (elementWithUxmlNameAsNameArgument): No instances of VisualElement found", this);
        if (UIC_elementWithUxmlNameAsNameArgumentList.Count > 0)
            elementWithUxmlNameAsNameArgument = UIC_elementWithUxmlNameAsNameArgumentList[0];

        // elementWithClassName
        var UIC_elementWithClassNameList = new List<VisualElement>();
        this.Query<VisualElement>(null, "class-name").ToList(UIC_elementWithClassNameList);
        if (UIC_elementWithClassNameList.Count == 0)
            Logger.LogError("Query (elementWithClassName): No instances of VisualElement found", this);
        if (UIC_elementWithClassNameList.Count > 0)
            elementWithClassName = UIC_elementWithClassNameList[0];

        // elementWithNameAndClass
        var UIC_elementWithNameAndClassList = new List<VisualElement>();
        this.Query<VisualElement>("third-uxml-name", "second-class-name").ToList(UIC_elementWithNameAndClassList);
        if (UIC_elementWithNameAndClassList.Count == 0)
            Logger.LogError("Query (elementWithNameAndClass): No instances of VisualElement found", this);
        if (UIC_elementWithNameAndClassList.Count > 0)
            elementWithNameAndClass = UIC_elementWithNameAndClassList[0];

        // elementProperty
        var UIC_elementPropertyList = new List<VisualElement>();
        this.Query<VisualElement>("fourth-uxml-name", "third-class-name").ToList(UIC_elementPropertyList);
        if (UIC_elementPropertyList.Count == 0)
            Logger.LogError("Query (elementProperty): No instances of VisualElement found", this);
        if (UIC_elementPropertyList.Count > 0)
            elementProperty = UIC_elementPropertyList[0];
    }
}
