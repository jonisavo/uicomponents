﻿using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIComponents.Roslyn.Generation.Readers;

namespace UIComponents.Roslyn.Generation.Generators.AssetLoad
{
    [Generator]
    internal class StylesheetAugmentGenerator : UIComponentAugmentGenerator
    {
        private INamedTypeSymbol _stylesheetAttributeSymbol;
        private readonly List<StylesheetDescription> _stylesheetDescriptions
            = new List<StylesheetDescription>();

        protected override void OnBeforeExecute(GeneratorExecutionContext context)
        {
            base.OnBeforeExecute(context);
            _stylesheetAttributeSymbol = context.Compilation.GetTypeByMetadataName("UIComponents.StylesheetAttribute");
        }

        private StylesheetDescription GetStylesheetDescription(string stylesheetPath)
        {
            var stylesheetStringBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(CurrentAssetPathValue))
                stylesheetStringBuilder.Append(CurrentAssetPathValue);

            if (!string.IsNullOrEmpty(stylesheetPath))
                stylesheetStringBuilder.Append(stylesheetPath);

            return new StylesheetDescription(stylesheetStringBuilder.ToString());
        }

        private void GetStylesheetDescriptions(AugmentGenerationContext context, IList<StylesheetDescription> stylesheets)
        {
            var stylesheetPaths = GetPathAttributeValues(_stylesheetAttributeSymbol, context, ClassAttributeArgumentReaderMode.BaseFirst);

            foreach (var path in stylesheetPaths)
                stylesheets.Add(GetStylesheetDescription(path));
        }

        protected override bool ShouldGenerateSource(AugmentGenerationContext context)
        {
            if (!base.ShouldGenerateSource(context))
                return false;

            _stylesheetDescriptions.Clear();
            GetStylesheetDescriptions(context, _stylesheetDescriptions);

            return _stylesheetDescriptions.Count > 0;
        }

        protected override void BuildUsingStatements(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("using System.Threading.Tasks;");
            base.BuildUsingStatements(stringBuilder);
        }

        protected override void GenerateSource(AugmentGenerationContext context, StringBuilder stringBuilder)
        {
            stringBuilder.Append("    ").AppendLine($@"{Constants.GeneratedCodeAttribute}
    private async Task<StyleSheetLoadTuple> UIC_GetSingleStyleSheet(string assetPath)
    {{
        var styleSheet = await AssetResolver.LoadAsset<StyleSheet>(assetPath);
        return new StyleSheetLoadTuple(assetPath, styleSheet);
    }}

    {Constants.GeneratedCodeAttribute}
    protected override Task<StyleSheetLoadTuple>[] UIC_StartStyleSheetLoad()
    {{
        var assetPaths = new string[] {{ {string.Join(", ", _stylesheetDescriptions.Select(desc => $"\"{desc.Path}\""))} }};
        var styleSheetLoadTasks = new Task<StyleSheetLoadTuple>[assetPaths.Length];

        for (var i = 0; i < assetPaths.Length; i++)
            styleSheetLoadTasks[i] = UIC_GetSingleStyleSheet(assetPaths[i]);

        return styleSheetLoadTasks;
    }}");
        }

        protected override string GetHintPostfix()
        {
            return "Stylesheet";
        }
    }
}
