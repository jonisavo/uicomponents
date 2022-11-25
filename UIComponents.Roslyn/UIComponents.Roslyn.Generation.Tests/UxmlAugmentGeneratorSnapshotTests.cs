using System.Diagnostics.CodeAnalysis;
using UIComponents.Roslyn.Generation.Generators.Uxml;
using UIComponents.Roslyn.Generation.Tests.Utilities;

namespace UnityEngine
{
    [ExcludeFromCodeCoverage]
    public class Color
    {
        public static readonly Color red = new Color();
    }

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
    public class UxmlAugmentGeneratorSnapshotTests : TraitTestFixture
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
            var source = @"
using UIComponents;

public class Test
{
    [UxmlTrait]
    public int Number;
}
";
            return GeneratorTester.VerifyWithoutReferences<UxmlAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Uses_Initializer_As_Default_Trait_Value()
        {
            var source = @"
using UIComponents;
using UnityEngine;

public class Test
{
    [UxmlTrait]
    public int Number = 0;

    [UxmlTrait]
    public string MyMessage { get; set; } = ""Hello world!"";

    [UxmlTrait]
    public Color color = Color.red;
}
";
            return GeneratorTester.Verify<UxmlAugmentGenerator>(source);
        }

        [Fact]
        public Task Does_Not_Generate_UxmlFactory_If_Name_Is_Missing()
        {
            var source = @"
using UIComponents;

[UxmlName()]
public class Test {}
";
            return GeneratorTester.Verify<UxmlAugmentGenerator>(source);
        }

        [Fact]
        public Task Works_On_Non_UIComponent_Type()
        {
            var source = @"
using UIComponents;

public partial class NonUIComponentClass
{
    [UxmlTrait(Name = ""custom-trait-name"")]
    public int FieldTrait;

    [UxmlTrait(Name = ""my-property"")]
    public float PropertyTrait { get; set; }

    [UxmlTrait(Name = ""nope"")]
    public bool PropertyWithoutSetter { get; }
}";

            return GeneratorTester.Verify<UxmlAugmentGenerator>(source);
        }

        [Fact]
        public Task Generates_Enum_Trait_With_Internal_Enum()
        {
            var source = @"
using UIComponents;

public partial class OwnEnumComponent : UIComponent
{
    public enum OwnEnum
    {
        A,
        B
    }

    [UxmlTrait]
    public OwnEnum FieldTrait;

    [UxmlTrait]
    public OwnEnum PropertyTrait { get; set; }

    [UxmlTrait]
    public OwnEnum PropertyWithoutSetter { get; }
}";

            return GeneratorTester.Verify<UxmlAugmentGenerator>(source);
        }

        [Fact]
        public Task Generates_In_Namespace()
        {
            var source = @"
using UIComponents;

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

            [UxmlTrait]
            public OwnEnum FieldTrait;

            [UxmlTrait]
            public OwnEnum PropertyTrait { get; set; }

            [UxmlTrait]
            public OwnEnum PropertyWithoutSetter { get; }
        }
    }
}";

            return GeneratorTester.Verify<UxmlAugmentGenerator>(source);
        }

        [Fact]
        public Task Allows_Specifying_Uxml_Name()
        {
            var source = @"
using UIComponents;

public partial class CustomNamespaceComponent : UIComponent
{
    [UxmlTrait(Name = ""custom-trait-name"")]
    public int FieldTrait;

    [UxmlTrait(Name = ""my-property"")]
    public float PropertyTrait { get; set; }

    [UxmlTrait(Name = ""nope"")]
    public bool PropertyWithoutSetter { get; }
}";

            return GeneratorTester.Verify<UxmlAugmentGenerator>(source);
        }

        [Fact]
        public Task Handles_Subclass()
        {
            var source = @"
using UIComponents;

public class MyComponent : UIComponent {}

public partial class MyComponentWithTraits : MyComponent
{
    [UxmlTrait(Name = ""double-value"")]
    public double Trait;
}";

            return GeneratorTester.Verify<UxmlAugmentGenerator>(source);
        }

        [Fact]
        public Task Allows_Setting_Default_Value()
        {
            var source = @"
using UIComponents;

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
    [UxmlTrait(Name = ""description"", DefaultValue = ""Description not set."")]
    public string Description;

    [UxmlTrait(Name = ""lives"", DefaultValue = 3)]
    public int Lives;

    [UxmlTrait(Name = ""custom-value"", DefaultValue = Some.Place.Where.Enum.Is.TheEnum.VALUE_B)]
    public Some.Place.Where.Enum.Is.TheEnum MyValue;
}";

            return GeneratorTester.Verify<UxmlAugmentGenerator>(source);
        }

        [Fact]
        public Task Generates_Traits_For_Many_Classes()
        {
            var source = @"
using UIComponents;

public partial class FirstTraitClass
{
    [UxmlTrait(Name = ""custom-trait-name"")]
    public int FieldTrait;

    [UxmlTrait(Name = ""my-property"")]
    public float PropertyTrait { get; set; }
}

public partial class SecondTraitClass
{
    [UxmlTrait(DefaultValue = true)]
    public bool Enabled;

    [UxmlTrait(Name = ""secret"")]
    public long SomeValue;
}

public partial class ThirdTraitClass
{
    [UxmlTrait]
    public string Name;
}";

            return GeneratorTester.Verify<UxmlAugmentGenerator>(source);
        }

        [Fact]
        public Task Generates_UxmlFactory_With_UxmlName()
        {
            var source = @"
using UIComponents;

[UxmlName(""MyUxmlNameAttribute"")]
public partial class MyUxmlNameAttributeComponent : UIComponent
{
  
}
";
            return GeneratorTester.Verify<UxmlAugmentGenerator>(source);
        }

        [Fact]
        public Task Generates_Both_Traits_And_UxmlFactory()
        {
            var source = @"
using UIComponents;

[UxmlName(""AwesomeUxmlName"")]
public partial class ComponentWithUxmlNameAndTraits : UIComponent
{
    [UxmlTrait(DefaultValue = true)]
    public bool Value;
}
";
            return GeneratorTester.Verify<UxmlAugmentGenerator>(source);
        }

        [Fact]
        public Task Handles_Long_Member_Name_For_Trait()
        {
            var source = @"
using UIComponents;

public partial class LongTraitNameComponent : UIComponent
{
    [UxmlTrait]
    public int HereIsALongMemberNameWithALotOfComplexity123Test_Hello___WorldA;
}
";
            return GeneratorTester.Verify<UxmlAugmentGenerator>(source);
        }
    }
}
