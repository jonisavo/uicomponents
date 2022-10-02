using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UIComponents.Roslyn.Generation.Utilities;

namespace UIComponents.Roslyn.Generation.Tests
{
    public class CodeAnalyzingUtilitiesTests
    {
        public class GetTypeNamespace
        {
            [Fact]
            public void Returns_The_Namespace_String_Of_A_Syntax_Node()
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(@"
namespace Hello
{
    namespace World
    {
        public class A_Thing
        {
            public int Number;
            public float OtherNumber;
        }
    }
}");
                var nodes = syntaxTree.GetRoot().DescendantNodes();
                var classSyntax = nodes.OfType<ClassDeclarationSyntax>().First();

                Assert.Equal("Hello.World", CodeAnalyzingUtilities.GetTypeNamespace(classSyntax));
            }

            [Fact]
            public void Returns_Empty_String_In_Case_Of_No_Namespace()
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(@"
public class A_Thing
{
    public int Number;
    public float OtherNumber;
}");

                var nodes = syntaxTree.GetRoot().DescendantNodes();
                var classSyntax =
                    nodes.OfType<ClassDeclarationSyntax>().First();

                Assert.Equal("", CodeAnalyzingUtilities.GetTypeNamespace(classSyntax));
            }

            [Fact]
            public void Returns_Empty_String_If_Syntax_Node_Is_Null()
            {
                Assert.Equal("", CodeAnalyzingUtilities.GetTypeNamespace(null));
            }
        }

        public class GetBaseTypeSyntax
        {
            [Fact]
            public void Returns_Parent_Base_Type_Syntax()
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(@"
public class MyAttribute : Attribute
{
    public bool Setting
    {
        get => _setting;
        set => _setting = value;
    }

    private bool _setting;
}");
                var nodes = syntaxTree.GetRoot().DescendantNodes();
                var propertyBlockSyntax =
                    nodes.OfType<AccessorDeclarationSyntax>().First();

                var excpectedBaseTypeSyntax = nodes.OfType<BaseTypeDeclarationSyntax>().First();
                var baseTypeSyntax = CodeAnalyzingUtilities.GetBaseTypeSyntax(propertyBlockSyntax);

                Assert.Equal(excpectedBaseTypeSyntax, baseTypeSyntax);
            }

            [Fact]
            public void Returns_Null_If_No_Parent_Base_Type_Syntax_Exists()
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(@"
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo(""Foo.Bar"")]
");
                var nodes = syntaxTree.GetRoot().DescendantNodes();
                var attributeSyntax =
                    nodes.OfType<AttributeSyntax>().First();

                var baseTypeSyntax = CodeAnalyzingUtilities.GetBaseTypeSyntax(attributeSyntax);

                Assert.Null(baseTypeSyntax);
            }
        }

        public class GetTypeName
        {
            [Fact]
            public void Returns_Parent_Type_Name()
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(@"
public class MyAttribute
{
    public bool Setting
    {
        get => _setting;
        set => _setting = value;
    }

    private bool _setting;
}");
                var nodes = syntaxTree.GetRoot().DescendantNodes();
                var propertyBlockSyntax =
                    nodes.OfType<AccessorDeclarationSyntax>().First();

                var typeName = CodeAnalyzingUtilities.GetTypeName(propertyBlockSyntax);

                Assert.Equal("MyAttribute", typeName);
            }

            [Fact]
            public void Returns_Empty_String_If_There_Is_No_Parent_Type()
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(@"
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo(""Foo.Bar"")]
}");
                var nodes = syntaxTree.GetRoot().DescendantNodes();
                var attributeSyntax =
                    nodes.OfType<AttributeSyntax>().First();

                var typeName = CodeAnalyzingUtilities.GetTypeName(attributeSyntax);

                Assert.Equal("", typeName);
            }
        }
    }
}
