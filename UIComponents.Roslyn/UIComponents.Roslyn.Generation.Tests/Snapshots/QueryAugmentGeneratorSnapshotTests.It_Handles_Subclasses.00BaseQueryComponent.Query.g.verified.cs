﻿//HintName: BaseQueryComponent.Query.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using System.Collections.Generic;
using UnityEngine.UIElements;

public partial class BaseQueryComponent
{
    protected override void PopulateQueryFields()
    {
        baseElement = this.Query<UnityEngine.UIElements.VisualElement>(null, (string) null).First();
        anotherBaseElement = this.Query<UnityEngine.UIElements.VisualElement>("uxml-name", (string) null).First();
    }
}
