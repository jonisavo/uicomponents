using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIComponents.Roslyn.Generation.Readers;
using UIComponents.Roslyn.Generation.SyntaxReceivers;
using UIComponents.Roslyn.Generation.Utilities;

namespace UIComponents.Roslyn.Generation.Generators.Uxml
{
    /// <summary>
    /// A generator for TraitAttribute and UxmlNameAttribute.
    /// Generates UxmlFactory and UxmlTraits implementations.
    /// </summary>
    [Generator]
    public sealed class UxmlAugmentGenerator : AugmentGenerator<ClassMemberSyntaxReceiver>
    {
        private INamedTypeSymbol _traitAttributeSymbol;
        private INamedTypeSymbol _uxmlNameAttributeSymbol;
        private readonly List<TraitDescription> _traitsToGenerate =
            new List<TraitDescription>();
        private UxmlFactoryInfo _uxmlFactoryInfo;

        protected override void OnBeforeExecute(GeneratorExecutionContext context)
        {
            _traitAttributeSymbol =
                context.Compilation.GetTypeByMetadataName("UIComponents.UxmlTraitAttribute");
            _uxmlNameAttributeSymbol =
                context.Compilation.GetTypeByMetadataName("UIComponents.UxmlNameAttribute");
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
            AttributeArgumentReader attributeArgumentReader,
            SemanticModel classSemanticModel,
            Dictionary<TSymbol, Dictionary<string, TypedConstant>> output
            )
            where TSyntax : SyntaxNode
            where TSymbol : class, ISymbol
        {
            var attributeArguments = new Dictionary<AttributeData, Dictionary<string, TypedConstant>>();
            attributeArgumentReader.Read(syntax, attributeArguments);
            var fieldSymbol = classSemanticModel.GetDeclaredSymbol(syntax) as TSymbol;
            var arguments = attributeArguments.Values.ToArray();

            if (arguments.Length > 0)
                output.Add(fieldSymbol, arguments[0]);
        }

        private void GetTraitFields(AugmentGenerationContext context, IList<TraitDescription> traits)
        {
            var traitFields = new List<IFieldSymbol>();
            var traitArguments = new Dictionary<IFieldSymbol, Dictionary<string, TypedConstant>>();
            var fieldAttributeReader =
                new FieldAttributeReader(_traitAttributeSymbol, context.ClassSemanticModel);
            var attributeArgumentReader =
                new AttributeArgumentReader(_traitAttributeSymbol, context.ClassSemanticModel);

            foreach (var fieldOrProperty in SyntaxReceiver.FieldsAndProperties)
            {
                if (!context.ClassSyntax.ChildNodes().Contains(fieldOrProperty))
                    continue;

                var fieldDeclaration = fieldOrProperty as FieldDeclarationSyntax;

                if (fieldDeclaration == null)
                    continue;

                fieldAttributeReader.Read(fieldDeclaration, traitFields);

                foreach (var variableDeclaration in fieldDeclaration.Declaration.Variables)
                    ReadAttributeArguments(variableDeclaration, attributeArgumentReader, context.ClassSemanticModel, traitArguments);
            }

            foreach (var traitField in traitFields)
                traits.Add(TraitDescription.CreateFromFieldSymbol(traitField, traitArguments[traitField]));
        }

        private void GetTraitProperties(AugmentGenerationContext context, IList<TraitDescription> traits)
        {
            var traitProperties = new List<IPropertySymbol>();
            var traitArguments = new Dictionary<IPropertySymbol, Dictionary<string, TypedConstant>>();
            var propertyAttributeReader = new PropertyAttributeReader(_traitAttributeSymbol, context.ClassSemanticModel);
            var attributeArgumentReader =
                new AttributeArgumentReader(_traitAttributeSymbol, context.ClassSemanticModel);

            foreach (var fieldOrProperty in SyntaxReceiver.FieldsAndProperties)
            {
                if (!context.ClassSyntax.ChildNodes().Contains(fieldOrProperty))
                    continue;

                var propertyDeclaration = fieldOrProperty as PropertyDeclarationSyntax;

                if (propertyDeclaration == null)
                    continue;

                propertyAttributeReader.Read(propertyDeclaration, traitProperties);

                ReadAttributeArguments(propertyDeclaration, attributeArgumentReader, context.ClassSemanticModel, traitArguments);
            }

            foreach (var traitProperty in traitProperties)
                traits.Add(TraitDescription.CreateFromPropertySymbol(traitProperty, traitArguments[traitProperty]));
        }

        private UxmlFactoryInfo GetUxmlFactoryInfo(AugmentGenerationContext context)
        {
            var attributeArguments = new Dictionary<AttributeData, Dictionary<string, TypedConstant>>();
            var classAttributeArgumentReader = new ClassAttributeArgumentReader(_uxmlNameAttributeSymbol, context.ClassSemanticModel);

            classAttributeArgumentReader.Read(context.ClassSyntax, attributeArguments);

            foreach (var arguments in attributeArguments.Values)
            {
                if (arguments.Count == 0)
                    continue;

                return UxmlFactoryInfo.CreateFromArguments(arguments);
            }

            return default;
        }

        private void WriteUxmlFactory(UxmlFactoryInfo info, AugmentGenerationContext context, StringBuilder stringBuilder)
        {
            var compilation = context.GeneratorExecutionContext.Compilation;
            var uxmlTraitsMetadataName = $"{context.CurrentTypeSymbol.MetadataName}.UxmlTraits";

            var traitsDefined = _traitsToGenerate.Count > 0 ||
                compilation.GetTypeByMetadataName(uxmlTraitsMetadataName) != null;

            stringBuilder
                .AppendPadding()
                .AppendCodeGeneratedAttribute()
                .AppendPadding()
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
        {Constants.GeneratedCodeAttribute}
        public override string uxmlName
        {{
            get {{ return ""{info.Name}""; }}
        }}

        {Constants.GeneratedCodeAttribute}
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
                    .AppendPadding(2)
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
                    .AppendPadding(3)
                    .AppendLine($"{trait.TraitMemberName}.defaultValue = {trait.DefaultValue};");
            }
        }

        private void WriteTraitDescriptionInitialization(List<TraitDescription> traits, AugmentGenerationContext context, StringBuilder stringBuilder)
        {
            foreach (var trait in traits)
            {
                stringBuilder
                    .AppendPadding(3)
                    .AppendLine($"(({context.TypeName})ve).{trait.ClassMemberName} = {trait.TraitMemberName}.GetValueFromBag(bag, cc);");
            }
        }

        private void WriteUxmlTraits(List<TraitDescription> traits, AugmentGenerationContext context, StringBuilder stringBuilder)
        {
            if (traits.Count == 0)
                return;

            stringBuilder.AppendLine($@"
    {Constants.GeneratedCodeAttribute}
    public new partial class UxmlTraits : VisualElement.UxmlTraits
    {{");
            WriteTraitDescriptionVariables(_traitsToGenerate, stringBuilder);

            stringBuilder.AppendLine($@"
        {Constants.GeneratedCodeAttribute}
        public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
        {{
            base.Init(ve, bag, cc);");

            WriteTraitDescriptionDefaultValues(_traitsToGenerate, stringBuilder);
            WriteTraitDescriptionInitialization(_traitsToGenerate, context, stringBuilder);

            stringBuilder.AppendPadding(2).AppendLine($@"}}
    }}");
        }

        protected override void GenerateSource(AugmentGenerationContext context, StringBuilder stringBuilder)
        {
            WriteUxmlFactory(_uxmlFactoryInfo, context, stringBuilder);

            WriteUxmlTraits(_traitsToGenerate, context, stringBuilder);
        }

        protected override string GetHintPostfix()
        {
            return "Uxml";
        }
    }
}
