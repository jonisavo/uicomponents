using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using NSubstitute;
using UIComponents.Roslyn.Common.Utilities;

namespace UIComponents.Roslyn.Common.Tests
{
    public class RoslynUtilitiesTests
    {
        public static string TestSource = @"
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
}";

        public class GetTypeNamespace
        {
            [Fact]
            public void Returns_The_Namespace_String_Of_A_Syntax_Node()
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(TestSource);
                var nodes = syntaxTree.GetRoot().DescendantNodes();
                var classSyntax = nodes.OfType<ClassDeclarationSyntax>().First();

                Assert.Equal("Hello.World", RoslynUtilities.GetTypeNamespace(classSyntax));
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

                Assert.Equal("", RoslynUtilities.GetTypeNamespace(classSyntax));
            }

            [Fact]
            public void Returns_Empty_String_If_Syntax_Node_Is_Null()
            {
                Assert.Equal("", RoslynUtilities.GetTypeNamespace(null));
            }
        }

        public class GetCompilationUnitSyntax
        {
            [Fact]
            public void Returns_Compilation_Unit_Syntax()
            {
                var syntaxTree = CSharpSyntaxTree.ParseText(TestSource);
                var root = syntaxTree.GetRoot();
                var nodes = root.DescendantNodes();
                var classSyntax = nodes.OfType<ClassDeclarationSyntax>().First();

                var compilationUnitSyntax = RoslynUtilities.GetCompilationUnitSyntax(classSyntax);

                Assert.Equal(root, compilationUnitSyntax);
            }

            [Fact]
            public void Throws_Exception_If_No_Compilation_Unit_Syntax_Exists()
            {
                var syntaxTree = CSharpSyntaxTree.ParseText("");
                var root = syntaxTree.GetRoot();

                Assert.Throws<InvalidOperationException>(() => RoslynUtilities.GetCompilationUnitSyntax(root));
            }

            [Fact]
            public void Throws_Exception_If_Null_Node_Is_Given()
            {
                Assert.Throws<ArgumentNullException>(() => RoslynUtilities.GetCompilationUnitSyntax(null));
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

                var expectedBaseTypeSyntax = nodes.OfType<BaseTypeDeclarationSyntax>().First();
                var baseTypeSyntax = RoslynUtilities.GetBaseTypeSyntax(propertyBlockSyntax);

                Assert.Equal(expectedBaseTypeSyntax, baseTypeSyntax);
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

                var baseTypeSyntax = RoslynUtilities.GetBaseTypeSyntax(attributeSyntax);

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

                var typeName = RoslynUtilities.GetTypeName(propertyBlockSyntax);

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

                var typeName = RoslynUtilities.GetTypeName(attributeSyntax);

                Assert.Equal("", typeName);
            }
        }

        public class GetMemberType
        {
            [Fact]
            public void Returns_Null_If_Member_Is_Not_Field_Or_Property()
            {
                var typeSymbol = Substitute.For<ITypeSymbol>();
                var memberType = RoslynUtilities.GetMemberType(typeSymbol);

                Assert.Null(memberType);
            }
        }

        public class GetTypeNameForNamespace
        {
            [Fact]
            public void Returns_The_Type_Name_With_Shared_Portions_Removed()
            {
                var twoPartTypeSymbol = Substitute.For<ITypeSymbol>();
                twoPartTypeSymbol
                    .ToDisplayString()
                    .Returns("MyLibrary.Service");

                var fivePartTypeSymbol = Substitute.For<ITypeSymbol>();
                fivePartTypeSymbol
                    .ToDisplayString()
                    .Returns("MyLibrary.Services.Area.Internal.Helper");

                var twoPartName =
                    RoslynUtilities.GetTypeNameForNamespace(twoPartTypeSymbol, "MyLibrary");

                Assert.Equal("Service", twoPartName);

                var fivePartName =
                    RoslynUtilities.GetTypeNameForNamespace(fivePartTypeSymbol, "MyLibrary.Services.Area");

                Assert.Equal("Internal.Helper", fivePartName);
            }

            [Fact]
            public void Returns_Type_Name_If_It_Has_One_Part()
            {
                var onePartTypeSymbol = Substitute.For<ITypeSymbol>();
                onePartTypeSymbol
                    .ToDisplayString()
                    .Returns("MyService");

                var onePartName =
                    RoslynUtilities.GetTypeNameForNamespace(onePartTypeSymbol, "MyService");

                Assert.Equal("MyService", onePartName);
            }

            [Fact]
            public void Returns_Type_Name_If_There_Is_No_Match()
            {
                var typeSymbol = Substitute.For<ITypeSymbol>();
                typeSymbol
                    .ToDisplayString()
                    .Returns("MyLibrary.Services.MyService");
                var typeSymbolWithoutGlobal = Substitute.For<ITypeSymbol>();
                typeSymbolWithoutGlobal
                    .ToDisplayString()
                    .Returns("MyLibrary.Services.MyService");

                var newTypeName =
                    RoslynUtilities.GetTypeNameForNamespace(typeSymbol, "OtherLibrary.Services");
                var newTypeNameWithoutGlobal =
                    RoslynUtilities.GetTypeNameForNamespace(typeSymbolWithoutGlobal, "OtherLibrary.Services");

                Assert.Equal("MyLibrary.Services.MyService", newTypeName);
                Assert.Equal("MyLibrary.Services.MyService", newTypeNameWithoutGlobal);
            }

            [Fact]
            public void Returns_Type_Name_If_It_Belongs_In_UIComponents()
            {
                var typeSymbol = Substitute.For<ITypeSymbol>();
                typeSymbol
                    .ToDisplayString()
                    .Returns("UIComponents.Addressables.AddressableAssetResolver");

                var newTypeName =
                    RoslynUtilities.GetTypeNameForNamespace(typeSymbol, "UIComponents.Samples.Addressables.Stuff");

                Assert.Equal("UIComponents.Addressables.AddressableAssetResolver", newTypeName);
            }
        }
    }
}
