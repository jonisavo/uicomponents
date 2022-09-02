using System;
using NUnit.Framework;
using UIComponents.DependencyInjection;

namespace UIComponents.Tests
{
    [TestFixture]
    public class DiContextTests
    {
        private interface IDependency {}
        
        private class DependencyProvider : IDependency {}

        [Dependency(typeof(IDependency), provide: typeof(DependencyProvider))]
        private class UIComponentWithDependency : UIComponent
        {
            public IDependency GetDependency() => Provide<IDependency>();
        }

        [TestFixture]
        public class SetCurrent
        {
            [Test]
            public void Sets_Current_Context()
            {
                var previousContext = DiContext.Current;
                var context = new DiContext();
                DiContext.SetCurrent(context);
                Assert.That(DiContext.Current, Is.SameAs(context));
                Assert.That(DiContext.Previous, Is.SameAs(previousContext));
                DiContext.SetCurrent(previousContext);
            }
            
            [Test]
            public void Does_Not_Set_Previous_If_Given_Context_Is_Same_As_Current()
            {
                var previousContext = DiContext.Current;
                var context = new DiContext();
                DiContext.SetCurrent(context);
                Assert.That(DiContext.Previous, Is.SameAs(previousContext));
                DiContext.SetCurrent(context);
                Assert.That(DiContext.Previous, Is.SameAs(previousContext));
                DiContext.SetCurrent(previousContext);
            }
            
            [Test]
            public void Throws_ArgumentNullException_If_Given_Context_Is_Null()
            {
                Assert.Throws<ArgumentNullException>(() => DiContext.SetCurrent(null));
            }
        }

        [TestFixture]
        public class SwitchContainer
        {
            [Test]
            public void Should_Switch_Container()
            {
                var context = new DiContext();
                DiContext.SetCurrent(context);
                var dependency = new DependencyProvider();
                
                context.SetDependency<UIComponentWithDependency, IDependency>(dependency);
                
                var component = new UIComponentWithDependency();
                Assert.That(component.GetDependency(), Is.EqualTo(dependency));

                var otherContainer = new DiContainer();
                var dependencyTwo = new DependencyProvider();
                
                otherContainer
                    .GetInjector(typeof(UIComponentWithDependency))
                    .SetDependency<IDependency>(dependencyTwo);
                
                context.SwitchContainer(otherContainer);
                
                component = new UIComponentWithDependency();
                Assert.That(component.GetDependency(), Is.Not.EqualTo(dependency));
                Assert.That(component.GetDependency(), Is.EqualTo(dependencyTwo));
                DiContext.SetCurrent(DiContext.Previous);
            }
            
            [Test]
            public void Throws_ArgumentNullException_If_Given_Container_Is_Null()
            {
                var context = new DiContext();
                Assert.Throws<ArgumentNullException>(() => context.SwitchContainer(null));
            }
        }
        
        [TestFixture]
        public class ClearDependency
        {
            [Test]
            public void Removes_The_Dependency_Instance()
            {
                var diContext = new DiContext();
                DiContext.SetCurrent(diContext);
                var component = new UIComponentWithDependency();
                
                diContext.ClearDependency<UIComponentWithDependency, IDependency>();

                Assert.Throws<MissingProviderException>(() => component.GetDependency());
                DiContext.SetCurrent(DiContext.Previous);
            }
        }

        [TestFixture]
        public class SetDependency
        {
            [Test]
            public void Sets_The_Dependency_Instance()
            {
                var diContext = new DiContext();
                DiContext.SetCurrent(diContext);
                var component = new UIComponentWithDependency();
                var dependency = new DependencyProvider();
                
                diContext.SetDependency<UIComponentWithDependency, IDependency>(dependency);
                
                Assert.That(component.GetDependency(), Is.SameAs(dependency));
                
                DiContext.SetCurrent(DiContext.Previous);
            }
        }

        [TestFixture]
        public class RemoveInjector
        {
            [Test]
            public void Removes_The_Injector()
            {
                var diContext = new DiContext();
                var componentType = typeof(UIComponentWithDependency);
                var injector = new DependencyInjector(diContext.Container);
                diContext.Container.InjectorDictionary[componentType] = injector;
                diContext.RemoveInjector(componentType);
                Assert.That(diContext.Container.InjectorDictionary.ContainsKey(componentType), Is.False);
            }
        }

        [TestFixture]
        public class ResetProvidedInstance
        {
            [Test]
            public void Resets_The_Dependency_Instance()
            {
                var diContext = new DiContext();
                
                DiContext.SetCurrent(diContext);
                
                var component = new UIComponentWithDependency();

                diContext.ClearDependency<UIComponentWithDependency, IDependency>();
                diContext.ResetProvidedInstance<UIComponentWithDependency, IDependency>();
                
                Assert.That(component.GetDependency(), Is.InstanceOf<DependencyProvider>());
                
                DiContext.SetCurrent(DiContext.Previous);
            }
        }
    }
}