using UIComponents.Roslyn.Generation.Generators.DependencyInjection;
using UIComponents.Roslyn.Generation.Tests.Utilities;

namespace UIComponents.Roslyn.Generation.Tests
{
    [UsesVerify]
    public class ProvideAugmentGeneratorSnapshotTests
    {
        [Fact]
        public Task It_Generates_Provide_Calls()
        {
            var source = @"
using UIComponents;

public interface IDependency {}

public class Dependency : IDependency {}

public partial class BasicProvideComponent : UIComponent
{
    [Provide]
    public IDependency Dependency;

    [Provide(CastFrom = typeof(IDependency))]
    public Dependency ConcreteDependency;
}
";
            return GeneratorTester.Verify<ProvideAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Can_Be_Used_On_Non_UIComponent_Class()
        {
            var source = @"
using UIComponents;

public interface IDependency {}

public class Dependency : IDependency {}

public partial class MyClass
{
    [Provide]
    public IDependency Dependency;

    [Provide(CastFrom = typeof(IDependency))]
    public Dependency ConcreteDependency;
}
";
            return GeneratorTester.Verify<ProvideAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Handles_Subclasses()
        {
            var source = @"
using UIComponents;

public interface IDependency {}

public interface IAnotherDependency {}

public class Dependency : IDependency {}

public class AnotherDependency : IAnotherDependency {}

public partial class BaseProvideComponent : UIComponent
{
    [Provide]
    public IDependency Dependency;

    [Provide(CastFrom = typeof(IDependency))]
    public Dependency ConcreteDependency;
}

public partial class SubclassProvideComponent : BaseProvideComponent
{
    [Provide]
    public IAnotherDependency AnotherDependency;
}

public partial class SecondSubclassProvideComponent : SubclassProvideComponent
{
    [Provide(CastFrom = typeof(IAnotherDependency))]
    public AnotherDependency ThirdDependency;
}";
            return GeneratorTester.Verify<ProvideAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Handles_Namespaces_And_Nested_Types()
        {
            var source = @"
using UIComponents;

namespace Dependencies
{
    public interface IDependency {}

    public class Dependency : IDependency {}
}

namespace Components
{
    public partial class ParentClass
    {
        public interface IOtherDependency {}

        private partial class NestedProvideComponent : UIComponent
        {
            [Provide]
            public Dependencies.IDependency Dependency;

            [Provide(CastFrom = typeof(Dependencies.IDependency))]
            public Dependencies.Dependency ConcreteDependency;

            [Provide]
            public IOtherDependency OtherDependency;
        }
    }
}";
            return GeneratorTester.Verify<ProvideAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Handles_Common_Namespaces()
        {
            var firstSource = @"
namespace MyLibrary.Core.Services
{
    public interface IService {}
}
";
            var secondSource = @"
using UIComponents;
using MyLibrary.Core.Services;

namespace MyLibrary.GUI
{
    public class GuiComponent
    {
        [Provide]
        public IService service;
    }
}
";
            return GeneratorTester.Verify<ProvideAugmentGenerator>(firstSource, secondSource);
        }

        [Fact]
        public Task It_Does_Not_Generate_For_Non_Interface_Or_Class_Fields()
        {
            var source = @"
using UIComponents;

public interface IDependency {}

public partial class InvalidProvideComponent : UIComponent
{
    [Provide]
    public IDependency Dependency;

    [Provide]
    public int InvalidIntDependency;

    [Provide]
    public float InvalidFloatDependency;
}
";
            return GeneratorTester.Verify<ProvideAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Does_Not_Generate_If_There_Are_No_Provide_Fields()
        {
            var source = @"
using UIComponents;

public interface IDependency {}

public partial class NoProvideComponent : UIComponent
{
    public IDependency Dependency;
}
";
            return GeneratorTester.Verify<ProvideAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Does_Not_Generate_If_ProvideAttribute_Type_Is_Missing()
        {
            var source = @"
namespace UIComponents
{
    public class UIComponent {}

    public class AssetPathAttribute {}
}

public interface IDependency {}

public partial class MissingProvideComponent : UIComponents.UIComponent
{
    [UIComponents.Provide]
    public IDependency Dependency;
}
";
            return GeneratorTester.VerifyWithoutReferences<ProvideAugmentGenerator>(source);
        }
    }
}
