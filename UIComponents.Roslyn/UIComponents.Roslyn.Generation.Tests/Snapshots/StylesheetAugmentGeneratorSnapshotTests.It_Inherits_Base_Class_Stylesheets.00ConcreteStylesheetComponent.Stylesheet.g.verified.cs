﻿//HintName: ConcreteStylesheetComponent.Stylesheet.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using UIComponents;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;
using System.Threading.Tasks;

public partial class ConcreteStylesheetComponent
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.6")]
    private async Task<StyleSheetLoadTuple> UIC_GetSingleStyleSheet(string assetPath)
    {
        var styleSheet = await AssetResolver.LoadAsset<StyleSheet>(assetPath);
        return new StyleSheetLoadTuple(assetPath, styleSheet);
    }

    [GeneratedCode("UIComponents.Roslyn.Generation", "1.0.0-alpha.6")]
    protected override Task<StyleSheetLoadTuple>[] UIC_StartStyleSheetLoad()
    {
        var assetPaths = new string[] { "Components/BaseStylesheet", "Components/StylesheetOne", "Components/StylesheetTwo" };
        var styleSheetLoadTasks = new Task<StyleSheetLoadTuple>[assetPaths.Length];

        for (var i = 0; i < assetPaths.Length; i++)
            styleSheetLoadTasks[i] = UIC_GetSingleStyleSheet(assetPaths[i]);

        return styleSheetLoadTasks;
    }
}
