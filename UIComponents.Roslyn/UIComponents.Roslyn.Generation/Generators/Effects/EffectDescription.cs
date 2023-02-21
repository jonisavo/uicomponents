using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;

namespace UIComponents.Roslyn.Generation.Generators.Effects
{
    internal readonly struct EffectDescription
    {
        public readonly ITypeSymbol EffectTypeSymbol;

        public readonly ImmutableArray<TypedConstant> ConstructorArguments;

        private readonly ImmutableArray<KeyValuePair<string, TypedConstant>> NamedArguments;

        public readonly string CallString;

        public EffectDescription(
            ITypeSymbol effectTypeSymbol,
            ImmutableArray<TypedConstant> constructorArguments,
            ImmutableArray<KeyValuePair<string, TypedConstant>> namedArguments)
        {
            EffectTypeSymbol = effectTypeSymbol;
            ConstructorArguments = constructorArguments;
            NamedArguments = namedArguments;
            CallString = null;
            CallString = BuildCallString();
        }

        private string BuildCallString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append('(');

            for (var i = 0; i < ConstructorArguments.Length; i++)
            {
                stringBuilder.Append(ConstructorArguments[i].ToCSharpString());
                if (i != ConstructorArguments.Length - 1)
                    stringBuilder.Append(", ");
            }

            stringBuilder.Append(')');

            if (NamedArguments.Length > 0)
            {
                stringBuilder.Append(" { ");
                for (var i = 0; i < NamedArguments.Length; i++)
                {
                    var argument = NamedArguments[i];
                    stringBuilder.Append(argument.Key).Append(" = ").Append(argument.Value.ToCSharpString());
                    if (i != NamedArguments.Length - 1)
                        stringBuilder.Append(", ");
                }
                stringBuilder.Append(" }");
            }



            return stringBuilder.ToString();
        }

        public static EffectDescription FromAttributeData(AttributeData attributeData)
        {
            return new EffectDescription(attributeData.AttributeClass, attributeData.ConstructorArguments, attributeData.NamedArguments);
        }
    }
}
