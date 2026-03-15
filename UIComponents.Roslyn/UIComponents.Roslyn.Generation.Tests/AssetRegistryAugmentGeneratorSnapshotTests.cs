using UIComponents.Roslyn.Generation.Generators.AssetLoad;
using UIComponents.Roslyn.Generation.Tests.Utilities;

namespace UIComponents.Roslyn.Generation.Tests
{
    [UsesVerify]
    public class AssetRegistryAugmentGeneratorSnapshotTests
    {
        [Fact]
        public Task It_Generates_Registry_For_Component_With_Layout()
        {
            var source = @"
using UIComponents;

[Layout(""Components/MyComponent"")]
public partial class MyComponent : UIComponent {}
";
            return GeneratorTester.Verify<AssetRegistryAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Generates_Registry_For_Convention_Layout_And_Stylesheet()
        {
            var source = @"
using UIComponents;

[Layout]
[Stylesheet]
public partial class InventoryPanel : UIComponent {}
";
            return GeneratorTester.Verify<AssetRegistryAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Generates_Registry_For_Multiple_Components()
        {
            var source = @"
using UIComponents;

[Layout]
[Stylesheet]
public partial class PanelA : UIComponent {}

[Layout(""Custom/PanelB"")]
[Stylesheet(""Custom/PanelB.style"")]
[SharedStylesheet(""Shared/Common"")]
public partial class PanelB : UIComponent {}
";
            return GeneratorTester.Verify<AssetRegistryAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Does_Not_Generate_For_Abstract_Classes()
        {
            var source = @"
using UIComponents;

[Layout]
public abstract partial class BasePanel : UIComponent {}
";
            return GeneratorTester.Verify<AssetRegistryAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Does_Not_Generate_If_No_Assets()
        {
            var source = @"
using UIComponents;

public partial class EmptyComponent : UIComponent {}
";
            return GeneratorTester.Verify<AssetRegistryAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Includes_Inherited_Stylesheets()
        {
            var source = @"
using UIComponents;

[Stylesheet(""Shared/Base"")]
public abstract partial class BaseComponent : UIComponent {}

[Layout]
[Stylesheet]
public partial class ChildComponent : BaseComponent {}
";
            return GeneratorTester.Verify<AssetRegistryAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Skips_Private_Nested_Types()
        {
            var source = @"
using UIComponents;

public partial class OuterClass
{
    [Layout]
    private partial class PrivateComponent : UIComponent {}
}

[Layout]
public partial class PublicComponent : UIComponent {}
";
            return GeneratorTester.Verify<AssetRegistryAugmentGenerator>(source);
        }
    }
}
