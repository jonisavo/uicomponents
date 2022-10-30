using Microsoft.CodeAnalysis;
using System.Text;
using UIComponents.Roslyn.Generation.Utilities;

namespace UIComponents.Roslyn.Generation.Generators.AssetLoad
{
    [Generator]
    public sealed class LayoutAugmentGenerator : UIComponentAugmentGenerator
    {
        private INamedTypeSymbol _layoutAttributeSymbol;
        private LayoutDescription _layoutDescription;

        protected override void OnBeforeExecute(GeneratorExecutionContext context)
        {
            base.OnBeforeExecute(context);
            _layoutAttributeSymbol = context.Compilation.GetTypeByMetadataName("UIComponents.LayoutAttribute");
        }

        private LayoutDescription GetLayoutDescription(AugmentGenerationContext context)
        {
            var layoutStringBuilder = new StringBuilder();

            if (!string.IsNullOrEmpty(CurrentAssetPrefix))
                layoutStringBuilder.Append(CurrentAssetPrefix);

            var layoutPath = GetPathAttributeValue(_layoutAttributeSymbol, context);

            if (!string.IsNullOrEmpty(layoutPath))
                layoutStringBuilder.Append(layoutPath);

            return new LayoutDescription(layoutStringBuilder.ToString());
        }

        protected override bool ShouldGenerateSource(AugmentGenerationContext context)
        {
            if (!base.ShouldGenerateSource(context))
                return false;

            _layoutDescription = GetLayoutDescription(context);

            return !string.IsNullOrEmpty(_layoutDescription.Path);
        }

        protected override void BuildUsingStatements(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("using System.Threading.Tasks;");
            base.BuildUsingStatements(stringBuilder);
        }

        protected override void GenerateSource(AugmentGenerationContext context, StringBuilder stringBuilder)
        {
            stringBuilder
                .AppendPadding()
                .AppendCodeGeneratedAttribute()
                .AppendLineWithPadding($@"protected override Task<VisualTreeAsset> UIC_StartLayoutLoad()
    {{
        return AssetResolver.LoadAsset<VisualTreeAsset>(""{_layoutDescription.Path}"");
    }}");
        }

        protected override string GetHintPostfix()
        {
            return "Layout";
        }
    }
}
