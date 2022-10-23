using Microsoft.CodeAnalysis;
using System;
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

        public string GetCastFromTypeName(string nameSpace)
        {
            if (CastFromType != null)
                return RoslynUtilities.GetTypeNameWithoutRootNamespace(CastFromType, nameSpace);

            return RoslynUtilities.GetTypeNameWithoutRootNamespace(Field.Type, nameSpace);
        }
    }
}
