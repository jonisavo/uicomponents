using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using UIComponents.DependencyInjection;
using UIComponents.Testing;
using UIComponents.Tests.Utilities;
using UnityEngine;
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
            var component = _testBed.Instantiate();
            Assert.That(component.GetDependency(), Is.SameAs(_dependencyInstance));
        }

        [Test]
        public void Allows_Creating_Component_With_Transient_Dependencies()
        {
            var component = _testBed.Instantiate();
            Assert.That(component.GetTransientDependency(), Is.SameAs(_transientDependencyInstance));

            var newComponent = _testBed.Instantiate();
            Assert.That(newComponent.GetTransientDependency(), Is.SameAs(_transientDependencyInstance));
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

        [Dependency(typeof(ILogger), provide: typeof(DebugLogger))]
        private abstract class Service : IDependencyConsumer
        {
            protected readonly ILogger Logger;
            private readonly DependencyInjector _dependencyInjector;

            protected Service()
            {
                DiContext.Current.RegisterConsumer(this);
                _dependencyInjector = DiContext.Current.GetInjector(GetType());
                Logger = Provide<ILogger>();
                UIC_PopulateProvideFields();
            }

            protected T Provide<T>() where T : class
            {
                return _dependencyInjector.Provide<T>();
            }

            public abstract IEnumerable<IDependency> GetDependencies();

            protected virtual void UIC_PopulateProvideFields() {}
        }

        [Dependency(typeof(IMockDependency), provide: typeof(Dependency))]
        [Dependency(typeof(ITransientDependency), provide: typeof(TransientDependency), Scope.Transient)]
        private partial class TestService : Service
        {
            [Provide]
            public IMockDependency Dependency;

            public ITransientDependency GetTransientDependency() => Provide<ITransientDependency>();
        }

        [Test]
        public void Works_On_Non_UIComponent_Type()
        {
            var testBed = new TestBed<TestService>()
                .WithSingleton<IMockDependency>(_dependencyInstance)
                .WithTransient<ITransientDependency>(_transientDependencyInstance);

            var service = testBed.Instantiate();
            Assert.That(service.Dependency, Is.SameAs(_dependencyInstance));
            Assert.That(service.GetTransientDependency(), Is.SameAs(_transientDependencyInstance));
        }
    }
}
