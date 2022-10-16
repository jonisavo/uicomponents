using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIComponents.Roslyn.Generation.Utilities;

namespace UIComponents.Roslyn.Generation.Generators.Uxml
{
    [Generator]
    internal class QueryAugmentGenerator : UIComponentAugmentGenerator
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

                if (concreteType == null)
                    continue;

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

                foreach (var arguments in argumentsDictionary.Values)
                    queryDescriptions.Add(QueryDescription.CreateFromMember(member, arguments));
            }
        }

        protected override bool ShouldGenerateSource(AugmentGenerationContext context)
        {
            if (!base.ShouldGenerateSource(context))
                return false;

            _queryDescriptions.Clear();
            GetQueryDescriptions(context, _queryDescriptions);

            return _queryAttributeSymbol != null && _queryDescriptions.Count > 0;
        }

        private void GenerateQueryAssignment(QueryDescription queryDescription, StringBuilder stringBuilder)
        {
            var memberSymbol = queryDescription.MemberSymbol;
            var memberType = RoslynUtilities.GetMemberType(memberSymbol);

            var concreteMemberType = RoslynUtilities.GetConcreteType(memberType);
            var concreteMemberTypeName = concreteMemberType.ToDisplayString();

            var queryCallString = $"this.Query<{concreteMemberTypeName}>({queryDescription.UxmlName}, {queryDescription.ClassName})";

            if (memberType is IArrayTypeSymbol)
            {
                var listVariableName = $"{memberSymbol.Name}List";
                stringBuilder
                    .Append("        ")
                    .AppendLine($"var {listVariableName} = new List<{concreteMemberTypeName}>();");

                stringBuilder
                    .Append("        ")
                    .Append(queryCallString)
                    .AppendLine($".ToList({listVariableName});");

                stringBuilder
                    .Append("        ")
                    .Append($"{memberSymbol.Name} = new {concreteMemberTypeName}[{listVariableName}.Count];");

                stringBuilder.AppendLine($@"
        for (var i = 0; i < {listVariableName}.Count; i++)
            {memberSymbol.Name}[i] = {listVariableName}[i];");
            }
            else
            {
                stringBuilder
                    .Append("        ")
                    .Append($"{memberSymbol.Name} = this.Query<{concreteMemberTypeName}>({queryDescription.UxmlName}, {queryDescription.ClassName})");

                var name = memberType.Name;

                if (name == "List")
                    stringBuilder.AppendLine(".ToList();");
                else
                    stringBuilder.AppendLine(".First();");
            }
        }

        protected override void BuildUsingStatements(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("using System.Collections.Generic;");
            base.BuildUsingStatements(stringBuilder);
        }

        protected override void GenerateSource(AugmentGenerationContext context, StringBuilder stringBuilder)
        {
            stringBuilder.Append("    ").AppendLine($@"protected override void PopulateQueryFields()
    {{");

            foreach (var queryDescription in _queryDescriptions)
                GenerateQueryAssignment(queryDescription, stringBuilder);

            stringBuilder.AppendLine("    }");
        }

        protected override string GetHintPostfix()
        {
            return "Query";
        }
    }
}
