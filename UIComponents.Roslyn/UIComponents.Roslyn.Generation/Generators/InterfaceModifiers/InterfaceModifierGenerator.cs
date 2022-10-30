using System.Linq;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using UIComponents.Roslyn.Generation.SyntaxReceivers;
using UIComponents.Roslyn.Generation.Utilities;
using UIComponents.Roslyn.Generation.Readers;

namespace UIComponents.Roslyn.Generation.Generators.InterfaceModifiers
{
    public abstract class InterfaceModifierGenerator<T> : AugmentGenerator<ClassSyntaxReceiver>
    {
        protected INamedTypeSymbol InterfaceModifierAttributeSymbol;
        protected readonly List<T> ModifierDescriptions = new List<T>();

        protected abstract string GetAttributeName();

        protected readonly struct InterfaceAttributeInfo
        {
            public readonly INamedTypeSymbol InterfaceType;
            public readonly Dictionary<AttributeData, Dictionary<string, TypedConstant>> Attributes;

            public InterfaceAttributeInfo(INamedTypeSymbol interfaceType, Dictionary<AttributeData, Dictionary<string, TypedConstant>> attributes)
            {
                InterfaceType = interfaceType;
                Attributes = attributes;
            }
        }

        protected abstract void GetModifierDescriptions(InterfaceAttributeInfo attributeInfo, List<T> output);

        protected override void OnBeforeExecute(GeneratorExecutionContext context)
        {
            InterfaceModifierAttributeSymbol = context.Compilation.GetTypeByMetadataName(GetAttributeName());
        }

        protected override bool ShouldGenerateSource(AugmentGenerationContext context)
        {
            if (InterfaceModifierAttributeSymbol == null)
                return false;

            if (context.CurrentTypeSymbol.IsAbstract)
                return false;

            ModifierDescriptions.Clear();

            var interfaces = context.CurrentTypeSymbol.AllInterfaces
                .Where((interfaceType) => interfaceType.GetAttributes().Any((attribute) => RoslynUtilities.HasBaseType(attribute.AttributeClass, InterfaceModifierAttributeSymbol)));

            var classAttributeArgumentReader =
                new ClassAttributeArgumentReader(InterfaceModifierAttributeSymbol, context.ClassSemanticModel);

            foreach (var usedInterface in interfaces)
            {
                var attributes = new Dictionary<AttributeData, Dictionary<string, TypedConstant>>();
                classAttributeArgumentReader.ReadWithSymbol(usedInterface, attributes);
                var attributeInfo = new InterfaceAttributeInfo(usedInterface, attributes);
                GetModifierDescriptions(attributeInfo, ModifierDescriptions);
            }

            return ModifierDescriptions.Count > 0;
        }

        protected override string GetHintPostfix()
        {
            return InterfaceModifierAttributeSymbol.Name;
        }
    }
}

