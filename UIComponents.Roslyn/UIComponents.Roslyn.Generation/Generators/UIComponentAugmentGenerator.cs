﻿using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using UIComponents.Roslyn.Generation.Readers;
using UIComponents.Roslyn.Generation.SyntaxReceivers;
using UIComponents.Roslyn.Generation.Utilities;

namespace UIComponents.Roslyn.Generation.Generators
{
    internal abstract class UIComponentAugmentGenerator : AugmentGenerator<ClassSyntaxReceiver>
    {
        protected INamedTypeSymbol UIComponentSymbol;
        protected INamedTypeSymbol AssetPrefixAttributeSymbol;

        protected string CurrentAssetPrefix;

        protected override void OnBeforeExecute(GeneratorExecutionContext context)
        {
            UIComponentSymbol = context.Compilation.GetTypeByMetadataName("UIComponents.UIComponent");
            AssetPrefixAttributeSymbol = context.Compilation.GetTypeByMetadataName("UIComponents.AssetPrefixAttribute");
        }

        protected List<string> GetPathAttributeValues(
            INamedTypeSymbol attributeSymbol,
            AugmentGenerationContext context,
            ClassAttributeArgumentReaderMode readerMode = ClassAttributeArgumentReaderMode.CurrentFirst)
        {
            var values = new List<string>();

            var arguments = new Dictionary<AttributeData, Dictionary<string, TypedConstant>>();
            var classAttributeArgumentReader = new ClassAttributeArgumentReader(attributeSymbol, context.ClassSemanticModel);

            classAttributeArgumentReader.SetMode(readerMode);
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

        protected override bool ShouldGenerateSource(AugmentGenerationContext context)
        {
            if (UIComponentSymbol == null || AssetPrefixAttributeSymbol == null)
                return false;

            CurrentAssetPrefix = GetPathAttributeValue(AssetPrefixAttributeSymbol, context);

            return RoslynUtilities.HasBaseType(context.CurrentTypeSymbol, UIComponentSymbol);
        }
    }
}
