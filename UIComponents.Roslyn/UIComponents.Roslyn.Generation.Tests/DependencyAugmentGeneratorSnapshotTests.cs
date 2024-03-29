﻿using UIComponents.Roslyn.Generation.Generators.DependencyInjection;
using UIComponents.Roslyn.Generation.Tests.Utilities;

namespace UIComponents.Roslyn.Generation.Tests
{
	[UsesVerify]
	public class DependencyAugmentGeneratorSnapshotTests
	{
		[Fact]
		public Task It_Generates_Dependencies_From_Type_Declaration()
		{
			var source = @"
using UIComponents;
using UIComponents.DependencyInjection;

public interface IMyDependency {}
public class MyDependency : IMyDependency {}

public interface ISecondDependency {}
public class SecondDependency : ISecondDependency {}

[Dependency(typeof(IMyDependency), provide: typeof(MyDependency))]
[Dependency(typeof(ISecondDependency), provide: typeof(SecondDependency), Scope.Transient)]
public partial class ConsumerClass : IDependencyConsumer {}

[Dependency(typeof(IMyDependency), provide: typeof(MyDependency))]
[Dependency(typeof(ISecondDependency), provide: typeof(SecondDependency), Scope.Transient)]
public partial struct ConsumerStruct : IDependencyConsumer {}
";
			return GeneratorTester.Verify<DependencyAugmentGenerator>(source);
		}

		[Fact]
		public Task It_Generates_Dependencies_From_Assembly_Declaration()
		{
            var source = @"
using UIComponents;
using UIComponents.DependencyInjection;

[assembly: Dependency(typeof(IMyDependency), provide: typeof(MyDependency))]
[assembly: Dependency(typeof(ISecondDependency), provide: typeof(SecondDependency), Scope.Transient)]

public interface IMyDependency {}
public class MyDependency : IMyDependency {}

public interface ISecondDependency {}
public class SecondDependency : ISecondDependency {}

public partial class ConsumerClass : IDependencyConsumer {}

public partial struct ConsumerStruct : IDependencyConsumer {}
";
            return GeneratorTester.Verify<DependencyAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Allows_Overriding_Assembly_Declarations_With_Class_Declarations()
        {
            var source = @"
using UIComponents;
using UIComponents.DependencyInjection;

public interface IMyDependency {}
public class MyDependency : IMyDependency {}

[assembly: Dependency(typeof(IMyDependency), provide: typeof(MyDependency))]

public class ConsumerDependency : IMyDependency {}

[Dependency(typeof(IMyDependency), provide: typeof(ConsumerDependency))]
public partial class ConsumerClass : IDependencyConsumer {}
";
            return GeneratorTester.Verify<DependencyAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Allows_Overriding_Scope()
        {
            var source = @"
using UIComponents;
using UIComponents.DependencyInjection;

public interface IMyDependency {}
public class MyDependency : IMyDependency {}

[assembly: Dependency(typeof(IMyDependency), provide: typeof(MyDependency))]

[Dependency(typeof(IMyDependency), provide: typeof(MyDependency), Scope.Transient)]
public partial class ConsumerClass : IDependencyConsumer {}
";
            return GeneratorTester.Verify<DependencyAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Allows_For_Dependency_Inheritance()
        {
            var source = @"
using UIComponents;
using UIComponents.DependencyInjection;

[assembly: Dependency(typeof(IMyDependency), provide: typeof(MyDependency))]

public interface IMyDependency {}
public class MyDependency : IMyDependency {}

public interface ISecondDependency {}
public class SecondDependency : ISecondDependency {}

[Dependency(typeof(ISecondDependency), provide: typeof(SecondDependency))]
public partial class ConsumerClass : IDependencyConsumer {}

public interface IThirdDependency {}
public class ThirdDependency : IThirdDependency {}

[Dependency(typeof(IThirdDependency), provide: typeof(ThirdDependency))]
public partial class SecondConsumerClass : ConsumerClass {}
";
            return GeneratorTester.Verify<DependencyAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Allows_Overriding_Inherited_Dependencies()
        {
            var source = @"
using UIComponents;
using UIComponents.DependencyInjection;

[assembly: Dependency(typeof(IMyDependency), provide: typeof(MyDependency))]

public interface IMyDependency {}
public class MyDependency : IMyDependency {}

public interface ISecondDependency {}
public class SecondDependency : ISecondDependency {}

[Dependency(typeof(ISecondDependency), provide: typeof(SecondDependency))]
public abstract class ConsumerClass : IDependencyConsumer {}

public class OverriddenMyDependency : IMyDependency {}
public class OverriddenSecondDependency : ISecondDependency {}

[Dependency(typeof(IMyDependency), provide: typeof(OverriddenMyDependency))]
[Dependency(typeof(ISecondDependency), provide: typeof(OverriddenSecondDependency))]
public partial class SecondConsumerClass : ConsumerClass {}
";
            return GeneratorTester.Verify<DependencyAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Handles_Concrete_Dependencies()
        {
            var source = @"
using UIComponents;
using UIComponents.DependencyInjection;

public class MyDependency {}

[Dependency(typeof(MyDependency))]
public partial class ConsumerComponent : UIComponent {}

[Dependency(typeof(MyDependency), null, Scope.Transient)]
public partial class TransientConsumerComponent : UIComponent {}
";
            return GeneratorTester.Verify<DependencyAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Adds_Override_Keyword_When_Used_On_UIComponent()
        {
            var source = @"
using UIComponents;
using UIComponents.DependencyInjection;

public interface IMyDependency {}
public class MyDependency : IMyDependency {}

[Dependency(typeof(IMyDependency), provide: typeof(MyDependency))]
public partial class ConsumerComponent : UIComponent {}

public partial class UIComponentWithNoOwnDependencies : UIComponent {}
";
            return GeneratorTester.Verify<DependencyAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Handles_Multiple_Namespaces()
        {
            var firstSource = @"
namespace MyLibrary.Core.Services
{
    public interface IService {}
    public class Service : IService {}
}
";
            var secondSource = @"
using UIComponents;
using MyLibrary.Core.Services;

namespace MyLibrary.GUI.Components
{
    [Dependency(typeof(IService), provide: typeof(Service)]
    public class MyGUIComponent : UIComponent {}
}
";
            return GeneratorTester.Verify<DependencyAugmentGenerator>(firstSource, secondSource);
        }

        [Fact]
        public Task It_Does_Not_Generate_When_Not_Inheriting_Consumer_Interface()
        {
            var source = @"
using UIComponents;
using UIComponents.DependencyInjection;

[assembly: Dependency(typeof(IMyDependency), provide: typeof(MyDependency))]

public interface IMyDependency {}
public class MyDependency : IMyDependency {}

public interface ISecondDependency {}
public class SecondDependency : ISecondDependency {}

[Dependency(typeof(ISecondDependency), provide: typeof(SecondDependency))]
public partial class ConsumerClass {}
";
            return GeneratorTester.Verify<DependencyAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Does_Not_Generate_For_Abstract_Classes()
        {
            var source = @"
using UIComponents;
using UIComponents.DependencyInjection;

[assembly: Dependency(typeof(IMyDependency), provide: typeof(MyDependency))]

public interface IMyDependency {}
public class MyDependency : IMyDependency {}

public interface ISecondDependency {}
public class SecondDependency : ISecondDependency {}

[Dependency(typeof(ISecondDependency), provide: typeof(SecondDependency))]
public abstract class ConsumerClass : IDependencyConsumer {}
";
            return GeneratorTester.Verify<DependencyAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Does_Not_Generate_For_Interfaces()
        {
            var source = @"
using UIComponents;
using UIComponents.DependencyInjection;

[assembly: Dependency(typeof(IMyDependency), provide: typeof(MyDependency))]

public interface IMyDependency {}
public class MyDependency : IMyDependency {}

public interface ISecondDependency {}
public class SecondDependency : ISecondDependency {}

[Dependency(typeof(ISecondDependency), provide: typeof(SecondDependency))]
public interface IMyDependencyConsumer : IDependencyConsumer {}
";
            return GeneratorTester.Verify<DependencyAugmentGenerator>(source);
        }

        [Fact]
        public Task It_Does_Not_Generate_If_Types_Are_Missing()
        {
            var source = @"
using UIComponents;
using UIComponents.DependencyInjection;

public interface IMyDependency {}
public class MyDependency : IMyDependency {}

public interface ISecondDependency {}
public class SecondDependency : ISecondDependency {}

[Dependency(typeof(IMyDependency), provide: typeof(MyDependency))]
[Dependency(typeof(ISecondDependency), provide: typeof(SecondDependency), Scope.Transient)]
public partial class ConsumerClass : IDependencyConsumer {}

[Dependency(typeof(IMyDependency), provide: typeof(MyDependency))]
[Dependency(typeof(ISecondDependency), provide: typeof(SecondDependency), Scope.Transient)]
public partial struct ConsumerStruct : IDependencyConsumer {}
";
            return GeneratorTester.VerifyWithoutReferences<DependencyAugmentGenerator>(source);
        }
    }
}

