﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using UIComponents.Roslyn.Generation.SyntaxReceivers;
using UIComponents.Roslyn.Generation.Utilities;

namespace UIComponents.Roslyn.Generation.Generators
{
    /// <summary>
    /// A generator for augmenting code.
    /// </summary>
    /// <typeparam name="TSyntaxReceiver">Used syntax receiver</typeparam>
    public abstract class AugmentGenerator<TSyntaxReceiver> : ISourceGenerator
        where TSyntaxReceiver : ISyntaxReceiverWithClasses, new()
    {
        protected TSyntaxReceiver SyntaxReceiver { get; private set; }

        private AugmentGenerationContext _currentContext
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

        protected virtual void BuildUsingStatements(StringBuilder stringBuilder)
        {
            stringBuilder
                .AppendLine("using System.CodeDom.Compiler;")
                .AppendLine("using UnityEngine.UIElements;")
                .AppendLine();
        }

        private void ExecuteForClass(ClassDeclarationSyntax node, GeneratorExecutionContext context)
        {
            _stringBuilder.Clear();
            _currentContext.ClassSyntax = node;
            _currentContext.ClassSemanticModel = context.Compilation.GetSemanticModel(node.SyntaxTree);
            _currentContext.CurrentTypeNamespace = RoslynUtilities.GetTypeNamespace(node);
            _currentContext.TypeName = RoslynUtilities.GetTypeName(node);
            _currentParentClass = ParentClass.GetParentClasses(_currentContext.ClassSyntax);

            _currentContext.CurrentTypeSymbol =
                _currentContext.ClassSemanticModel.GetDeclaredSymbol(node) as INamedTypeSymbol;

            if (!ShouldGenerateSource(_currentContext))
                return;

            _stringBuilder.AppendLine(@"// <auto-generated>
// This file has been generated automatically by UIComponents.Roslyn.
// Do not attempt to modify it. Any changes will be overridden during compilation.
// </auto-generated>
");

            BuildUsingStatements(_stringBuilder);

            using (new WithinNamespaceScope(_currentContext.CurrentTypeNamespace, _stringBuilder))
            {
                using (new WithinParentClassScope(_currentParentClass, _stringBuilder))
                {
                    var accessibility = _currentContext.CurrentTypeSymbol.DeclaredAccessibility
                        .ToString()
                        .ToLower();

                    _stringBuilder.AppendLine($"{accessibility} partial class {_currentContext.TypeName}\n{{");
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

            foreach (var node in SyntaxReceiver.GetClasses())
                ExecuteForClass(node, context);
        }
    }
}
