using Microsoft.CodeAnalysis;
using System;

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

        public string GetCastFromTypeName()
        {
            if (CastFromType != null)
                return CastFromType.ToDisplayString();

            return Field.Type.ToDisplayString();
        }
    }
}
