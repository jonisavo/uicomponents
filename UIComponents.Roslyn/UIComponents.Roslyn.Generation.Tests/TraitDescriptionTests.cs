using Microsoft.CodeAnalysis;
using NSubstitute;
using UIComponents.Roslyn.Generation.Generators.Uxml;

namespace UIComponents.Roslyn.Generation.Tests
{
    public class TraitDescriptionTests
    {
        private INamedTypeSymbol CreateSpecialTypeSymbol(SpecialType specialType)
        {
            var namedTypeSymbol = Substitute.For<INamedTypeSymbol>();

            namedTypeSymbol.SpecialType.Returns(specialType);
            namedTypeSymbol.Name.Returns(specialType.ToString());

            return namedTypeSymbol;
        }

        [Fact]
        public void Populates_Readonly_Variables()
        {
            var namedTypeSymbol = CreateSpecialTypeSymbol(SpecialType.System_Boolean);

            var traitDescription = new TraitDescription("MemberName", "member-name", namedTypeSymbol, "default");

            Assert.Equal("MemberName", traitDescription.ClassMemberName);
            Assert.Equal("m_MemberName", traitDescription.TraitMemberName);
            Assert.Equal("member-name", traitDescription.UxmlName);
            Assert.Equal("default", traitDescription.DefaultValue);
        }

        [Fact]
        public void Throws_Exception_On_Unknown_Type()
        {
            var namedTypeSymbol = CreateSpecialTypeSymbol(SpecialType.None);
            namedTypeSymbol.ToDisplayString().Returns("None");
            namedTypeSymbol.Name.Returns("FooBar");

            var ex = Assert.Throws<InvalidOperationException>(() => new TraitDescription("Foo", "foo", namedTypeSymbol));

            Assert.Equal("Unknown trait field type None", ex.Message);
        }
    }
}
