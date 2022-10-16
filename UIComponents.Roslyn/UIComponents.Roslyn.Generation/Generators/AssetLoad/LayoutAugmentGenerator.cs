using Microsoft.CodeAnalysis;
using System.Text;

namespace UIComponents.Roslyn.Generation.Generators.AssetLoad
{
    [Generator]
    internal class LayoutAugmentGenerator : UIComponentAugmentGenerator
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

            if (!string.IsNullOrEmpty(CurrentAssetPathValue))
                layoutStringBuilder.Append(CurrentAssetPathValue);

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
            stringBuilder.Append("    ").AppendLine($@"protected override Task<VisualTreeAsset> StartLayoutLoad()
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
