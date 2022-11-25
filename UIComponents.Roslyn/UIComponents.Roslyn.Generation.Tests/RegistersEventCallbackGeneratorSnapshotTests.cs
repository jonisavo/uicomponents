using UIComponents.Roslyn.Generation.Tests.Utilities;
using UIComponents.Roslyn.Generation.Generators.InterfaceModifiers;

namespace UIComponents.Roslyn.Generation.Tests
{
    [UsesVerify]
    public class RegistersEventCallbackGeneratorSnapshotTests
    {
        [Fact]
        public Task It_Generates_Functions_For_Callbacks()
        {
            var source = @"
using UIComponents.InterfaceModifiers;

public class MyEvent {}

[RegistersEventCallback(typeof(MyEvent))]
public interface IOnMyEvent {}

public partial class CallbackHandler : IOnMyEvent {}

public class MyOtherEvent

[RegistersEventCallback(typeof(MyOtherEvent))]
public interface IOnMyOtherEvent {}

public partial class OtherCallbackHandler : CallbackHandler, IOnMyOtherEvent {}
";
            return GeneratorTester.Verify<RegistersEventCallbackGenerator>(source);
        }

        [Fact]
        public Task It_Allows_Specifying_Method_Name()
        {
            var source = @"
using UIComponents.InterfaceModifiers;

public class MyEvent {}

[RegistersEventCallback(typeof(MyEvent), ""MyEventHappened"")]
public interface IOnMyEvent {}

public partial class CallbackHandler : IOnMyEvent {}

public class MyOtherEvent

[RegistersEventCallback(typeof(MyOtherEvent), ""Callback"")]
public interface IOnMyOtherEvent {}

public partial class OtherCallbackHandler : CallbackHandler, IOnMyOtherEvent {}
";
            return GeneratorTester.Verify<RegistersEventCallbackGenerator>(source);
        }

        [Fact]
        public Task It_Does_Not_Generate_For_Abstract_Class()
        {
            var source = @"
using UIComponents.InterfaceModifiers;

public class MyEvent {}

[RegistersEventCallback(typeof(MyEvent))]
public interface IOnMyEvent {}

public abstract class BaseCallbackHandler : IOnMyEvent {}

public class MyOtherEvent

[RegistersEventCallback(typeof(MyOtherEvent))]
public interface IOnMyOtherEvent {}

public partial class CallbackHandler : BaseCallbackHandler, IOnMyOtherEvent {}
";
            return GeneratorTester.Verify<RegistersEventCallbackGenerator>(source);
        }

        [Fact]
        public Task It_Handles_Common_Namespaces()
        {
            var firstSource = @"
using UIComponents.InterfaceModifiers;

namespace MyLibrary.Events
{
    public class MyEvent {}
}

namespace MyLibrary.Generation.EventInterfaces
{
    [RegistersEventCallback(typeof(MyLibrary.Events.MyEvent))]
    public interface IOnEvent
    {
        void OnEvent(MyEvent evt);
    }
}";
            var secondSource = @"
using UIComponents;
using MyLibrary.Generation.EventInterfaces;

namespace MyLibrary.Components
{
    public partial class MyComponent : UIComponent, IOnEvent {}
}
";
            return GeneratorTester.Verify<RegistersEventCallbackGenerator>(firstSource, secondSource);
        }

        [Fact]
        public Task It_Handles_Nested_Types()
        {
            var source = @"
using UIComponents.InterfaceModifiers;

public partial class BaseClass
{
    [RegistersEventCallback(typeof(OnClickEvent))]
    public interface IOnClick {}

    private partial class ClickHandler : IOnClick {}
}
";
            return GeneratorTester.Verify<RegistersEventCallbackGenerator>(source);
        }

        [Fact]
        public Task It_Handles_Nested_Types_And_Namespace()
        {
            var eventSource = @"
using UIComponents.InterfaceModifiers;

namespace UnityEngine.UIElements
{
    public class CallbackEvent {}

    public class Event {}
}

namespace UIComponents
{
    [RegistersEventCallback(typeof(UnityEngine.UIElements.CallbackEvent))]
    public interface IOnCallback {}

    [RegistersEventCallback(typeof(UnityEngine.UIElements.Event))]
    public interface IOnEvent {}
}
";
            var source = @"
using UIComponents;

namespace UIComponents.Tests.Editor.Interfaces
{
    public partial class Tests
    {
        private class BaseTestComponent : UIComponent {}

        private partial class OnCallbackComponent : BaseTestComponent, IOnCallback {}

        private partial class OnEventComponent : BaseTestComponent, IOnEvent {}
    }    
}
";
            return GeneratorTester.Verify<RegistersEventCallbackGenerator>(eventSource, source);
        }

        [Fact]
        public Task It_Handles_Interface_Inheritance()
        {
            var source = @"
using UIComponents.InterfaceModifiers;

public class MyEvent {}

public class OtherEvent {}

[RegistersEventCallback(typeof(MyEvent))]
public interface IOnMyEvent {}

[RegistersEventCallback(typeof(OtherEvent))]
public interface IOnOtherEvent {}

public interface IEventHandler : IOnMyEvent, IOnOtherEvent {}

public class EventHandler : IEventHandler {}
";
            return GeneratorTester.Verify<RegistersEventCallbackGenerator>(source);
        }

        [Fact]
        public Task It_Does_Not_Generate_If_No_Attribute_Exists()
        {
            var source = @"
using UIComponents.InterfaceModifiers;

public class MyEvent {}

public interface IOnMyEvent {}

public class CallbackHandler : IOnMyEvent {}
";
            return GeneratorTester.Verify<RegistersEventCallbackGenerator>(source);
        }

        [Fact]
        public Task It_Does_Not_Generate_On_Anything_Other_Than_Class()
        {
            var source = @"
using UIComponents.InterfaceModifiers;

public class MyEvent {}

[RegistersEventCallback(typeof(MyEvent))]
public interface IOnMyEvent {}

public struct CallbackHandler : IOnMyEvent {}

public interface ICallbackHandler : IOnMyEvent {}
";
            return GeneratorTester.Verify<RegistersEventCallbackGenerator>(source);
        }

        [Fact]
        public Task It_Does_Not_Generate_If_The_RegistersCallbackAttribute_Type_Is_Missing()
        {
            var source = @"
using UIComponents.InterfaceModifiers;

public class MyEvent {}

[RegistersEventCallback(typeof(MyEvent))]
public interface IOnMyEvent {}

public partial class CallbackHandler : IOnMyEvent {}
";
            return GeneratorTester.VerifyWithoutReferences<RegistersEventCallbackGenerator>(source);
        }
    }
}

