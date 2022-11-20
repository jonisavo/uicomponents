using System.Text;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using UIComponents.Roslyn.Generation.SyntaxReceivers;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UIComponents.Roslyn.Generation.Utilities;
using System.Linq;
using UIComponents.Roslyn.Generation.Readers;

namespace UIComponents.Roslyn.Generation.Generators.DependencyInjection
{
    [Generator]
    public sealed class DependencyAugmentGenerator : AugmentGenerator<TypeSyntaxReceiver>
    {
        private INamedTypeSymbol _dependencyAttributeSymbol;
        private INamedTypeSymbol _dependencyConsumerInterfaceSymbol;
        private INamedTypeSymbol _uiComponentSymbol;
        private readonly List<DependencyDescription> _dependencyDescriptions
            = new List<DependencyDescription>();

        protected override void OnBeforeExecute(GeneratorExecutionContext context)
        {
            _dependencyAttributeSymbol =
                context.Compilation.GetTypeByMetadataName("UIComponents.DependencyAttribute");
            _dependencyConsumerInterfaceSymbol =
                context.Compilation.GetTypeByMetadataName("UIComponents.DependencyInjection.IDependencyConsumer");
            _uiComponentSymbol =
                context.Compilation.GetTypeByMetadataName("UIComponents.UIComponent");
        }

        private void GetDependencyDescriptions(AugmentGenerationContext context, IList<DependencyDescription> output)
        {
            var attributeDatas = new Dictionary<AttributeData, Dictionary<string, TypedConstant>>();

            var assembly = context.GeneratorExecutionContext.Compilation.Assembly;

            var allAssemblyAttributes = assembly.GetAttributes();

            var assemblyAttributes = allAssemblyAttributes.Where(
                (attribute) => attribute.AttributeClass.Equals(_dependencyAttributeSymbol, SymbolEqualityComparer.Default)
            );

            foreach (var assemblyAttributeData in assemblyAttributes)
            {
                var attributeArgs = new Dictionary<string, TypedConstant>();
                RoslynUtilities.ReadAttributeArguments(assemblyAttributeData, attributeArgs);
                attributeDatas.Add(assemblyAttributeData, attributeArgs);
            }

            var reader = new ClassAttributeArgumentReader(_dependencyAttributeSymbol, context.ClassSemanticModel);
            reader.SetReadOrder(AttributeReadOrder.BaseFirst);

            reader.ReadWithSymbol(context.CurrentTypeSymbol, attributeDatas);

            foreach (var attributeData in attributeDatas)
            {
                var description = DependencyDescription.FromAttributeArgs(attributeData.Value);

                if (output.Contains(description))
                    output.Remove(description);

                output.Add(description);
            }
        }

        protected override bool ShouldGenerateSource(AugmentGenerationContext context)
        {
            if (_dependencyAttributeSymbol == null ||
                _dependencyConsumerInterfaceSymbol == null ||
                _uiComponentSymbol == null)
                return false;

            if (context.ClassSyntax is InterfaceDeclarationSyntax)
                return false;

            if (context.CurrentTypeSymbol.IsAbstract)
                return false;

            var hasDependencyConsumerInterface = context.CurrentTypeSymbol.AllInterfaces
                .Where((interfaceType) => interfaceType.Equals(_dependencyConsumerInterfaceSymbol, SymbolEqualityComparer.Default))
                .Any();

            if (!hasDependencyConsumerInterface)
                return false;

            _dependencyDescriptions.Clear();
            GetDependencyDescriptions(context, _dependencyDescriptions);

            return _dependencyDescriptions.Count > 0;
        }

        protected override void BuildUsingStatements(StringBuilder stringBuilder)
        {
            stringBuilder.AppendLine("using System.Collections.Generic;");
            stringBuilder.AppendLine("using UIComponents.DependencyInjection;");
            base.BuildUsingStatements(stringBuilder);
        }

        protected override void GenerateSource(AugmentGenerationContext context, StringBuilder stringBuilder)
        {
            stringBuilder
                .AppendPadding()
                .AppendCodeGeneratedAttribute();

            stringBuilder.AppendLineWithPadding("private static readonly IDependency[] UIC_Dependencies = new IDependency[] {");

            for (var i = 0; i < _dependencyDescriptions.Count; i++)
            {
                var dependency = _dependencyDescriptions[i];
                stringBuilder.AppendWithPadding(dependency.ToConstructorCallString(), 2);
                if (i != _dependencyDescriptions.Count - 1)
                    stringBuilder.Append(",");
                stringBuilder.AppendLine();
            }

            stringBuilder
                .AppendLineWithPadding("};")
                .AppendLine();

            stringBuilder
                .AppendPadding()
                .AppendCodeGeneratedAttribute();

            stringBuilder.AppendWithPadding("public ");

            if (RoslynUtilities.HasBaseType(context.CurrentTypeSymbol, _uiComponentSymbol))
                stringBuilder.Append("override ");

            stringBuilder.AppendLine(@"IEnumerable<IDependency> GetDependencies()
    {
        return UIC_Dependencies;
    }");
        }

        protected override string GetHintPostfix()
        {
            return "Dependencies";
        }
    }
}

