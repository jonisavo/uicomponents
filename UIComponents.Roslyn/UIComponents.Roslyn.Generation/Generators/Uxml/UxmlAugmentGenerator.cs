using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIComponents.Roslyn.Generation.Readers;
using UIComponents.Roslyn.Generation.SyntaxReceivers;

namespace UIComponents.Roslyn.Generation.Generators.Uxml
{
    /// <summary>
    /// A generator for TraitAttribute and UxmlNameAttribute.
    /// Generates UxmlFactory and UxmlTraits implementations.
    /// </summary>
    [Generator]
    internal class UxmlAugmentGenerator : AugmentGenerator<UxmlTraitsSyntaxReceiver>
    {
        private INamedTypeSymbol _traitAttributeSymbol;
        private INamedTypeSymbol _uxmlNameAttributeSymbol;
        private readonly List<TraitDescription> _traitsToGenerate =
            new List<TraitDescription>();
        private UxmlFactoryInfo _uxmlFactoryInfo;

        protected override void OnBeforeExecute(GeneratorExecutionContext context)
        {
            _traitAttributeSymbol =
                context.Compilation.GetTypeByMetadataName("UIComponents.Experimental.TraitAttribute");
            _uxmlNameAttributeSymbol =
                context.Compilation.GetTypeByMetadataName("UIComponents.Experimental.UxmlNameAttribute");
        }

        protected override bool ShouldGenerateSource(AugmentGenerationContext context)
        {
            if (_traitAttributeSymbol == null || _uxmlNameAttributeSymbol == null)
                return false;

            _traitsToGenerate.Clear();

            GetTraitFields(context, _traitsToGenerate);
            GetTraitProperties(context, _traitsToGenerate);
            _uxmlFactoryInfo = GetUxmlFactoryInfo(context);

            return _traitsToGenerate.Count > 0 || !string.IsNullOrEmpty(_uxmlFactoryInfo.Name);
        }

        private void ReadAttributeArguments<TSyntax, TSymbol>(
            TSyntax syntax,
            AttributeArgumentReader<TSyntax> attributeArgumentReader,
            SemanticModel classSemanticModel,
            Dictionary<TSymbol, Dictionary<string, TypedConstant>> output
            )
            where TSyntax : SyntaxNode
            where TSymbol : class, ISymbol
        {
            var arguments = new Dictionary<string, TypedConstant>();
            attributeArgumentReader.Read(syntax, arguments);
            var fieldSymbol = classSemanticModel.GetDeclaredSymbol(syntax) as TSymbol;
            output.Add(fieldSymbol, arguments);
        }

        private void GetTraitFields(AugmentGenerationContext context, IList<TraitDescription> traits)
        {
            var traitFields = new List<IFieldSymbol>();
            var traitArguments = new Dictionary<IFieldSymbol, Dictionary<string, TypedConstant>>();
            var fieldAttributeReader =
                new FieldAttributeReader(_traitAttributeSymbol, context.ClassSemanticModel);
            var variableAttributeArgumentReader =
                new VariableAttributeArgumentReader(_traitAttributeSymbol, context.ClassSemanticModel);

            foreach (var fieldOrProperty in SyntaxReceiver.FieldsAndProperties)
            {
                if (!context.ClassSyntax.ChildNodes().Contains(fieldOrProperty))
                    continue;

                var fieldDeclaration = fieldOrProperty as FieldDeclarationSyntax;

                if (fieldDeclaration == null)
                    continue;

                fieldAttributeReader.Read(fieldDeclaration, traitFields);

                foreach (var variableDeclaration in fieldDeclaration.Declaration.Variables)
                    ReadAttributeArguments(variableDeclaration, variableAttributeArgumentReader, context.ClassSemanticModel, traitArguments);
            }

            foreach (var traitField in traitFields)
                traits.Add(TraitDescription.CreateFromFieldSymbol(traitField, traitArguments[traitField]));
        }

        private void GetTraitProperties(AugmentGenerationContext context, IList<TraitDescription> traits)
        {
            var traitProperties = new List<IPropertySymbol>();
            var traitArguments = new Dictionary<IPropertySymbol, Dictionary<string, TypedConstant>>();
            var propertyAttributeReader = new PropertyAttributeReader(_traitAttributeSymbol, context.ClassSemanticModel);
            var propertyAttributeArgumentReader =
                new PropertyAttributeArgumentReader(_traitAttributeSymbol, context.ClassSemanticModel);

            foreach (var fieldOrProperty in SyntaxReceiver.FieldsAndProperties)
            {
                if (!context.ClassSyntax.ChildNodes().Contains(fieldOrProperty))
                    continue;

                var propertyDeclaration = fieldOrProperty as PropertyDeclarationSyntax;

                if (propertyDeclaration == null)
                    continue;

                propertyAttributeReader.Read(propertyDeclaration, traitProperties);

                ReadAttributeArguments(propertyDeclaration, propertyAttributeArgumentReader, context.ClassSemanticModel, traitArguments);
            }

            foreach (var traitProperty in traitProperties)
                traits.Add(TraitDescription.CreateFromPropertySymbol(traitProperty, traitArguments[traitProperty]));
        }

        private UxmlFactoryInfo GetUxmlFactoryInfo(AugmentGenerationContext context)
        {
            var arguments = new Dictionary<string, TypedConstant>();
            var classAttributeArgumentReader = new ClassAttributeArgumentReader(_uxmlNameAttributeSymbol, context.ClassSemanticModel);

            classAttributeArgumentReader.Read(context.ClassSyntax, arguments);

            if (arguments.Count == 0)
                return default;

            return UxmlFactoryInfo.CreateFromArguments(arguments);
        }

        private void WriteUxmlFactory(UxmlFactoryInfo info, AugmentGenerationContext context, StringBuilder stringBuilder)
        {
            var compilation = context.GeneratorExecutionContext.Compilation;
            var uxmlTraitsMetadataName = $"{context.CurrentTypeSymbol.MetadataName}.UxmlTraits";

            var traitsDefined = _traitsToGenerate.Count > 0 ||
                compilation.GetTypeByMetadataName(uxmlTraitsMetadataName) != null;

            stringBuilder.Append("    ")
                .Append($"public new partial class UxmlFactory : UxmlFactory<{context.TypeName}");

            if (traitsDefined)
                stringBuilder.Append(", UxmlTraits>");
            else
                stringBuilder.Append(">");

            if (string.IsNullOrEmpty(info.Name))
            {
                stringBuilder.AppendLine(" {}");
                return;
            }

            stringBuilder.AppendLine($@"
    {{
        public override string uxmlName
        {{
            get {{ return ""{info.Name}""; }}
        }}

        public override string uxmlQualifiedName
        {{
            get {{ return uxmlNamespace + ""."" + uxmlName; }}
        }}
    }}");
        }

        private void WriteTraitDescriptionVariables(List<TraitDescription> traits, StringBuilder stringBuilder)
        {
            foreach (var trait in traits)
            {
                stringBuilder
                    .Append("        ")
                    .AppendLine($"{trait.Type} {trait.TraitMemberName} = new {trait.Type} {{ name = \"{trait.UxmlName}\" }};");
            }
        }

        private void WriteTraitDescriptionDefaultValues(List<TraitDescription> traits, StringBuilder stringBuilder)
        {
            foreach (var trait in traits)
            {
                if (string.IsNullOrEmpty(trait.DefaultValue))
                    continue;

                stringBuilder
                    .Append("            ")
                    .AppendLine($"{trait.TraitMemberName}.defaultValue = {trait.DefaultValue};");
            }
        }

        private void WriteTraitDescriptionInitialization(List<TraitDescription> traits, AugmentGenerationContext context, StringBuilder stringBuilder)
        {
            foreach (var trait in traits)
            {
                stringBuilder
                    .Append("            ")
                    .AppendLine($"(({context.TypeName})ve).{trait.ClassMemberName} = {trait.TraitMemberName}.GetValueFromBag(bag, cc);");
            }
        }

        private void WriteUxmlTraits(List<TraitDescription> traits, AugmentGenerationContext context, StringBuilder stringBuilder)
        {
            if (traits.Count == 0)
                return;

            stringBuilder.AppendLine($@"
    public new partial class UxmlTraits : VisualElement.UxmlTraits
    {{");
            WriteTraitDescriptionVariables(_traitsToGenerate, stringBuilder);

            stringBuilder.AppendLine($@"
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {{
            base.Init(ve, bag, cc);");

            WriteTraitDescriptionDefaultValues(_traitsToGenerate, stringBuilder);
            WriteTraitDescriptionInitialization(_traitsToGenerate, context, stringBuilder);

            stringBuilder.Append("        ").AppendLine($@"}}
    }}");
        }

        protected override void GenerateSource(AugmentGenerationContext context, StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("using UnityEngine.UIElements;").AppendLine();

            stringBuilder.AppendLine($@"public partial class {context.TypeName}
{{");

            WriteUxmlFactory(_uxmlFactoryInfo, context, stringBuilder);

            WriteUxmlTraits(_traitsToGenerate, context, stringBuilder);

            stringBuilder.AppendLine("}");
        }

        protected override string GetHintPostfix()
        {
            return "Uxml";
        }
    }
}
