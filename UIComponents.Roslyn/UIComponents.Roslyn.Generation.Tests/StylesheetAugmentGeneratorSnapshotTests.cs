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

[Stylesheet(""Components/StylesheetTestComponentStyle"")]
public partial class StylesheetTestComponent : UIComponent {}
";
            return GeneratorTester.Verify<StylesheetAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Does_Not_Generate_A_Call_For_Non_UIComponent_Class()
        {
            var source = @"
using UIComponents;

[Stylesheet(""Components/StylesheetTestComponentStyle"")]
public partial class StylesheetTestComponent {}
";
            return GeneratorTester.Verify<StylesheetAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Takes_AssetPrefixAttribute_Into_Account()
        {
            var source = @"
using UIComponents;

[AssetPrefix(""UI/"")]
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

[Stylesheet(""Components/StylesheetTestComponentStyle"")]
public partial class StylesheetTestComponent : UIComponent {}
";
            return GeneratorTester.VerifyWithoutReferences<StylesheetAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Uses_Class_Name_As_Convention_Path()
        {
            var source = @"
using UIComponents;

[Stylesheet]
public partial class InventoryPanel : UIComponent {}
";
            return GeneratorTester.Verify<StylesheetAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Handles_Mix_Of_Convention_And_Explicit_Stylesheets()
        {
            var source = @"
using UIComponents;

[Stylesheet]
[Stylesheet(""Shared/Common"")]
public partial class InventoryPanel : UIComponent {}
";
            return GeneratorTester.Verify<StylesheetAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Resolves_Inherited_Convention_Stylesheet_To_Declaring_Class_Name()
        {
            var source = @"
using UIComponents;

[Stylesheet]
public partial class BaseComponent : UIComponent {}

[Stylesheet]
public partial class ChildComponent : BaseComponent {}
";
            return GeneratorTester.Verify<StylesheetAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Generates_SharedStylesheet_Loading()
        {
            var source = @"
using UIComponents;

[Stylesheet(""Components/MyStyle"")]
[SharedStylesheet(""Shared/Common"")]
public partial class MyComponent : UIComponent {}
";
            return GeneratorTester.Verify<StylesheetAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Inherits_SharedStylesheets_From_Base_Class()
        {
            var source = @"
using UIComponents;

[SharedStylesheet(""Shared/Base"")]
public partial class BaseComponent : UIComponent {}

[Stylesheet(""Components/ChildStyle"")]
[SharedStylesheet(""Shared/Child"")]
public partial class ChildComponent : BaseComponent {}
";
            return GeneratorTester.Verify<StylesheetAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Combines_Convention_Stylesheet_With_SharedStylesheet()
        {
            var source = @"
using UIComponents;

[Stylesheet]
[SharedStylesheet(""Shared/Common"")]
public partial class InventoryPanel : UIComponent {}
";
            return GeneratorTester.Verify<StylesheetAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Uses_AssetRoot_For_Stylesheets()
        {
            var source = @"
using UIComponents;

[AssetRoot(""UI/Components/"")]
[Stylesheet]
public partial class InventoryPanel : UIComponent {}
";
            return GeneratorTester.Verify<StylesheetAugmentGenerator>(source);
        }
    }
}
