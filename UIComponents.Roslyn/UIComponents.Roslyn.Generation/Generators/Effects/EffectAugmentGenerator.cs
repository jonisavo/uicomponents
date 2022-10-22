using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIComponents.Roslyn.Generation.Utilities;

namespace UIComponents.Roslyn.Generation.Generators.Effects
{
    [Generator]
    internal class EffectAugmentGenerator : UIComponentAugmentGenerator
    {
        private INamedTypeSymbol _uiComponentEffectSymbol;
        private readonly List<EffectDescription> _effectDescriptions =
            new List<EffectDescription>();

        protected override void OnBeforeExecute(GeneratorExecutionContext context)
        {
            base.OnBeforeExecute(context);
            _uiComponentEffectSymbol =
                context.Compilation.GetTypeByMetadataName("UIComponents.UIComponentEffectAttribute");
        }

        private void GetEffectDescriptions(AugmentGenerationContext context, IList<EffectDescription> output)
        {
            var allAttributes = RoslynUtilities.GetAllAttributesOfType(context.CurrentTypeSymbol);

            var effectAttributes = allAttributes.Where(attribute =>
            {
                return RoslynUtilities.HasBaseType(attribute.AttributeClass, _uiComponentEffectSymbol);
            });

            foreach (var attribute in effectAttributes)
                output.Add(EffectDescription.FromAttributeData(attribute));
        }

        protected override bool ShouldGenerateSource(AugmentGenerationContext context)
        {
            if (!base.ShouldGenerateSource(context))
                return false;

            _effectDescriptions.Clear();
            GetEffectDescriptions(context, _effectDescriptions);

            return _uiComponentEffectSymbol != null && _effectDescriptions.Count > 0;
        }

        protected override void BuildUsingStatements(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("using System;");
            base.BuildUsingStatements(stringBuilder);
        }

        private void GenerateEffectArrayInitialization(StringBuilder stringBuilder)
        {
            stringBuilder
                .Append("    ")
                .Append(Constants.GeneratedCodeAttribute)
                .AppendLine(@"
    private static void UIC_InitializeEffectAttributes()
    {
        UIC_EffectAttributes = new [] {");

            for (var i = 0; i < _effectDescriptions.Count; i++)
            {
                var effect = _effectDescriptions[i];

                stringBuilder
                    .Append("            new ")
                    .Append(effect.EffectTypeSymbol.ToDisplayString())
                    .Append(effect.CallString);

                if (i != _effectDescriptions.Count - 1)
                    stringBuilder.Append(',');

                stringBuilder.AppendLine();
            }

            stringBuilder
                .AppendLine("        };")
                .AppendLine("        Array.Sort(UIC_EffectAttributes);")
                .AppendLine("    }");
        }

        protected override void GenerateSource(AugmentGenerationContext context, StringBuilder stringBuilder)
        {
            stringBuilder
                .Append("    ")
                .AppendLine(Constants.GeneratedCodeAttribute)
                .Append("    ")
                .AppendLine("private static UIComponentEffectAttribute[] UIC_EffectAttributes;")
                .AppendLine();

            GenerateEffectArrayInitialization(stringBuilder);

            stringBuilder.AppendLine($@"
    {Constants.GeneratedCodeAttribute}
    protected override void UIC_ApplyEffects()
    {{
        if (UIC_EffectAttributes == null)
            UIC_InitializeEffectAttributes();

        foreach (var effect in UIC_EffectAttributes)
            effect.Apply(this);
    }}");
        }

        protected override string GetHintPostfix()
        {
            return "Effects";
        }
    }
}
