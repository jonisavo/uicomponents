using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using UIComponents.Roslyn.Generation.Utilities;

namespace UIComponents.Roslyn.Generation.Tests
{
    public class ParentClassTests
    {
        public class GetParentClasses
        {
            [Fact]
            public void ReturnsCorrectParentClass_WhenGivenClassDeclaration()
            {
                var code = @"
                public class Foo<T>
                {
                    private class Bar {}
                }";

                var syntaxTree = CSharpSyntaxTree.ParseText(code);
                var root = syntaxTree.GetCompilationUnitRoot();
                var classDeclaration = root.DescendantNodes().OfType<ClassDeclarationSyntax>().Last();

                var parentClass = ParentClass.GetParentClasses(classDeclaration);

                Assert.Equal("public", parentClass.Accessibility);
                Assert.Equal("class", parentClass.Keyword);
                Assert.Equal("Foo<T>", parentClass.Name);
                Assert.Equal("", parentClass.Constraints);
                Assert.Null(parentClass.Child);
            }


            [Fact]
            public void ReturnsCorrectParentClass_WhenGivenStructDeclaration()
            {
                var code = @"
                public struct Foo<T>
                {
                    private struct Bar {}
                }";

                var syntaxTree = CSharpSyntaxTree.ParseText(code);
                var root = syntaxTree.GetCompilationUnitRoot();
                var structDeclaration = root.DescendantNodes().OfType<StructDeclarationSyntax>().Last();

                var parentClass = ParentClass.GetParentClasses(structDeclaration);

                Assert.Equal("public", parentClass.Accessibility);
                Assert.Equal("struct", parentClass.Keyword);
                Assert.Equal("Foo<T>", parentClass.Name);
                Assert.Equal("", parentClass.Constraints);
                Assert.Null(parentClass.Child);
            }

            [Fact]
            public void ReturnsCorrectParentClass_WhenGivenRecordDeclaration()
            {
                var code = @"
                public record Foo<T>
                {
                    private record Bar {}
                }";

                var syntaxTree = CSharpSyntaxTree.ParseText(code);
                var root = syntaxTree.GetCompilationUnitRoot();
                var recordDeclaration = root.DescendantNodes().OfType<RecordDeclarationSyntax>().Last();

                var parentClass = ParentClass.GetParentClasses(recordDeclaration);

                Assert.Equal("public", parentClass.Accessibility);
                Assert.Equal("record", parentClass.Keyword);
                Assert.Equal("Foo<T>", parentClass.Name);
                Assert.Equal("", parentClass.Constraints);
                Assert.Null(parentClass.Child);
            }

            [Fact]
            public void ReturnsCorrectParentClass_WhenGivenNestedTypeDeclaration()
            {
                var code = @"
                public record Foo
                {
                    private struct Bar
                    {
                        public class Baz {}
                    }
                }";

                var syntaxTree = CSharpSyntaxTree.ParseText(code);
                var root = syntaxTree.GetCompilationUnitRoot();
                var nestedTypeDeclaration = root.DescendantNodes().OfType<ClassDeclarationSyntax>().Last();

                var parentClass = ParentClass.GetParentClasses(nestedTypeDeclaration);

                Assert.Equal("public", parentClass.Accessibility);
                Assert.Equal("record", parentClass.Keyword);
                Assert.Equal("Foo", parentClass.Name);
                Assert.Equal("", parentClass.Constraints);
                Assert.NotNull(parentClass.Child);
                Assert.Equal("Bar", parentClass.Child.Name);
                Assert.Equal("struct", parentClass.Child.Keyword);
            }

            [Fact]
            public void ReturnsNull_WhenTypeSyntaxIsNull()
            {
                var result = ParentClass.GetParentClasses(null);

                Assert.Null(result);
            }

            [Fact]
            public void ReturnsNull_WhenParentTypeSyntaxIsNull()
            {
                var typeSyntax = SyntaxFactory.ClassDeclaration("MyClass");

                var result = ParentClass.GetParentClasses(typeSyntax);

                Assert.Null(result);
            }
        }
    }
}
