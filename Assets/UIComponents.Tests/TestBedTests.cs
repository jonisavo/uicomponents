using System;
using System.Collections;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using UIComponents.Testing;
using UIComponents.Tests.Utilities;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace UIComponents.Tests
{
    [TestFixture]
    public class TestBedTests
    {
        private TestBed _testBed;
        
        private Dependency _dependencyInstance;
        private TransientDependency _transientDependencyInstance;
        
        private interface IDependency {}

        private class Dependency : IDependency {}
        
        private interface ITransientDependency {}
        
        private class TransientDependency : ITransientDependency {}

        [Dependency(typeof(IDependency), provide: typeof(Dependency))]
        [Dependency(typeof(ITransientDependency), provide: typeof(TransientDependency), Scope.Transient)]
        private class Component : UIComponent
        {
            public readonly bool Value;

            public Component(bool value)
            {
                Value = value;
            }
            
            public Component() : this(false) {}
            
            public IDependency GetDependency() => Provide<IDependency>();

            public ITransientDependency GetTransientDependency() => Provide<ITransientDependency>();
        }

        [SetUp]
        public void SetUp()
        {
            _dependencyInstance = new Dependency();
            _transientDependencyInstance = new TransientDependency();
            _testBed = TestBed.Create()
                .WithSingleton<IDependency>(_dependencyInstance)
                .WithTransient<Component, ITransientDependency>(_transientDependencyInstance)
                .Build();
        }

        [Test]
        public void Allows_Creating_Component_With_Singleton_Dependencies()
        {
            var component = _testBed.CreateComponent<Component>();
            Assert.That(component.GetDependency(), Is.SameAs(_dependencyInstance));
        }

        [Test]
        public void Allows_Creating_Component_With_Transient_Dependencies()
        {
            var component = _testBed.CreateComponent<Component>();
            Assert.That(component.GetTransientDependency(), Is.SameAs(_transientDependencyInstance));

            var newComponent = _testBed.CreateComponent<Component>();
            Assert.That(newComponent.GetTransientDependency(), Is.SameAs(_transientDependencyInstance));
        }
        
        [Test]
        public void Allows_Creating_Component_With_Predicate()
        {
            var component = _testBed.CreateComponent(() => new Component(true));
            Assert.That(component.Value, Is.True);
            Assert.That(component.GetDependency(), Is.SameAs(_dependencyInstance));
        }

        [Test]
        public void Allows_Fetching_Dependencies()
        {
            var dependency = _testBed.Provide<Component, IDependency>();
            Assert.That(dependency, Is.SameAs(_dependencyInstance));
        }

        [UnityTest]
        public IEnumerator Allows_Fetching_Component_With_Task()
        {
            var componentTask = _testBed.CreateComponentAsync(() => new Component(true));
            
            yield return componentTask.AsEnumerator();
            
            var component = componentTask.Result;
            
            Assert.That(component.Value, Is.True);
            Assert.That(component.GetDependency(), Is.SameAs(_dependencyInstance));
        }
        
        [Layout("Foo")]
        private class ComponentWithLayout : Component {}

        [Test]
        public void Allows_Setting_Timeout_For_Async_Operations()
        {
            var mockResolver = MockUtilities.CreateMockResolver();
            mockResolver.LoadAsset<VisualTreeAsset>("Foo")
                .Returns(Task.Delay(1000).ContinueWith(_ => ScriptableObject.CreateInstance<VisualTreeAsset>()));
            _testBed.DiContainer.SetSingletonOverride(mockResolver);
            _testBed.AsyncTimeout = TimeSpan.Zero;

            TestBedTimeoutException exception = null;

            try
            {
                var task = _testBed.CreateComponentAsync<ComponentWithLayout>();
                task.GetAwaiter().GetResult();
            }
            catch (TestBedTimeoutException ex)
            {
                exception = ex;
            }
            
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("Creation of component ComponentWithLayout timed out after 0ms."));
        }
    }
}
