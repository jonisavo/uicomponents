using UIComponents.Roslyn.Generation.Generators.AssetLoad;
using UIComponents.Roslyn.Generation.Tests.Utilities;

namespace UIComponents.Roslyn.Generation.Tests
{
    [UsesVerify]
    public class LayoutAugmentGeneratorSnapshotTests
    {
        [Fact]
        public Task It_Generates_Layout_Call()
        {
            var source = @"
using UIComponents;

[Layout(""Components/LayoutTestComponent"")]
public partial class LayoutTestComponent : UIComponent {}
";
            return GeneratorTester.Verify<LayoutAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Does_Not_Generate_A_Call_For_Non_UIComponent_Class()
        {
            var source = @"
using UIComponents;

[Layout(""Components/LayoutTestComponent"")]
public partial class LayoutTestComponent {}
";
            return GeneratorTester.Verify<LayoutAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Does_Not_Generate_For_Abstract_Class()
        {
            var source = @"
using UIComponents;

[Layout(""Components/LayoutTestComponent"")]
public partial abstract class AbstractLayoutComponent : UIComponent {}
";
            return GeneratorTester.Verify<LayoutAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Takes_AssetPrefixAttribute_Into_Account()
        {
            var source = @"
using UIComponents;

[AssetPrefix(""UI/"")]
[Layout(""Components/LayoutTestComponent"")]
public partial class LayoutTestComponent : UIComponent {}
";
            return GeneratorTester.Verify<LayoutAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Uses_Base_Class_Attribute()
        {
            var source = @"
using UIComponents;

[Layout(""Components/BaseLayoutComponent"")]
public partial class BaseLayoutComponent : UIComponent {}

public partial class SuperclassLayoutComponent : BaseLayoutComponent {}

public partial class ThirdLayoutComponent : SuperclassLayoutComponent {}
";
            return GeneratorTester.Verify<LayoutAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Allows_Overriding_Base_Class_Attribute()
        {
            var source = @"
using UIComponents;

[Layout(""Components/BaseLayoutComponent"")]
public partial class BaseLayoutComponent : UIComponent {}

[Layout(""Components/OverriddenLayoutComponent"")]
public partial class OverriddenLayoutComponent : BaseLayoutComponent {}
";
            return GeneratorTester.Verify<LayoutAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Handles_Nested_Types()
        {
            var source = @"
using UIComponents;

public partial class ParentClass
{
    [Layout(""Components/FirstNestedComponent"")]
    public partial class FirstNestedComponent : UIComponent {}

    [Layout(""Components/SecondNestedComponent"")]
    private partial class SecondNestedComponent : UIComponent {}
}";
            return GeneratorTester.Verify<LayoutAugmentGenerator>(source);
        }

        [Fact]
        public Task Does_Not_Generate_If_Layout_Type_Is_Missing()
        {
            var source = @"
using UIComponents;

[Layout(""Components/LayoutTestComponent"")]
public partial class LayoutTestComponent : UIComponent {}
";

            return GeneratorTester.VerifyWithoutReferences<LayoutAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Uses_Class_Name_As_Convention_Path()
        {
            var source = @"
using UIComponents;

[Layout]
public partial class InventoryPanel : UIComponent {}
";
            return GeneratorTester.Verify<LayoutAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Uses_Class_Name_With_AssetPrefix()
        {
            var source = @"
using UIComponents;

[AssetPrefix(""UI/Components/"")]
[Layout]
public partial class InventoryPanel : UIComponent {}
";
            return GeneratorTester.Verify<LayoutAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Uses_Declaring_Class_Name_When_Inheriting_Parameterless_Layout()
        {
            var source = @"
using UIComponents;

[Layout]
public partial class BasePanel : UIComponent {}

public partial class ChildPanel : BasePanel {}
";
            return GeneratorTester.Verify<LayoutAugmentGenerator>(source);
        }

        [Fact]
        public Task Convention_Layout_Overrides_Explicit_Parent_Layout()
        {
            var source = @"
using UIComponents;

[Layout(""Components/BasePanel"")]
public partial class BasePanel : UIComponent {}

[Layout]
public partial class ChildPanel : BasePanel {}
";
            return GeneratorTester.Verify<LayoutAugmentGenerator>(source);
        }

        [Fact]
        public Task Explicit_Layout_Overrides_Convention_Parent_Layout()
        {
            var source = @"
using UIComponents;

[Layout]
public partial class BasePanel : UIComponent {}

[Layout(""Components/SpecificChild"")]
public partial class ChildPanel : BasePanel {}
";
            return GeneratorTester.Verify<LayoutAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Uses_AssetRoot_Instead_Of_AssetPrefix()
        {
            var source = @"
using UIComponents;

[AssetRoot(""UI/Components/"")]
[Layout]
public partial class InventoryPanel : UIComponent {}
";
            return GeneratorTester.Verify<LayoutAugmentGenerator>(source);
        }

        [Fact]
        public Task AssetRoot_Takes_Priority_Over_AssetPrefix()
        {
            var source = @"
using UIComponents;

[AssetPrefix(""Old/"")]
[AssetRoot(""New/"")]
[Layout(""MyLayout"")]
public partial class MyComponent : UIComponent {}
";
            return GeneratorTester.Verify<LayoutAugmentGenerator>(source);
        }
    }
}
