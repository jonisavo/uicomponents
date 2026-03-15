using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIComponents.Roslyn.Generation.Utilities;

namespace UIComponents.Roslyn.Generation.Generators.AssetLoad
{
    [Generator]
    public sealed class StylesheetAugmentGenerator : UIComponentAugmentGenerator
    {
        private INamedTypeSymbol _stylesheetAttributeSymbol;
        private INamedTypeSymbol _sharedStylesheetAttributeSymbol;
        private readonly List<StylesheetDescription> _stylesheetDescriptions
            = new List<StylesheetDescription>();

        protected override void OnBeforeExecute(GeneratorExecutionContext context)
        {
            base.OnBeforeExecute(context);
            _stylesheetAttributeSymbol = context.Compilation.GetTypeByMetadataName("UIComponents.StylesheetAttribute");
            _sharedStylesheetAttributeSymbol = context.Compilation.GetTypeByMetadataName("UIComponents.SharedStylesheetAttribute");
        }

        private StylesheetDescription GetStylesheetDescription(string stylesheetPath, string declaringTypeFullName)
        {
            return new StylesheetDescription(BuildPrefixedPath(stylesheetPath), declaringTypeFullName);
        }

        private StylesheetDescription GetConventionStylesheetDescription(string className, string declaringTypeFullName)
        {
            return new StylesheetDescription(
                BuildPrefixedPath(className + Constants.ConventionStylesheetSuffix),
                declaringTypeFullName);
        }

        private StylesheetDescription GetSharedStylesheetDescription(string name)
        {
            return new StylesheetDescription(BuildPrefixedPath(name), name, isShared: true);
        }

        /// <summary>
        /// Walks the type hierarchy base-first and collects all stylesheet
        /// descriptions from both [Stylesheet] and [SharedStylesheet] attributes.
        ///
        /// For each type in the hierarchy, all its [Stylesheet] attributes are
        /// collected first, then all its [SharedStylesheet] attributes. This
        /// ensures per-type-level ordering: a base class's shared stylesheets
        /// appear before the child's regular stylesheets.
        ///
        /// For parameterless [Stylesheet] attributes, the convention path uses
        /// the DECLARING type's name (not the current processing type's name).
        /// </summary>
        private void CollectStylesheetDescriptions(AugmentGenerationContext context, IList<StylesheetDescription> stylesheets)
        {
            // Build base-first hierarchy (stop at UIComponent's base type)
            var typeHierarchy = new List<INamedTypeSymbol>();
            var typeSymbol = context.CurrentTypeSymbol;

            while (typeSymbol != null &&
                   !SymbolEqualityComparer.Default.Equals(typeSymbol, UIComponentSymbol?.BaseType))
            {
                typeHierarchy.Add(typeSymbol);
                typeSymbol = typeSymbol.BaseType;
            }

            typeHierarchy.Reverse();

            foreach (var type in typeHierarchy)
            {
                var attributes = type.GetAttributes();
                var typeFullName = type.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);

                // Collect [Stylesheet] attributes for this type
                if (_stylesheetAttributeSymbol != null)
                {
                    foreach (var attribute in attributes)
                    {
                        if (!SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, _stylesheetAttributeSymbol))
                            continue;

                        if (attribute.ConstructorArguments.Length > 0 &&
                            attribute.ConstructorArguments[0].Value is string path)
                            stylesheets.Add(GetStylesheetDescription(path, typeFullName));
                        else
                            stylesheets.Add(GetConventionStylesheetDescription(type.Name, typeFullName));
                    }
                }

                // Collect [SharedStylesheet] attributes for this type
                if (_sharedStylesheetAttributeSymbol != null)
                {
                    foreach (var attribute in attributes)
                    {
                        if (!SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, _sharedStylesheetAttributeSymbol))
                            continue;

                        if (attribute.ConstructorArguments.Length > 0 &&
                            attribute.ConstructorArguments[0].Value is string name)
                            stylesheets.Add(GetSharedStylesheetDescription(name));
                    }
                }
            }
        }

        protected override bool ShouldGenerateSource(AugmentGenerationContext context)
        {
            if (!base.ShouldGenerateSource(context))
                return false;

            _stylesheetDescriptions.Clear();
            CollectStylesheetDescriptions(context, _stylesheetDescriptions);

            return _stylesheetDescriptions.Count > 0;
        }

        protected override void AddAdditionalUsings(HashSet<string> usings)
        {
            base.AddAdditionalUsings(usings);
            usings.Add("System.Threading.Tasks");
        }

        protected override void GenerateSource(AugmentGenerationContext context, StringBuilder stringBuilder)
        {
            stringBuilder.AppendLineWithPadding($@"{Constants.GeneratedCodeAttribute}
    private async Task<StyleSheetLoadTuple> UIC_GetSingleStyleSheet(string assetPath)
    {{
        var styleSheet = await AssetResolver.LoadAsset<StyleSheet>(assetPath);
        return new StyleSheetLoadTuple(assetPath, styleSheet);
    }}

    {Constants.GeneratedCodeAttribute}
    protected override Task<StyleSheetLoadTuple>[] UIC_StartStyleSheetLoad()
    {{
        var assetPaths = new string[] {{ {BuildStylesheetPathArrayElements()} }};
        var styleSheetLoadTasks = new Task<StyleSheetLoadTuple>[assetPaths.Length];

        for (var i = 0; i < assetPaths.Length; i++)
            styleSheetLoadTasks[i] = UIC_GetSingleStyleSheet(assetPaths[i]);

        return styleSheetLoadTasks;
    }}");
        }

        private string BuildStylesheetPathArrayElements()
        {
            return string.Join(", ", _stylesheetDescriptions.Select(desc =>
            {
                if (desc.IsShared)
                    return $"AssetCatalog.ResolveSharedStylesheetPath(\"{desc.LogicalName}\", \"{desc.Path}\")";
                return $"AssetCatalog.ResolveStylesheetPath(typeof({desc.DeclaringTypeFullName}), \"{desc.Path}\")";
            }));
        }

        protected override string GetHintPostfix()
        {
            return "Stylesheet";
        }
    }
}
