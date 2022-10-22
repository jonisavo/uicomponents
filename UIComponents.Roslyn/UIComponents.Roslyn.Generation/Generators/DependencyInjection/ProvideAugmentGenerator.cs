using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIComponents.Roslyn.Generation.SyntaxReceivers;
using UIComponents.Roslyn.Generation.Utilities;

namespace UIComponents.Roslyn.Generation.Generators.DependencyInjection
{
    [Generator]
    internal class ProvideAugmentGenerator : AugmentGenerator<ClassSyntaxReceiver>
    {
        private INamedTypeSymbol _provideAttributeSymbol;
        private readonly List<ProvideDescription> _provideDescriptions =
            new List<ProvideDescription>();

        protected override void OnBeforeExecute(GeneratorExecutionContext context)
        {
            _provideAttributeSymbol =
                context.Compilation.GetTypeByMetadataName("UIComponents.ProvideAttribute");
        }

        private void GetProvideDescriptions(AugmentGenerationContext context, IList<ProvideDescription> output)
        {
            var members = RoslynUtilities.GetAllMembersOfType(context.CurrentTypeSymbol);

            foreach (var member in members)
            {
                if (!(member is IFieldSymbol fieldSymbol))
                    continue;

                var memberType = RoslynUtilities.GetMemberType(member) as INamedTypeSymbol;

                if (memberType == null)
                    continue;

                var memberTypeIsClassOrInterface =
                    memberType.TypeKind == TypeKind.Class || memberType.TypeKind == TypeKind.Interface;

                if (!memberTypeIsClassOrInterface)
                    continue;

                var provideAttributes = fieldSymbol
                    .GetAttributes()
                    .Where((attribute) => attribute.AttributeClass.Equals(_provideAttributeSymbol, SymbolEqualityComparer.Default))
                    .ToList();

                if (provideAttributes.Count == 0)
                    continue;

                var argumentsDictionary = new Dictionary<AttributeData, Dictionary<string, TypedConstant>>();

                foreach (var attribute in provideAttributes)
                {
                    var attributeArgs = new Dictionary<string, TypedConstant>();
                    argumentsDictionary.Add(attribute, attributeArgs);
                    RoslynUtilities.ReadAttributeArguments(attribute, attributeArgs);
                }

                foreach (var arguments in argumentsDictionary.Values)
                {
                    INamedTypeSymbol castFromType = null;

                    if (arguments.TryGetValue("CastFrom", out var castFromArg))
                        castFromType = castFromArg.Value as INamedTypeSymbol;

                    output.Add(new ProvideDescription(fieldSymbol, castFromType));
                }
            }
        }

        protected override bool ShouldGenerateSource(AugmentGenerationContext context)
        {
            _provideDescriptions.Clear();
            GetProvideDescriptions(context, _provideDescriptions);

            return _provideAttributeSymbol != null && _provideDescriptions.Count > 0;
        }

        protected override void BuildUsingStatements(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("using System;");
            stringBuilder.AppendLine("using UIComponents;");
            base.BuildUsingStatements(stringBuilder);
        }

        protected override void GenerateSource(AugmentGenerationContext context, StringBuilder stringBuilder)
        {
            stringBuilder
                .Append("    ")
                .Append(Constants.GeneratedCodeAttribute)
                .AppendLine(@"
    private void UIC_SetProvideField<TField, TCastFrom>(ref TField value, string fieldName) where TField : class where TCastFrom : class
    {
        try
        {
            value = (TField) (object) Provide<TCastFrom>();
        }
        catch (MissingProviderException)
        {
            Logger.LogError($""Could not provide {typeof(TField).Name} to {fieldName}"", this);
        }
        catch (InvalidCastException)
        {
            Logger.LogError($""Could not cast {typeof(TCastFrom).Name} to {typeof(TField).Name}"", this);
        }
    }
");

            stringBuilder
                .Append("    ")
                .Append(Constants.GeneratedCodeAttribute)
                .AppendLine(@"
    protected override void UIC_PopulateProvideFields()
    {");

            foreach (var provideDescription in _provideDescriptions)
            {
                stringBuilder
                    .Append("        UIC_SetProvideField<")
                    .Append(provideDescription.Field.Type.ToDisplayString())
                    .Append(", ")
                    .Append(provideDescription.GetCastFromTypeName())
                    .Append(">(ref ")
                    .Append(provideDescription.Field.Name)
                    .Append(", ")
                    .Append(StringUtilities.AddQuotesToString(provideDescription.Field.Name))
                    .AppendLine(");");
            }

            stringBuilder.AppendLine("    }");
        }

        protected override string GetHintPostfix()
        {
            return "Provide";
        }
    }
}
