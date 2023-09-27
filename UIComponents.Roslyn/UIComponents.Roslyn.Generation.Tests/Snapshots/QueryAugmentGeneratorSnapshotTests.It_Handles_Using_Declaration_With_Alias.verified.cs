﻿//HintName: UsingAliasComponent.Query.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using System.Collections.Generic;
using UnityEngine.UIElements;
using UIComponents;
using System.CodeDom.Compiler;

public partial class UsingAliasComponent
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-beta.8")]
    protected override void UIC_PopulateQueryFields()
    {
        // firstButton
        var UIC_firstButtonList = new List<MyLibrary.Button>();
        this.Query<MyLibrary.Button>(null, (string) null).ToList(UIC_firstButtonList);
        this.Query<MyLibrary.Button>(null, "test").ToList(UIC_firstButtonList);
        if (UIC_firstButtonList.Count == 0)
            Logger.LogError("Query (firstButton): No instances of MyLibrary.Button found", this);
        if (UIC_firstButtonList.Count > 0)
            firstButton = UIC_firstButtonList[0];

        // buttonArray
        var UIC_buttonArrayList = new List<MyLibrary.Button>();
        this.Query<MyLibrary.Button>("uxml-name", "class").ToList(UIC_buttonArrayList);
        this.Query<MyLibrary.Button>(null, "class-name").ToList(UIC_buttonArrayList);
        if (UIC_buttonArrayList.Count == 0)
            Logger.LogError("Query (buttonArray): No instances of MyLibrary.Button found", this);
        buttonArray = new MyLibrary.Button[UIC_buttonArrayList.Count];
        for (var i = 0; i < UIC_buttonArrayList.Count; i++)
            buttonArray[i] = UIC_buttonArrayList[i];

        // buttonList
        var UIC_buttonListList = new List<MyLibrary.Button>();
        this.Query<MyLibrary.Button>("name", "class").ToList(UIC_buttonListList);
        this.Query<MyLibrary.Button>("other-name", "other-class").ToList(UIC_buttonListList);
        this.Query<MyLibrary.Button>("third-name", "third-class").ToList(UIC_buttonListList);
        if (UIC_buttonListList.Count == 0)
            Logger.LogError("Query (buttonList): No instances of MyLibrary.Button found", this);
        buttonList = UIC_buttonListList;
    }
}
