using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// Walks the type hierarchy current-first to find the most-derived
        /// [Layout] attribute. Returns the layout description using either
        /// the explicit path or the declaring type's class name as convention.
        /// Returns null if no [Layout] attribute is found.
        /// </summary>
        private LayoutDescription? FindLayoutDescription(AugmentGenerationContext context)
        {
            if (_layoutAttributeSymbol == null)
                return null;

            var typeSymbol = context.CurrentTypeSymbol;

            // Walk current-first, stop at UIComponent's base type
            while (typeSymbol != null &&
                   !SymbolEqualityComparer.Default.Equals(typeSymbol, UIComponentSymbol?.BaseType))
            {
                var layoutAttr = typeSymbol.GetAttributes().FirstOrDefault(a =>
                    SymbolEqualityComparer.Default.Equals(a.AttributeClass, _layoutAttributeSymbol));

                if (layoutAttr != null)
                {
                    var assetPath = layoutAttr.ConstructorArguments.Length > 0 &&
                        layoutAttr.ConstructorArguments[0].Value is string path
                            ? path
                            : typeSymbol.Name;

                    return new LayoutDescription(BuildPrefixedPath(assetPath));
                }

                typeSymbol = typeSymbol.BaseType;
            }

            return null;
        }

        protected override bool ShouldGenerateSource(AugmentGenerationContext context)
        {
            if (!base.ShouldGenerateSource(context))
                return false;

            var description = FindLayoutDescription(context);

            if (description == null || string.IsNullOrEmpty(description.Value.Path))
                return false;

            _layoutDescription = description.Value;
            return true;
        }

        protected override void AddAdditionalUsings(HashSet<string> usings)
        {
            base.AddAdditionalUsings(usings);
            usings.Add("System.Threading.Tasks");
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
