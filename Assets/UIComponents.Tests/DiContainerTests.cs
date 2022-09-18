using System;
using NUnit.Framework;
using UIComponents.DependencyInjection;

namespace UIComponents.Tests
{
    [TestFixture]
    public class DiContainerTests
    {
        private static readonly Type StringType = typeof(string);
        private DiContainer _container;
        private DependencyInjector _dependencyInjector;

        [SetUp]
        public void SetUp()
        {
            _container = new DiContainer();
            _dependencyInjector = new DependencyInjector(_container);
            _dependencyInjector.SetDependency<string>("Initial value");
            _container.InjectorDictionary.Add(StringType, _dependencyInjector);
        }
        
        [Test]
        public void Singleton_Instance_Can_Be_Set()
        {
            _container.RegisterSingletonInstance(StringType, "Hello world");

            var couldFetchValue = _container.TryGetSingletonInstance(StringType, out var text);
            var couldFetchValueGeneric = _container.TryGetSingletonInstance<string>(out var textTwo);
            
            Assert.That(couldFetchValue, Is.True);
            Assert.That(text, Is.EqualTo("Hello world"));
            Assert.That(couldFetchValueGeneric, Is.True);
            Assert.That(textTwo, Is.EqualTo("Hello world"));
        }

        [Test]
        public void Missing_Singleton_Instance_Can_Be_Queried()
        {
            Assert.That(_container.TryGetSingletonInstance(typeof(int[]), out _), Is.False);
            Assert.That(_container.TryGetSingletonInstance<int[]>(out _), Is.False);
        }

        [Test]
        public void Singleton_Instance_Existence_Can_Be_Queried()
        {
            Assert.That(_container.ContainsSingletonInstanceOfType(typeof(int)), Is.False);
            _container.RegisterSingletonInstance(typeof(int), 1);
            Assert.That(_container.ContainsSingletonInstanceOfType(typeof(int)), Is.True);
        }

        [Test]
        public void Null_Can_Not_Be_Set_As_Singleton_Instance()
        {
            Assert.That(() => _container.RegisterSingletonInstance(StringType, null), Throws.ArgumentNullException);
        }

        [Test]
        public void Singleton_Override_Can_Be_Set()
        {
            Assert.That(_container.TryGetSingletonOverride(StringType, out _), Is.False);
            _container.SetSingletonOverride("Hello world");
            Assert.That(_container.TryGetSingletonOverride(StringType, out var text), Is.True);
            Assert.That(text, Is.EqualTo("Hello world"));
            Assert.That(_dependencyInjector.Provide<string>(), Is.EqualTo("Hello world"));
        }
        
        [Test]
        public void Singleton_Override_Can_Not_Be_Null()
        {
            Assert.That(() => _container.SetSingletonOverride((string) null), Throws.ArgumentNullException);
        }

        [Test]
        public void Singleton_Override_Can_Be_Removed()
        {
            _container.SetSingletonOverride("Hello world");
            _container.RemoveSingletonOverride<string>();
            Assert.That(_container.TryGetSingletonOverride(StringType, out _), Is.False);
            Assert.That(_dependencyInjector.Provide<string>(), Is.EqualTo("Initial value"));
        }
    }
}
