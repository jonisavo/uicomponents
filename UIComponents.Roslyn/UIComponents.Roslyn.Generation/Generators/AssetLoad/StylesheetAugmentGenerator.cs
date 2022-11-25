using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIComponents.Roslyn.Generation.Readers;
using UIComponents.Roslyn.Generation.Utilities;

namespace UIComponents.Roslyn.Generation.Generators.AssetLoad
{
    [Generator]
    public sealed class StylesheetAugmentGenerator : UIComponentAugmentGenerator
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

            if (!string.IsNullOrEmpty(CurrentAssetPrefix))
                stylesheetStringBuilder.Append(CurrentAssetPrefix);

            if (!string.IsNullOrEmpty(stylesheetPath))
                stylesheetStringBuilder.Append(stylesheetPath);

            return new StylesheetDescription(stylesheetStringBuilder.ToString());
        }

        private void GetStylesheetDescriptions(AugmentGenerationContext context, IList<StylesheetDescription> stylesheets)
        {
            var stylesheetPaths = GetPathAttributeValues(_stylesheetAttributeSymbol, context, AttributeReadOrder.BaseFirst);

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

        protected override void AddAdditionalUsings(HashSet<string> usings)
        {
            base.AddAdditionalUsings(usings);
            usings.Add("System.Threading.Tasks");
        }

        protected override void GenerateSource(AugmentGenerationContext context, StringBuilder stringBuilder)
        {
            stringBuilder.AppendLineWithPadding($@"{Constants.GeneratedCodeAttribute}
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
