using System;
using NUnit.Framework;
using UIComponents.DependencyInjection;
using UIComponents.Tests.Utilities;

namespace UIComponents.Tests
{
    [TestFixture]
    public class DiContextTests
    {
        // private interface IDependency {}
        //
        // private class DependencyProvider : IDependency {}
        //
        // [Dependency(typeof(IDependency), provide: typeof(DependencyProvider))]
        // private class UIComponentWithDependency : UIComponent
        // {
        //     public IDependency GetDependency() => Provide<IDependency>();
        // }

        [Test]
        public void Allows_Changing_Current_Context()
        {
            var previousContext = DiContext.Current;

            try
            {
                var context = new DiContext();
                DiContext.ChangeCurrent(context);
                Assert.That(DiContext.Current, Is.SameAs(context));
            }
            finally
            {
                DiContext.ChangeCurrent(previousContext);
            }
        }

        [Test]
        public void Does_Not_Allow_Changing_To_Null_Context()
        {
            Assert.Throws<ArgumentNullException>(() => DiContext.ChangeCurrent(null));
        }

        [Test]
        public void Allows_Getting_Singleton_Instances()
        {
            var diContext = new DiContext();
            
            diContext.SingletonInstanceDictionary.Add(typeof(string), "Hello world");

            var instances = diContext.GetSingletonInstances();
            
            Assert.That(instances[typeof(string)], Is.EqualTo("Hello world"));
        }

        [Test]
        public void Allows_Resetting_Context()
        {
            var diContext = new DiContext();
            
            diContext.SingletonInstanceDictionary.Add(typeof(string), "Hello world");
            diContext.InjectorDictionary.Add(typeof(string), new DependencyInjector(diContext));
            
            diContext.Clear();
            
            Assert.That(diContext.SingletonInstanceDictionary.Count, Is.EqualTo(0));
            Assert.That(diContext.InjectorDictionary.Count, Is.EqualTo(0));
        }

        [TestFixture]
        public class GetInjector
        {
            private class Consumer {}
            
            [Test]
            public void It_Creates_A_New_Injector_If_One_Does_Not_Exist()
            {
                var diContext = new DiContext();

                var injector = diContext.GetInjector(typeof(Consumer));

                var injectionIsInContext = diContext.InjectorDictionary.ContainsValue(injector);
                
                Assert.That(injectionIsInContext);
            }

            [Test]
            public void It_Yields_Existing_Injector()
            {
                var diContext = new DiContext();

                var injector = new DependencyInjector(diContext);
                
                diContext.InjectorDictionary.Add(typeof(Consumer), injector);

                var receivedInjector = diContext.GetInjector(typeof(Consumer));
                
                Assert.That(receivedInjector, Is.SameAs(injector));
            }

            [Test]
            public void It_Throws_If_Consumer_Type_Is_Null()
            {
                var diContext = new DiContext();

                Assert.Throws<ArgumentNullException>(() => diContext.GetInjector(null));
            }
        }

        [TestFixture]
        public class RegisterConsumer
        {
            private interface IMockDependency {}
            public class MockDependency : IMockDependency {}
            
            [Test]
            public void It_Populates_Singleton_Instances()
            {
                var diContext = new DiContext();

                var consumer = MockUtilities.CreateDependencyConsumer(new IDependency[]
                {
                    new Dependency<IMockDependency>(Scope.Singleton, () => new MockDependency())
                });
                
                diContext.RegisterConsumer(consumer);

                var contextHasSingletonInstance =
                    diContext.SingletonInstanceDictionary.ContainsKey(typeof(IMockDependency));
                
                Assert.That(contextHasSingletonInstance);
            }
        }

        // [TestFixture]
        // public class SwitchContainer
        // {
        //     [Test]
        //     public void Should_Switch_Container()
        //     {
        //         var context = new DiContext();
        //         DiContext.SetCurrent(context);
        //         var dependency = new DependencyProvider();
        //         
        //         context.SetDependency<UIComponentWithDependency, IDependency>(dependency);
        //         
        //         var component = new UIComponentWithDependency();
        //         Assert.That(component.GetDependency(), Is.EqualTo(dependency));
        //
        //         var otherContainer = new DiContainer();
        //         var dependencyTwo = new DependencyProvider();
        //         
        //         otherContainer
        //             .GetInjector(typeof(UIComponentWithDependency))
        //             .SetDependency<IDependency>(dependencyTwo);
        //         
        //         context.SwitchContainer(otherContainer);
        //         
        //         component = new UIComponentWithDependency();
        //         Assert.That(component.GetDependency(), Is.Not.EqualTo(dependency));
        //         Assert.That(component.GetDependency(), Is.EqualTo(dependencyTwo));
        //         DiContext.SetCurrent(DiContext.Previous);
        //     }
        //     
        //     [Test]
        //     public void Throws_ArgumentNullException_If_Given_Container_Is_Null()
        //     {
        //         var context = new DiContext();
        //         Assert.Throws<ArgumentNullException>(() => context.SwitchContainer(null));
        //     }
        // }
        
        // [TestFixture]
        // public class ClearDependency
        // {
        //     [Test]
        //     public void Removes_The_Dependency_Instance()
        //     {
        //         var diContext = new DiContext();
        //         DiContext.SetCurrent(diContext);
        //         var component = new UIComponentWithDependency();
        //         
        //         diContext.ClearDependency<UIComponentWithDependency, IDependency>();
        //
        //         Assert.Throws<MissingProviderException>(() => component.GetDependency());
        //         DiContext.SetCurrent(DiContext.Previous);
        //     }
        // }

        // [TestFixture]
        // public class SetDependency
        // {
        //     [Test]
        //     public void Sets_The_Dependency_Instance()
        //     {
        //         var diContext = new DiContext();
        //         DiContext.SetCurrent(diContext);
        //         var component = new UIComponentWithDependency();
        //         var dependency = new DependencyProvider();
        //         
        //         diContext.SetDependency<UIComponentWithDependency, IDependency>(dependency);
        //         
        //         Assert.That(component.GetDependency(), Is.SameAs(dependency));
        //         
        //         DiContext.SetCurrent(DiContext.Previous);
        //     }
        // }

        // [TestFixture]
        // public class RemoveInjector
        // {
        //     [Test]
        //     public void Removes_The_Injector()
        //     {
        //         var diContext = new DiContext();
        //         var componentType = typeof(UIComponentWithDependency);
        //         var injector = new DependencyInjector(diContext.Container);
        //         diContext.Container.InjectorDictionary[componentType] = injector;
        //         diContext.RemoveInjector(componentType);
        //         Assert.That(diContext.Container.InjectorDictionary.ContainsKey(componentType), Is.False);
        //     }
        // }

        // [TestFixture]
        // public class ResetProvidedInstance
        // {
        //     [Test]
        //     public void Resets_The_Dependency_Instance()
        //     {
        //         var diContext = new DiContext();
        //         
        //         DiContext.SetCurrent(diContext);
        //         
        //         var component = new UIComponentWithDependency();
        //
        //         diContext.ClearDependency<UIComponentWithDependency, IDependency>();
        //         diContext.ResetProvidedInstance<UIComponentWithDependency, IDependency>();
        //         
        //         Assert.That(component.GetDependency(), Is.InstanceOf<DependencyProvider>());
        //         
        //         DiContext.SetCurrent(DiContext.Previous);
        //     }
        // }
    }
}