using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using UIComponents.Roslyn.Common.Readers;
using UIComponents.Roslyn.Generation.SyntaxReceivers;
using UIComponents.Roslyn.Common.Utilities;

namespace UIComponents.Roslyn.Generation.Generators
{
    public abstract class UIComponentAugmentGenerator : AugmentGenerator<ClassSyntaxReceiver>
    {
        protected INamedTypeSymbol UIComponentSymbol;
        protected INamedTypeSymbol AssetPrefixAttributeSymbol;
        protected INamedTypeSymbol AssetRootAttributeSymbol;

        protected string CurrentAssetRoot;

        protected override void OnBeforeExecute(GeneratorExecutionContext context)
        {
            UIComponentSymbol = context.Compilation.GetTypeByMetadataName("UIComponents.UIComponent");
            AssetPrefixAttributeSymbol = context.Compilation.GetTypeByMetadataName("UIComponents.AssetPrefixAttribute");
            AssetRootAttributeSymbol = context.Compilation.GetTypeByMetadataName("UIComponents.AssetRootAttribute");
        }

        internal List<string> GetPathAttributeValues(
            INamedTypeSymbol attributeSymbol,
            AugmentGenerationContext context,
            AttributeReadOrder readerMode = AttributeReadOrder.CurrentFirst)
        {
            var values = new List<string>();

            var arguments = new Dictionary<AttributeData, Dictionary<string, TypedConstant>>();
            var classAttributeArgumentReader = new ClassAttributeArgumentReader(attributeSymbol, context.ClassSemanticModel);

            classAttributeArgumentReader.SetReadOrder(readerMode);
            classAttributeArgumentReader.Read(context.ClassSyntax, arguments);

            foreach (var attributeArgs in arguments.Values)
                if (attributeArgs.TryGetValue("constructor_0", out var pathArg))
                    values.Add(pathArg.Value as string);

            return values;
        }

        protected string GetPathAttributeValue(INamedTypeSymbol attributeSymbol, AugmentGenerationContext context)
        {
            var values = GetPathAttributeValues(attributeSymbol, context);

            if (values.Count == 0)
                return null;

            return values[0];
        }

        protected string BuildPrefixedPath(string value)
        {
            if (string.IsNullOrEmpty(CurrentAssetRoot))
                return value;

            return CurrentAssetRoot + value;
        }

        protected override bool ShouldGenerateSource(AugmentGenerationContext context)
        {
            if (UIComponentSymbol == null)
                return false;

            if (context.CurrentTypeSymbol.IsAbstract)
                return false;

            // Prefer AssetRoot, fall back to AssetPrefix
            CurrentAssetRoot = null;
            if (AssetRootAttributeSymbol != null)
                CurrentAssetRoot = GetPathAttributeValue(AssetRootAttributeSymbol, context);
            if (CurrentAssetRoot == null && AssetPrefixAttributeSymbol != null)
                CurrentAssetRoot = GetPathAttributeValue(AssetPrefixAttributeSymbol, context);

            return RoslynUtilities.HasBaseType(context.CurrentTypeSymbol, UIComponentSymbol);
        }
    }
}
