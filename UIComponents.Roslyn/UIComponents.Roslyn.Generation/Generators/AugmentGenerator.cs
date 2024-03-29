﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIComponents.Roslyn.Generation.SyntaxReceivers;
using UIComponents.Roslyn.Generation.Utilities;
using UIComponents.Roslyn.Common.Utilities;

namespace UIComponents.Roslyn.Generation.Generators
{
    /// <summary>
    /// A generator for augmenting code.
    /// </summary>
    /// <typeparam name="TSyntaxReceiver">Used syntax receiver</typeparam>
    public abstract class AugmentGenerator<TSyntaxReceiver> : ISourceGenerator
        where TSyntaxReceiver : ISyntaxReceiverWithTypes, new()
    {
        protected TSyntaxReceiver SyntaxReceiver { get; private set; }

        private readonly AugmentGenerationContext _currentContext
            = new AugmentGenerationContext();
        private ParentClass _currentParentClass;

        private readonly StringBuilder _stringBuilder = new StringBuilder();

        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new TSyntaxReceiver());
        }

        /// <summary>
        /// Called before executing the generator.
        /// </summary>
        protected abstract void OnBeforeExecute(GeneratorExecutionContext context);

        /// <summary>
        /// Returns whether source should be generated in the given context.
        /// </summary>
        /// <param name="context">Current generation context</param>
        protected abstract bool ShouldGenerateSource(AugmentGenerationContext context);

        /// <summary>
        /// Generates source in the given context.
        /// </summary>
        /// <param name="context">Current generation context</param>
        /// <param name="stringBuilder">Used StringBuilder</param>
        protected abstract void GenerateSource(AugmentGenerationContext context, StringBuilder stringBuilder);

        /// <returns>A postfix used for the hint file name.</returns>
        protected abstract string GetHintPostfix();

        private string GetHintName()
        {
            var hintBuilder = new StringBuilder();

            var parentClassString = _currentParentClass?.ToString();

            if (!string.IsNullOrEmpty(parentClassString))
                hintBuilder.Append(parentClassString).Append(".");

            hintBuilder.Append(_currentContext.TypeName);

            var postfix = GetHintPostfix();

            if (!string.IsNullOrEmpty(postfix))
                hintBuilder.Append(".").Append(postfix);

            hintBuilder.Append(".g.cs");

            return hintBuilder.ToString();
        }

        protected virtual void AddAdditionalUsings(HashSet<string> usings)
        {
            usings.Add("System.CodeDom.Compiler");
            usings.Add("UnityEngine.UIElements");
        }

        private void ExecuteForType(TypeDeclarationSyntax node, GeneratorExecutionContext context)
        {
            _stringBuilder.Clear();
            _currentContext.ClassSyntax = node;
            _currentContext.ClassSemanticModel = context.Compilation.GetSemanticModel(node.SyntaxTree);
            _currentContext.CurrentTypeNamespace = RoslynUtilities.GetTypeNamespace(node);
            _currentContext.TypeName = RoslynUtilities.GetTypeName(node);
            _currentParentClass = ParentClass.GetParentClasses(_currentContext.ClassSyntax);

            var compilationUnitSyntax = RoslynUtilities.GetCompilationUnitSyntax(node);

            var usingsList = compilationUnitSyntax.Usings
                .Where((declaration) => declaration.Alias == null)
                .Select((declaration) => declaration.Name.ToString())
                .ToList();

            _currentContext.Usings = new HashSet<string>();

            foreach (var usingName in usingsList)
                _currentContext.Usings.Add(usingName);

            _currentContext.CurrentTypeSymbol =
                _currentContext.ClassSemanticModel.GetDeclaredSymbol(node) as INamedTypeSymbol;

            if (!ShouldGenerateSource(_currentContext))
                return;

            AddAdditionalUsings(_currentContext.Usings);

            _stringBuilder.AppendLine(@"// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>
");

            foreach (var usingNamespace in _currentContext.Usings)
            {
                _stringBuilder.Append("using ")
                    .Append(usingNamespace)
                    .AppendLine(";");
            }

            _stringBuilder.AppendLine();

            using (new WithinNamespaceScope(_currentContext.CurrentTypeNamespace, _stringBuilder))
            {
                using (new WithinParentClassScope(_currentParentClass, _stringBuilder))
                {
                    var accessibility = _currentContext.CurrentTypeSymbol.DeclaredAccessibility
                        .ToString()
                        .ToLower();

                    var keyword = _currentContext.ClassSyntax.Keyword;

                    _stringBuilder.AppendLine($"{accessibility} partial {keyword} {_currentContext.TypeName}\n{{");
                    GenerateSource(_currentContext, _stringBuilder);

                    _stringBuilder.AppendLine("}");
                }
            }

            context.AddSource(GetHintName(), SourceText.From(_stringBuilder.ToString(), Encoding.UTF8));
        }

        public void Execute(GeneratorExecutionContext context)
        {
            OnBeforeExecute(context);

            SyntaxReceiver = (TSyntaxReceiver)context.SyntaxReceiver;

            _currentContext.GeneratorExecutionContext = context;

            foreach (var node in SyntaxReceiver.GetTypes())
                ExecuteForType(node, context);
        }
    }
}
