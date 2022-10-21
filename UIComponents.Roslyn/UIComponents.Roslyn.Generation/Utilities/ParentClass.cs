using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text;

namespace UIComponents.Roslyn.Generation.Utilities
{
    internal class ParentClass
    {
        public ParentClass(string accessibility, string keyword, string name, string constraints, ParentClass child)
        {
            Accessibility = accessibility;
            Keyword = keyword;
            Name = name;
            Constraints = constraints;
            Child = child;
        }

        public ParentClass Child { get; }

        public string Accessibility { get; }

        public string Keyword { get; }

        public string Name { get; }

        public string Constraints { get; }

        // https://andrewlock.net/creating-a-source-generator-part-5-finding-a-type-declarations-namespace-and-type-hierarchy/
        public static ParentClass GetParentClasses(BaseTypeDeclarationSyntax typeSyntax)
        {
            var parentSyntax = typeSyntax.Parent as TypeDeclarationSyntax;
            ParentClass parentClass = null;

            while (parentSyntax != null && IsAllowedKind(parentSyntax.Kind()))
            {
                parentClass = new ParentClass(
                    accessibility: GetModifierFromDeclaration(parentSyntax),
                    keyword: parentSyntax.Keyword.ValueText,
                    name: parentSyntax.Identifier.ToString() + parentSyntax.TypeParameterList,
                    constraints: parentSyntax.ConstraintClauses.ToString(),
                    child: parentClass);

                parentSyntax = parentSyntax.Parent as TypeDeclarationSyntax;
            }

            return parentClass;

        }

        private static string GetModifierFromDeclaration(BaseTypeDeclarationSyntax syntax)
        {
            var modifierString = syntax.Modifiers.ToFullString();

            if (modifierString.Contains("private"))
                return "private";
            else if (modifierString.Contains("protected"))
                return "protected";
            else if (modifierString.Contains("internal"))
                return "internal";

            return "public";
        }

        private static bool IsAllowedKind(SyntaxKind kind) =>
            kind == SyntaxKind.ClassDeclaration ||
            kind == SyntaxKind.StructDeclaration ||
            kind == SyntaxKind.RecordDeclaration;

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            var current = this;

            while (current != null)
            {
                stringBuilder.Append(current.Name);
                current = current.Child;
                if (current != null)
                    stringBuilder.Append('.');
            }

            return stringBuilder.ToString();
        }
    }
}
