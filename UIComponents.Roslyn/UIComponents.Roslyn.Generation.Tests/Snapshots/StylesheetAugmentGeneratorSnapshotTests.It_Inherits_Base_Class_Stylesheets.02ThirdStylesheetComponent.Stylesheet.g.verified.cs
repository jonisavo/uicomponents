﻿//HintName: ThirdStylesheetComponent.Stylesheet.g.cs
// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>

using System.Threading.Tasks;
using System.CodeDom.Compiler;
using UnityEngine.UIElements;

public partial class ThirdStylesheetComponent
{
    [GeneratedCode("UIComponents.Roslyn.Generation", "0.26.0")]
    private async Task<StyleSheetLoadTuple> UIC_GetSingleStyleSheet(string assetPath)
    {
        var styleSheet = await AssetResolver.LoadAsset<StyleSheet>(assetPath);
        return new StyleSheetLoadTuple(assetPath, styleSheet);
    }

    [GeneratedCode("UIComponents.Roslyn.Generation", "0.26.0")]
    protected override Task<StyleSheetLoadTuple>[] UIC_StartStyleSheetLoad()
    {
        var assetPaths = new string[] { "Components/BaseStylesheet", "Components/StylesheetOne", "Components/StylesheetTwo", "Components/StylesheetThree" };
        var styleSheetLoadTasks = new Task<StyleSheetLoadTuple>[assetPaths.Length];

        for (var i = 0; i < assetPaths.Length; i++)
            styleSheetLoadTasks[i] = UIC_GetSingleStyleSheet(assetPaths[i]);

        return styleSheetLoadTasks;
    }
}
