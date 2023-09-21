using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIComponents.Roslyn.Generation.SyntaxReceivers;
using UIComponents.Roslyn.Generation.Utilities;
using UIComponents.Roslyn.Common.Utilities;

namespace UIComponents.Roslyn.Generation.Generators.DependencyInjection
{
    [Generator]
    public sealed class ProvideAugmentGenerator : AugmentGenerator<ClassSyntaxReceiver>
    {
        private INamedTypeSymbol _provideAttributeSymbol;
        private bool _hasLogger = false;
        private bool _hasPopulateFieldsMethod = false;
        private readonly List<ProvideDescription> _provideDescriptions =
            new List<ProvideDescription>();

        private const string MissingProviderExceptionMessage =
            "$\"Could not provide {typeof(TField).Name} to {fieldName}\"";
        private const string InvalidCastExceptionMessage =
            "$\"Could not cast {typeof(TCastFrom).Name} to {typeof(TField).Name}\"";
        private const string PopulateMethodName = "UIC_PopulateProvideFields";

        protected override void OnBeforeExecute(GeneratorExecutionContext context)
        {
            _provideAttributeSymbol =
                context.Compilation.GetTypeByMetadataName("UIComponents.ProvideAttribute");
        }

        private void GetProvideDescriptions(IEnumerable<IFieldSymbol> fields, IList<ProvideDescription> output)
        {
            foreach (var field in fields)
            {
                var provideAttributes = field
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

                    output.Add(new ProvideDescription(field, castFromType));
                }
            }
        }

        private bool DoesFieldsContainLoggerField(IEnumerable<IFieldSymbol> fields)
        {
            foreach (var field in fields)
            {
                if (field.Name == "Logger")
                    return true;
            }

            return false;
        }

        protected override bool ShouldGenerateSource(AugmentGenerationContext context)
        {
            _provideDescriptions.Clear();
            _hasLogger = false;
            _hasPopulateFieldsMethod = false;

            if (_provideAttributeSymbol == null)
                return false;

            var members = RoslynUtilities.GetAllMembersOfType(context.CurrentTypeSymbol).ToList();

            _hasPopulateFieldsMethod = members.Any((member) =>
            {
                if (!(member is IMethodSymbol methodSymbol))
                    return false;

                return member.Name == PopulateMethodName;
            });

            var fields = members.Where((member) =>
            {
                if (!(member is IFieldSymbol fieldSymbol))
                    return false;

                var memberType = fieldSymbol.Type;

                var memberTypeIsClassOrInterface =
                    memberType.TypeKind == TypeKind.Class || memberType.TypeKind == TypeKind.Interface;

                return memberTypeIsClassOrInterface;
            }).Cast<IFieldSymbol>().ToList();

            GetProvideDescriptions(fields, _provideDescriptions);
            _hasLogger = DoesFieldsContainLoggerField(fields);

            return _provideDescriptions.Count > 0;
        }

        protected override void AddAdditionalUsings(HashSet<string> usings)
        {
            usings.Add("System");
            usings.Add("UIComponents");

            if (!_hasLogger)
                usings.Add("UnityEngine");

            base.AddAdditionalUsings(usings);
        }

        private string CreateExceptionMessage(string message)
        {
            if (_hasLogger)
                return $"Logger.LogError({message}, this);";
            else
                return $"Debug.LogError({message});";
        }

        protected override void GenerateSource(AugmentGenerationContext context, StringBuilder stringBuilder)
        {
            stringBuilder
                .AppendPadding()
                .AppendCodeGeneratedAttribute()
                .AppendLineWithPadding($@"private void UIC_SetProvideField<TField, TCastFrom>(ref TField value, string fieldName) where TField : class where TCastFrom : class
    {{
        try
        {{
            value = (TField) (object) Provide<TCastFrom>();
        }}
        catch (MissingProviderException)
        {{
            {CreateExceptionMessage(MissingProviderExceptionMessage)}
        }}
        catch (InvalidCastException)
        {{
            {CreateExceptionMessage(InvalidCastExceptionMessage)}
        }}
    }}
");
            var keyword = _hasPopulateFieldsMethod ? " override " : " ";

            stringBuilder
                .AppendPadding()
                .AppendCodeGeneratedAttribute()
                .AppendLineWithPadding($@"protected{keyword}void {PopulateMethodName}()
    {{");

            foreach (var provideDescription in _provideDescriptions)
            {
                var typeName = context.GetTypeName(provideDescription.Field.Type);
                stringBuilder
                    .AppendWithPadding("UIC_SetProvideField<", 2)
                    .Append(typeName)
                    .Append(", ")
                    .Append(provideDescription.GetCastFromTypeName(context))
                    .Append(">(ref ")
                    .Append(provideDescription.Field.Name)
                    .Append(", ")
                    .Append(StringUtilities.AddQuotesToString(provideDescription.Field.Name))
                    .AppendLine(");");
            }

            stringBuilder.AppendLineWithPadding("}");
        }

        protected override string GetHintPostfix()
        {
            return "Provide";
        }
    }
}
