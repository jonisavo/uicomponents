using System;
using NUnit.Framework;
using UIComponents.DependencyInjection;

namespace UIComponents.Tests
{
    [TestFixture]
    public class DiContainerTests
    {
        private static readonly Type StringType = typeof(string);
        
        [Test]
        public void Singleton_Instance_Can_Be_Set()
        {
            var container = new DiContainer();
            
            container.RegisterSingletonInstance(StringType, "Hello world");

            var couldFetchValue = container.TryGetSingletonInstance(StringType, out var text);
            
            Assert.That(couldFetchValue, Is.True);
            Assert.That(text, Is.EqualTo("Hello world"));
        }

        [Test]
        public void Singleton_Instance_Existence_Can_Be_Queried()
        {
            var container = new DiContainer();
            
            Assert.That(container.ContainsSingletonInstanceOfType(StringType), Is.False);
            container.RegisterSingletonInstance(StringType, "Hello world");
            Assert.That(container.ContainsSingletonInstanceOfType(StringType), Is.True);
        }

        [Test]
        public void Null_Can_Not_Be_Set_As_Singleton_Instance()
        {
            var container = new DiContainer();
            
            Assert.That(() => container.RegisterSingletonInstance(StringType, null), Throws.ArgumentNullException);
        }

        [Test]
        public void Singleton_Override_Can_Be_Set()
        {
            var container = new DiContainer();
            
            Assert.That(container.TryGetSingletonOverride(StringType, out _), Is.False);
            container.SetSingletonOverride("Hello world");
            Assert.That(container.TryGetSingletonOverride(StringType, out var text), Is.True);
            Assert.That(text, Is.EqualTo("Hello world"));
        }
        
        [Test]
        public void Singleton_Override_Can_Not_Be_Null()
        {
            var container = new DiContainer();
            
            Assert.That(() => container.SetSingletonOverride((string) null), Throws.ArgumentNullException);
        }

        [Test]
        public void Singleton_Override_Can_Be_Removed()
        {
            var container = new DiContainer();
            
            container.SetSingletonOverride("Hello world");
            container.RemoveSingletonOverride<string>();
            Assert.That(container.TryGetSingletonOverride(StringType, out _), Is.False);
        }
    }
}