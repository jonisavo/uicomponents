using NUnit.Framework;

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
            public string GetValue() => "Hello world";
        }

        public class EmptyStringProvider : IStringDependency
        {
            public string GetValue() => "";
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

        private class UIComponentSubclassWithTryProvide : UIComponent
        {
            public readonly bool DependencyWasProvided;
            public readonly IStringDependency StringDependency;
            
            public UIComponentSubclassWithTryProvide()
            {
                DependencyWasProvided = TryProvide(out StringDependency);
            }
        }
        
        [Test]
        public void A_Missing_Dependency_Can_Be_Handled_With_TryProvide()
        {
            UIComponentSubclassWithTryProvide component = null;
            
            Assert.DoesNotThrow(() =>
            {
                component = new UIComponentSubclassWithTryProvide();
            });
            
            Assert.That(component.DependencyWasProvided, Is.False);
            Assert.That(component.StringDependency, Is.Null);
        }
        
        [Dependency(typeof(IStringDependency), provide: typeof(StringProvider), Scope.Transient)]
        private class UIComponentWithTransientDependencyA : UIComponent
        {
            public readonly IStringDependency StringDependency;
            
            public UIComponentWithTransientDependencyA()
            {
                StringDependency = Provide<IStringDependency>();
            }
        }
        
        [Dependency(typeof(IStringDependency), provide: typeof(StringProvider), Scope.Transient)]
        private class UIComponentWithTransientDependencyB : UIComponent
        {
            public readonly IStringDependency StringDependency;
            
            public UIComponentWithTransientDependencyB()
            {
                StringDependency = Provide<IStringDependency>();
            }
        }
        
        [Test]
        public void Transient_Dependencies_Are_Not_Cached()
        {
            var componentA = new UIComponentWithTransientDependencyA();
            var componentB = new UIComponentWithTransientDependencyB();
            
            Assert.That(componentA.StringDependency, Is.Not.SameAs(componentB.StringDependency));
        }
    }
}