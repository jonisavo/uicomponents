using UIComponents.Roslyn.Generation.Generators.AssetLoad;
using UIComponents.Roslyn.Generation.Tests.Utilities;

namespace UIComponents.Roslyn.Generation.Tests
{
    [UsesVerify]
    public class StylesheetAugmentGeneratorSnapshotTests
    {
        [Fact]
        public Task It_Generates_Stylesheet_Call()
        {
            var source = @"
using UIComponents;
using UIComponents.Experimental;

[Stylesheet(""Components/StylesheetTestComponentStyle"")]
public partial class StylesheetTestComponent : UIComponent {}
";
            return GeneratorTester.Verify<StylesheetAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Does_Not_Generate_A_Call_For_Non_UIComponent_Class()
        {
            var source = @"
using UIComponents.Experimental;

[Stylesheet(""Components/StylesheetTestComponentStyle"")]
public partial class StylesheetTestComponent {}
";
            return GeneratorTester.Verify<StylesheetAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Takes_AssetPathAttribute_Into_Account()
        {
            var source = @"
using UIComponents;
using UIComponents.Experimental;

[AssetPath(""UI/"")]
[Stylesheet(""Components/StylesheetTestComponentStyle"")]
public partial class StylesheetTestComponent : UIComponent {}
";
            return GeneratorTester.Verify<StylesheetAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Inherits_Base_Class_Stylesheets()
        {
            var source = @"
using UIComponents;
using UIComponents.Experimental;

[Stylesheet(""Components/BaseStylesheet"")]
public abstract partial class BaseStylesheetComponent : UIComponent {}

[Stylesheet(""Components/StylesheetOne"")]
[Stylesheet(""Components/StylesheetTwo"")]
public partial class ConcreteStylesheetComponent : BaseStylesheetComponent {}

[Stylesheet(""Components/StylesheetThree"")]
public partial class ThirdStylesheetComponent : ConcreteStylesheetComponent {}
";
            return GeneratorTester.Verify<StylesheetAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Handles_Nested_Types()
        {
            var source = @"
using UIComponents;
using UIComponents.Experimental;

public partial class ParentClass
{
    [Stylesheet(""Components/FirstNestedStyle"")]
    public partial class FirstNestedComponent : UIComponent {}

    [Stylesheet(""Components/SecondNestedStyle"")]
    private partial class SecondNestedComponent : UIComponent {}
}";
            return GeneratorTester.Verify<StylesheetAugmentGenerator>(source);
        }

        [Fact]
        public Task Does_Not_Generate_If_Stylesheet_Type_Is_Missing()
        {
            var source = @"
using UIComponents;
using UIComponents.Experimental;

[Stylesheet(""Components/StylesheetTestComponentStyle"")]
public partial class StylesheetTestComponent : UIComponent {}
";
            return GeneratorTester.VerifyWithoutReferences<StylesheetAugmentGenerator>(source);
        }
    }
}
