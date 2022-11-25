using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIComponents.Roslyn.Generation.Utilities;

namespace UIComponents.Roslyn.Generation.Generators.Uxml
{
    [Generator]
    public sealed class QueryAugmentGenerator : UIComponentAugmentGenerator
    {
        private INamedTypeSymbol _queryAttributeSymbol;
        private INamedTypeSymbol _visualElementSymbol;
        private readonly List<QueryDescription> _queryDescriptions =
            new List<QueryDescription>();

        protected override void OnBeforeExecute(GeneratorExecutionContext context)
        {
            base.OnBeforeExecute(context);

            _queryAttributeSymbol = context.Compilation.GetTypeByMetadataName("UIComponents.QueryAttribute");
            _visualElementSymbol = context.Compilation.GetTypeByMetadataName("UnityEngine.UIElements.VisualElement");
        }

        private void GetQueryDescriptions(AugmentGenerationContext context, IList<QueryDescription> queryDescriptions)
        {
            var members = RoslynUtilities.GetAllMembersOfType(context.CurrentTypeSymbol);

            foreach (var member in members)
            {
                if (member.Kind != SymbolKind.Field && member.Kind != SymbolKind.Property)
                    continue;

                if (member is IPropertySymbol propertySymbol && propertySymbol.SetMethod == null)
                    continue;

                var memberType = RoslynUtilities.GetMemberType(member);
                var concreteType = RoslynUtilities.GetConcreteType(memberType) as INamedTypeSymbol;

                if (!RoslynUtilities.HasBaseType(concreteType, _visualElementSymbol))
                    continue;

                var queryAttributes = member
                    .GetAttributes()
                    .Where((attribute) => attribute.AttributeClass.Equals(_queryAttributeSymbol, SymbolEqualityComparer.Default))
                    .ToList();

                if (queryAttributes.Count == 0)
                    continue;

                var argumentsDictionary = new Dictionary<AttributeData, Dictionary<string, TypedConstant>>();

                foreach (var attribute in queryAttributes)
                {
                    var attributeArgs = new Dictionary<string, TypedConstant>();
                    argumentsDictionary.Add(attribute, attributeArgs);
                    RoslynUtilities.ReadAttributeArguments(attribute, attributeArgs);
                }

                queryDescriptions.Add(QueryDescription.CreateFromMember(member, argumentsDictionary));
            }
        }

        protected override bool ShouldGenerateSource(AugmentGenerationContext context)
        {
            if (!base.ShouldGenerateSource(context))
                return false;

            if (_queryAttributeSymbol == null)
                return false;

            _queryDescriptions.Clear();
            GetQueryDescriptions(context, _queryDescriptions);

            return _queryDescriptions.Count > 0;
        }

        private void GenerateQueryAssignment(
            QueryDescription queryDescription, 
            AugmentGenerationContext context, 
            StringBuilder stringBuilder)
        {
            var memberSymbol = queryDescription.MemberSymbol;
            var memberType = RoslynUtilities.GetMemberType(memberSymbol);

            var concreteMemberType = RoslynUtilities.GetConcreteType(memberType);
            var concreteMemberTypeName = context.GetTypeName(concreteMemberType);

            stringBuilder.AppendLineWithPadding($"// {memberSymbol.Name}", 2);
            
            var listVariableName = $"UIC_{memberSymbol.Name}List";
            stringBuilder
                .AppendLineWithPadding($"var {listVariableName} = new List<{concreteMemberTypeName}>();", 2);

            foreach (var queryCall in queryDescription.QueryCalls)
            {
                var queryCallString = $"this.Query<{concreteMemberTypeName}>({queryCall.UxmlName}, {queryCall.ClassName})";

                stringBuilder
                    .AppendWithPadding(queryCallString, 2)
                    .AppendLine($".ToList({listVariableName});");
            }

            stringBuilder.AppendLineWithPadding($@"if ({listVariableName}.Count == 0)
            Logger.LogError(""Query ({memberSymbol.Name}): No instances of {concreteMemberTypeName} found"", this);", 2);

            if (memberType is IArrayTypeSymbol)
            {
                stringBuilder
                    .AppendWithPadding($"{memberSymbol.Name} = new {concreteMemberTypeName}[{listVariableName}.Count];", 2);

                stringBuilder.AppendLine($@"
        for (var i = 0; i < {listVariableName}.Count; i++)
            {memberSymbol.Name}[i] = {listVariableName}[i];");
            }
            else
            {
                var memberIsNotList = memberType.Name != "List";

                if (memberIsNotList)
                    stringBuilder.AppendLineWithPadding($"if ({listVariableName}.Count > 0)", 2).AppendPadding();

                stringBuilder
                    .AppendWithPadding($"{memberSymbol.Name} = {listVariableName}", 2);

                if (memberIsNotList)
                    stringBuilder.Append("[0]");

                stringBuilder.AppendLine(";");
            }
        }

        protected override void AddAdditionalUsings(HashSet<string> usings)
        {
            usings.Add("System.Collections.Generic");
            base.AddAdditionalUsings(usings);
        }

        protected override void GenerateSource(AugmentGenerationContext context, StringBuilder stringBuilder)
        {
            stringBuilder.AppendPadding().AppendLine($@"{Constants.GeneratedCodeAttribute}
    protected override void UIC_PopulateQueryFields()
    {{");

            for (var i = 0; i < _queryDescriptions.Count; i++)
            {
                GenerateQueryAssignment(_queryDescriptions[i], context, stringBuilder);

                if (i != _queryDescriptions.Count - 1)
                    stringBuilder.AppendLine();
            }

            stringBuilder.AppendLine("    }");
        }

        protected override string GetHintPostfix()
        {
            return "Query";
        }
    }
}
