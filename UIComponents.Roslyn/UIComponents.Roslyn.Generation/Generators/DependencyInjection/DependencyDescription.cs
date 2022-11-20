using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace UIComponents.Roslyn.Generation.Generators.DependencyInjection
{
	public readonly struct DependencyDescription : IEquatable<DependencyDescription>
	{
		public readonly INamedTypeSymbol DependencyType;
		public readonly INamedTypeSymbol ImplementationType;
		public readonly int ScopeAsInt;

		private const int SingletonScope = 0;

		public DependencyDescription(
			INamedTypeSymbol dependencyType,
			INamedTypeSymbol implementationType,
			int scopeAsInt)
		{
			DependencyType = dependencyType;
			ImplementationType = implementationType;
			ScopeAsInt = scopeAsInt;
		}

		public string ToConstructorCallString()
		{
			var stringBuilder = new StringBuilder();
			stringBuilder.Append("UIComponents.DependencyInjection.Dependency.");

			if (ScopeAsInt == SingletonScope)
				stringBuilder.Append("SingletonFor<");
			else
				stringBuilder.Append("TransientFor<");

			stringBuilder
				.Append(DependencyType.ToDisplayString())
				.Append(", ")
				.Append(ImplementationType.ToDisplayString())
				.Append(">()");

			return stringBuilder.ToString();
		}

		public static DependencyDescription FromAttributeArgs(Dictionary<string, TypedConstant> attributeArguments)
		{
			INamedTypeSymbol dependencyType = null;
			INamedTypeSymbol implementationType = null;
			int scopeAsInt = 0;

			if (attributeArguments.TryGetValue("constructor_0", out var dependencyTypeArg))
				dependencyType = dependencyTypeArg.Value as INamedTypeSymbol;

			if (attributeArguments.TryGetValue("constructor_1", out var implementationTypeArg))
				implementationType = implementationTypeArg.Value as INamedTypeSymbol;

			if (attributeArguments.TryGetValue("constructor_2", out var scopeArg))
				scopeAsInt = (int) scopeArg.Value;

			return new DependencyDescription(dependencyType, implementationType, scopeAsInt);
        }

        public bool Equals(DependencyDescription other)
        {
			var dependencyTypesAreEqual =
                DependencyType.Equals(other.DependencyType, SymbolEqualityComparer.Default);

			return dependencyTypesAreEqual && ScopeAsInt == other.ScopeAsInt;
        }
    }
}

