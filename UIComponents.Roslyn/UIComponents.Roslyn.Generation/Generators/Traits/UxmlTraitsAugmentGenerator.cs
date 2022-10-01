using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIComponents.Roslyn.Generation.Readers;
using UIComponents.Roslyn.Generation.SyntaxReceivers;

namespace UIComponents.Roslyn.Generation.Generators.Traits
{
    /// <summary>
    /// A generator for TraitAttribute. Generates a UxmlFactory and UxmlTraits implementation.
    /// </summary>
    [Generator]
    internal class UxmlTraitsAugmentGenerator : AugmentGenerator<UxmlTraitsSyntaxReceiver>
    {
        private INamedTypeSymbol _traitAttributeSymbol;
        private readonly List<TraitDescription> _traitsToGenerate =
            new List<TraitDescription>();

        protected override void OnBeforeExecute(GeneratorExecutionContext context)
        {
            _traitAttributeSymbol =
                context.Compilation.GetTypeByMetadataName("UIComponents.Experimental.TraitAttribute");
        }

        protected override bool ShouldGenerateSource(AugmentGenerationContext context)
        {
            if (_traitAttributeSymbol == null)
                return false;

            _traitsToGenerate.Clear();

            GetTraitFields(context, _traitsToGenerate);
            GetTraitProperties(context, _traitsToGenerate);

            return _traitsToGenerate.Count > 0;
        }

        private (string, string) GetTraitUxmlNameAndDefaultValue(Dictionary<string, TypedConstant> traitArguments, string traitDefaultName)
        {
            string uxmlName = null;
            string defaultValue = null;

            if (traitArguments.ContainsKey("Name"))
                uxmlName = traitArguments["Name"].Value?.ToString();

            if (string.IsNullOrEmpty(uxmlName))
                uxmlName = traitDefaultName;

            if (traitArguments.ContainsKey("DefaultValue"))
            {
                var defaultValueString = traitArguments["DefaultValue"].Value?.ToString();

                if (!string.IsNullOrEmpty(defaultValueString) && traitArguments["DefaultValue"].Value is string)
                    defaultValueString = $"\"{defaultValueString}\"";

                defaultValue = defaultValueString;
            }

            return (uxmlName, defaultValue);
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
                {
                    var arguments = new Dictionary<string, TypedConstant>();
                    variableAttributeArgumentReader.Read(variableDeclaration, arguments);
                    var fieldSymbol = context.ClassSemanticModel.GetDeclaredSymbol(variableDeclaration) as IFieldSymbol;
                    traitArguments.Add(fieldSymbol, arguments);
                }
            }

            foreach (var traitField in traitFields)
            {
                var arguments = traitArguments[traitField];
                var (uxmlName, defaultValue) = GetTraitUxmlNameAndDefaultValue(arguments, traitField.Name.ToLower());

                traits.Add(TraitDescription.CreateFromFieldSymbol(traitField, uxmlName, defaultValue));
            }
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

                var arguments = new Dictionary<string, TypedConstant>();
                propertyAttributeArgumentReader.Read(propertyDeclaration, arguments);
                var propertySymbol = context.ClassSemanticModel.GetDeclaredSymbol(propertyDeclaration) as IPropertySymbol;
                traitArguments.Add(propertySymbol, arguments);
            }

            foreach (var traitProperty in traitProperties)
            {
                var arguments = traitArguments[traitProperty];
                var (uxmlName, defaultValue) = GetTraitUxmlNameAndDefaultValue(arguments, traitProperty.Name.ToLower());

                traits.Add(TraitDescription.CreateFromPropertySymbol(traitProperty, uxmlName, defaultValue));
            }
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

        protected override void GenerateSource(AugmentGenerationContext context, StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("using UnityEngine.UIElements;").AppendLine();

            stringBuilder.AppendLine($@"public partial class {context.TypeName}
{{
    public new partial class UxmlFactory : UxmlFactory<{context.TypeName}, UxmlTraits> {{}}

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
    }}
}}");
        }

        protected override string GetHintPostfix()
        {
            return "UxmlTraits";
        }
    }
}
