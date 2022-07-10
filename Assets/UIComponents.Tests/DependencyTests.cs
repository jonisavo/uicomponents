using NUnit.Framework;

namespace UIComponents.Tests
{
    [TestFixture]
    public class DependencyTests
    {
        private interface IInterface {}
        
        private class ClassThatImplementsInterface : IInterface {}
        
        private class ClassThatDoesNotImplementInterface {}
        
        [Test]
        public void Constructor_Throws_On_Invalid_Instance()
        {
            Assert.That(() => new Dependency(typeof(IInterface), (object) null, Scope.Singleton), Throws.ArgumentNullException);
            Assert.That(() => new Dependency(null, typeof(ClassThatImplementsInterface), Scope.Singleton), Throws.ArgumentNullException);
            Assert.That(() => new Dependency(typeof(IInterface), typeof(ClassThatDoesNotImplementInterface), Scope.Singleton), Throws.ArgumentException);
        }

        [Test]
        public void ChangeInstance_Throws_On_Invalid_Instance()
        {
            var dependency = new Dependency(typeof(IInterface), typeof(ClassThatImplementsInterface), Scope.Singleton);
            
            Assert.That(() => dependency.ChangeInstance(null), Throws.ArgumentNullException);
            Assert.That(() => dependency.ChangeInstance(new ClassThatDoesNotImplementInterface()), Throws.ArgumentException);
        }

        [Test]
        public void Clear_Sets_Instance_To_Null()
        {
            var dependency = new Dependency(typeof(IInterface), typeof(ClassThatImplementsInterface), Scope.Singleton);
            Assert.That(dependency.Instance, Is.Not.Null);
            dependency.Clear();
            Assert.That(dependency.Instance, Is.Null);
        }
    }
}