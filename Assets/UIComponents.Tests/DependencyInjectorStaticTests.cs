using NUnit.Framework;

namespace UIComponents.Tests
{
    [TestFixture]
    public class DependencyInjectorStaticTests
    {
        private interface IDependency {}
        
        private class DependencyProvider : IDependency {}

        [Dependency(typeof(IDependency), provide: typeof(DependencyProvider))]
        private class UIComponentWithDependency : UIComponent
        {
            public IDependency GetDependency() => Provide<IDependency>();
        }
        
        [TestFixture]
        public class ClearDependency
        {
            [Test]
            public void Removes_The_Dependency_Instance()
            {
                var component = new UIComponentWithDependency();
                
                DependencyInjector.ClearDependency<UIComponentWithDependency, IDependency>();

                Assert.Throws<MissingProviderException>(() => component.GetDependency());
            }
            
            [TearDown]
            public void TearDown()
            {
                DependencyInjector.SetDependency<UIComponentWithDependency, IDependency>(
                    new DependencyProvider()
                );
            }
        }

        [TestFixture]
        public class RemoveInjector
        {
            [Test]
            public void Removes_The_Injector_From_The_Dependency_Injector()
            {
                var componentType = typeof(UIComponentWithDependency);
                var injector = new DependencyInjector();
                DependencyInjector.Container.InjectorDictionary[componentType] = injector;
                DependencyInjector.RemoveInjector(componentType);
                Assert.That(DependencyInjector.Container.InjectorDictionary.ContainsKey(componentType), Is.False);
            }
        }

        [TestFixture]
        public class RestoreDefaultDependency
        {
            [Test]
            public void Restores_The_Default_Dependency_Instance()
            {
                var component = new UIComponentWithDependency();

                DependencyInjector.ClearDependency<UIComponentWithDependency, IDependency>();
                DependencyInjector.RestoreDefaultDependency<UIComponentWithDependency, IDependency>();
                
                Assert.That(component.GetDependency(), Is.InstanceOf<DependencyProvider>());
            }
        }
    }
}