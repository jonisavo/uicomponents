using NUnit.Framework;
using UIComponents.Core;

namespace UIComponents.Tests
{
    [TestFixture]
    public class UIComponentDependencyTests
    {
        private interface IStringDependency
        {
            string GetValue();
        }

        public class StringProvider : IStringDependency
        {
            public string GetValue()
            {
                return "Hello world";
            }
        }

        public class EmptyStringProvider : IStringDependency
        {
            public string GetValue()
            {
                return "";
            }
        }

        [Dependency(typeof(IStringDependency), provide: typeof(StringProvider))]
        private class UIComponentWithStringDependency : UIComponent
        {
            public readonly IStringDependency StringDependency;

            public UIComponentWithStringDependency()
            {
                StringDependency = Provide<IStringDependency>();
            }
        }

        [Dependency(typeof(IStringDependency), provide: typeof(EmptyStringProvider))]
        private class UIComponentSubclassWithDependencyOverride : UIComponent
        {
            public readonly IStringDependency StringDependency;

            public UIComponentSubclassWithDependencyOverride()
            {
                StringDependency = Provide<IStringDependency>();
            }
        }

        private class UIComponentWithNoProvider : UIComponent
        {
            public readonly IStringDependency StringDependency;

            public UIComponentWithNoProvider()
            {
                StringDependency = Provide<IStringDependency>();
            }
        }

        [Test]
        public void The_Correct_Class_Is_Provided_To_Component()
        {
            var component = new UIComponentWithStringDependency();
            Assert.That(component.StringDependency, Is.InstanceOf<StringProvider>());
        }
        
        [Test]
        public void The_Correct_Class_Is_Provided_To_Component_When_Overridden()
        {
            var component = new UIComponentSubclassWithDependencyOverride();
            Assert.That(component.StringDependency, Is.InstanceOf<EmptyStringProvider>());
        }

        [Test]
        public void Null_Is_Provided_When_No_Provider_Exists()
        {
            var component = new UIComponentWithNoProvider();
            Assert.That(component.StringDependency, Is.Null);
        }
    }
}