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
using UIComponents.Experimental;

[Layout(""Components/LayoutTestComponent"")]
public partial class LayoutTestComponent : UIComponent {}
";
            return GeneratorTester.Verify<LayoutAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Does_Not_Generate_A_Call_For_Non_UIComponent_Class()
        {
            var source = @"
using UIComponents.Experimental;

[Layout(""Components/LayoutTestComponent"")]
public partial class LayoutTestComponent {}
";
            return GeneratorTester.Verify<LayoutAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Takes_AssetPathAttribute_Into_Account()
        {
            var source = @"
using UIComponents;
using UIComponents.Experimental;

[AssetPath(""UI/"")]
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
using UIComponents.Experimental;

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
using UIComponents.Experimental;

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
using UIComponents.Experimental;

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
using UIComponents.Experimental;

[Layout(""Components/LayoutTestComponent"")]
public partial class LayoutTestComponent : UIComponent {}
";

            return GeneratorTester.VerifyWithoutReferences<LayoutAugmentGenerator>(source);
        }
    }
}
