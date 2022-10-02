using Microsoft.CodeAnalysis.CSharp;
using UIComponents.Roslyn.Generation.Generators.Traits;
using UIComponents.Roslyn.Generation.Tests.Utilities;

namespace UnityEngine
{
    public class Color {}

    namespace UIElements { }
}

public enum MyEnum
{
    FIRST,
    SECOND
}

namespace UIComponents.Roslyn.Generation.Tests
{
    [UsesVerify]
    public class UxmlTraitsAugmentGeneratorSnapshotTests : TraitTestFixture
    {
        [Fact]
        public Task Generates_String_Traits()
        {
            return Verify_Traits("string");
        }

        [Fact]
        public Task Generates_Float_Traits()
        {
            return Verify_Traits("float");
        }

        [Fact]
        public Task Generates_Double_Traits()
        {
            return Verify_Traits("double");
        }

        [Fact]
        public Task Generates_Int_Traits()
        {
            return Verify_Traits("int");
        }

        [Fact]
        public Task Generates_Long_Traits()
        {
            return Verify_Traits("long");
        }

        [Fact]
        public Task Generates_Bool_Traits()
        {
            return Verify_Traits("bool");
        }

        [Fact]
        public Task Generates_Color_Traits()
        {
            return Verify_Traits("Color", true);
        }

        [Fact]
        public Task Generates_Enum_Traits()
        {
            return Verify_Traits("MyEnum");
        }

        [Fact]
        public Task Does_Not_Generate_If_TraitAttribute_Type_IsMissing()
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(@"
using UIComponents.Experimental;

public class Test
{
    [Trait]
    public int Number;
}
");

            var compilation = CSharpCompilation.Create(
                assemblyName: "Tests",
                syntaxTrees: new[] { syntaxTree }
            );

            var generator = new UxmlTraitsAugmentGenerator();

            var driver = CSharpGeneratorDriver.Create(generator).RunGenerators(compilation);

            return Verify(driver).UseDirectory("Snapshots");
        }

        [Fact]
        public Task Works_On_Non_UIComponent_Type()
        {
            var source = @"
using UIComponents.Experimental;

public partial class NonUIComponentClass
{
    [Trait(Name = ""custom-trait-name"")]
    public int FieldTrait;

    [Trait(Name = ""my-property"")]
    public float PropertyTrait { get; set; }

    [Trait(Name = ""nope"")]
    public bool PropertyWithoutSetter { get; }
}";

            return GeneratorTester.Verify<UxmlTraitsAugmentGenerator>(source);
        }

        [Fact]
        public Task Generates_Enum_Trait_With_Internal_Enum()
        {
            var source = @"
using UIComponents;
using UIComponents.Experimental;

public partial class OwnEnumComponent : UIComponent
{
    public enum OwnEnum
    {
        A,
        B
    }

    [Trait]
    public OwnEnum FieldTrait;

    [Trait]
    public OwnEnum PropertyTrait { get; set; }

    [Trait]
    public OwnEnum PropertyWithoutSetter { get; }
}";

            return GeneratorTester.Verify<UxmlTraitsAugmentGenerator>(source);
        }

        [Fact]
        public Task Generates_In_Namespace()
        {
            var source = @"
using UIComponents;
using UIComponents.Experimental;

namespace Custom
{
    namespace Components
    {
        public partial class CustomNamespaceComponent : UIComponent
        {
            public enum OwnEnum
            {
                A,
                B
            }

            [Trait]
            public OwnEnum FieldTrait;

            [Trait]
            public OwnEnum PropertyTrait { get; set; }

            [Trait]
            public OwnEnum PropertyWithoutSetter { get; }
        }
    }
}";

            return GeneratorTester.Verify<UxmlTraitsAugmentGenerator>(source);
        }

        [Fact]
        public Task Allows_Specifying_Uxml_Name()
        {
            var source = @"
using UIComponents;
using UIComponents.Experimental;

public partial class CustomNamespaceComponent : UIComponent
{
    [Trait(Name = ""custom-trait-name"")]
    public int FieldTrait;

    [Trait(Name = ""my-property"")]
    public float PropertyTrait { get; set; }

    [Trait(Name = ""nope"")]
    public bool PropertyWithoutSetter { get; }
}";

            return GeneratorTester.Verify<UxmlTraitsAugmentGenerator>(source);
        }

        [Fact]
        public Task Handles_Subclass()
        {
            var source = @"
using UIComponents;
using UIComponents.Experimental;

public class MyComponent : UIComponent {}

public partial class MyComponentWithTraits : MyComponent
{
    [Trait(Name = ""double-value"")]
    public double Trait;
}";

            return GeneratorTester.Verify<UxmlTraitsAugmentGenerator>(source);
        }

        [Fact]
        public Task Allows_Setting_Default_Value()
        {
            var source = @"
using UIComponents;
using UIComponents.Experimental;

namespace Some.Place.Where.Enum.Is
{
    public enum TheEnum
    {
        VALUE_A,
        VALUE_B
    }
}

public partial class ComponentWithDefaultValueTraits : UIComponent
{
    [Trait(Name = ""description"", DefaultValue = ""Description not set."")]
    public string Description;

    [Trait(Name = ""lives"", DefaultValue = 3)]
    public int Lives;

    [Trait(Name = ""custom-value"", DefaultValue = Some.Place.Where.Enum.Is.TheEnum.VALUE_B)]
    public Some.Place.Where.Enum.Is.TheEnum MyValue;
}";

            return GeneratorTester.Verify<UxmlTraitsAugmentGenerator>(source);
        }

        [Fact]
        public Task Generates_Traits_For_Many_Classes()
        {
            var source = @"
using UIComponents;
using UIComponents.Experimental;

public partial class FirstTraitClass
{
    [Trait(Name = ""custom-trait-name"")]
    public int FieldTrait;

    [Trait(Name = ""my-property"")]
    public float PropertyTrait { get; set; }
}

public partial class SecondTraitClass
{
    [Trait(DefaultValue = true)]
    public bool Enabled;

    [Trait(Name = ""secret"")]
    public long SomeValue;
}

public partial class ThirdTraitClass
{
    [Trait]
    public string Name;
}";

            return GeneratorTester.Verify<UxmlTraitsAugmentGenerator>(source);
        }
    }
}