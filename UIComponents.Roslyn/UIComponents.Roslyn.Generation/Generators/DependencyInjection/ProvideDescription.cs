using Microsoft.CodeAnalysis;
using UIComponents.Roslyn.Generation.Utilities;

namespace UIComponents.Roslyn.Generation.Generators.DependencyInjection
{
    internal readonly struct ProvideDescription
    {
        public readonly IFieldSymbol Field;
        public readonly INamedTypeSymbol CastFromType;

        public ProvideDescription(IFieldSymbol field, INamedTypeSymbol castFromType)
        {
            Field = field;
            CastFromType = castFromType;
        }

        public string GetCastFromTypeName(AugmentGenerationContext context)
        {
            if (CastFromType != null)
                return context.GetTypeName(CastFromType);

            return context.GetTypeName(Field.Type);
        }
    }
}
