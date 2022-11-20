using System;
using System.Collections;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using UIComponents.Internal;
using UIComponents.Testing;
using UIComponents.Tests.Utilities;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace UIComponents.Tests
{
    [TestFixture]
    public partial class TestBedTests
    {
        private interface IMockDependency { }
        private class Dependency : IMockDependency { }

        private interface ITransientDependency { }
        private class TransientDependency : ITransientDependency { }

        private IAssetResolver _mockResolver;

        private Dependency _dependencyInstance;
        private TransientDependency _transientDependencyInstance;

        [Layout("Foo")]
        [Dependency(typeof(IMockDependency), provide: typeof(Dependency))]
        [Dependency(typeof(ITransientDependency), provide: typeof(TransientDependency), Scope.Transient)]
        private partial class Component : UIComponent
        {
            public readonly bool Value;

            public Component(bool value)
            {
                Value = value;
            }
            
            public Component() : this(false) {}
            
            public IMockDependency GetDependency() => Provide<IMockDependency>();

            public ITransientDependency GetTransientDependency() => Provide<ITransientDependency>();
        }

        private TestBed<Component> _testBed;

        [SetUp]
        public void SetUp()
        {
            _dependencyInstance = new Dependency();
            _transientDependencyInstance = new TransientDependency();
            _mockResolver = MockUtilities.CreateMockResolver();
            _mockResolver.LoadAsset<VisualTreeAsset>("Foo")
                .Returns(Task.FromResult(ScriptableObject.CreateInstance<VisualTreeAsset>()));
            _testBed = new TestBed<Component>()
                .WithSingleton<IMockDependency>(_dependencyInstance)
                .WithSingleton(_mockResolver)
                .WithTransient<ITransientDependency>(_transientDependencyInstance);
        }

        [Test]
        public void Allows_Creating_Component_With_Singleton_Dependencies()
        {
            var component = _testBed.CreateComponent();
            Assert.That(component.GetDependency(), Is.SameAs(_dependencyInstance));
        }

        [Test]
        public void Allows_Creating_Component_With_Transient_Dependencies()
        {
            var component = _testBed.CreateComponent();
            Assert.That(component.GetTransientDependency(), Is.SameAs(_transientDependencyInstance));

            var newComponent = _testBed.CreateComponent();
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
            var dependency = _testBed.Provide<IMockDependency>();
            Assert.That(dependency, Is.SameAs(_dependencyInstance));
        }

        [Test]
        public void Allows_Changing_Singleton_Instance()
        {
            var newDependency = new Dependency();
            _testBed.WithSingleton<IMockDependency>(newDependency);
            var dependency = _testBed.Provide<IMockDependency>();
            Assert.That(dependency, Is.Not.SameAs(_dependencyInstance));
            Assert.That(dependency, Is.SameAs(newDependency));
        }

        [UnityTest]
        public IEnumerator Allows_Fetching_Component_With_Task()
        {
            var componentTask = _testBed.CreateComponentAsync(() => new Component(true));
            
            yield return componentTask.AsEnumerator();
            
            var component = componentTask.Result;
            
            Assert.That(component.Value, Is.True);
            Assert.That(component.GetDependency(), Is.SameAs(_dependencyInstance));

            var anotherComponentTask = _testBed.CreateComponentAsync();

            yield return componentTask.AsEnumerator();
            
            var anotherComponent = anotherComponentTask.Result;
            
            Assert.That(anotherComponent.Value, Is.False);
            Assert.That(anotherComponent.GetDependency(), Is.SameAs(_dependencyInstance));
        }
       

        [Test]
        public void Allows_Setting_Timeout_For_Async_Operations()
        {
            _mockResolver.LoadAsset<VisualTreeAsset>("Foo")
                .Returns(Task.Delay(1000).ContinueWith(_ => ScriptableObject.CreateInstance<VisualTreeAsset>()));
            _testBed.WithAsyncTimeout(TimeSpan.Zero);

            TestBedTimeoutException exception = null;

            try
            {
                var task = _testBed.CreateComponentAsync();
                task.GetAwaiter().GetResult();
            }
            catch (TestBedTimeoutException ex)
            {
                exception = ex;
            }
            
            Assert.That(exception, Is.Not.Null);
            Assert.That(exception.Message, Is.EqualTo("Creation of component Component timed out after 0ms."));
        }
    }
}
