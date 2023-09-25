using NUnit.Framework;

namespace UIComponents.Tests
{
    [TestFixture]
    public partial class UIComponentDependencyTests
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
        private partial class UIComponentWithStringDependency : UIComponent
        {
            public readonly IStringDependency StringDependency;

            public UIComponentWithStringDependency()
            {
                StringDependency = Provide<IStringDependency>();
            }
        }

        [Dependency(typeof(IStringDependency), provide: typeof(EmptyStringProvider))]
        private partial class UIComponentSubclassWithDependencyOverride : UIComponentWithStringDependency {}

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

        private partial class UIComponentSubclassWithTryProvide : UIComponent
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
        private partial class UIComponentWithTransientDependencyA : UIComponent
        {
            public readonly IStringDependency StringDependency;
            
            public UIComponentWithTransientDependencyA()
            {
                StringDependency = Provide<IStringDependency>();
            }
        }
        
        [Dependency(typeof(IStringDependency), provide: typeof(StringProvider), Scope.Transient)]
        private partial class UIComponentWithTransientDependencyB : UIComponent
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

        [Dependency(typeof(StringProvider))]
        private partial class UIComponentWithConcreteDependency : UIComponent
        {
            public readonly StringProvider StringProvider;
            
            [Provide]
            public StringProvider ProvidedStringProvider;
            
            public UIComponentWithConcreteDependency()
            {
                StringProvider = Provide<StringProvider>();
            }
        }
        
        [Test]
        public void Concrete_Dependencies_Are_Provided()
        {
            var component = new UIComponentWithConcreteDependency();
            var component2 = new UIComponentWithConcreteDependency();
            
            Assert.That(component.StringProvider, Is.InstanceOf<StringProvider>());
            Assert.That(component.ProvidedStringProvider, Is.InstanceOf<StringProvider>());
            Assert.That(component.StringProvider, Is.SameAs(component2.StringProvider));
            Assert.That(component.StringProvider, Is.SameAs(component2.ProvidedStringProvider));
        }
    }
}