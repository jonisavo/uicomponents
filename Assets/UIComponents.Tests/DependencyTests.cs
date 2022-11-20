using NUnit.Framework;
using UIComponents.DependencyInjection;

namespace UIComponents.Tests
{
    [TestFixture]
    public class DependencyTests
    {
        private interface IInterface {}
        
        private class ClassThatImplementsInterface : IInterface {}
        
        [Test]
        public void Constructor_Throws_On_Null_Factory()
        {
            Assert.That(() => new Dependency<IInterface, ClassThatImplementsInterface>(Scope.Singleton, null), Throws.ArgumentNullException);
        }

        [Test]
        public void Allows_Fetching_Dependency_Type()
        {
            var dependency = new Dependency<IInterface, ClassThatImplementsInterface>(Scope.Singleton, () => new ClassThatImplementsInterface());
            
            Assert.That(dependency.GetDependencyType(), Is.EqualTo(typeof(IInterface)));
        }

        [Test]
        public void Allows_Fetching_Implementation_Type()
        {
            var dependency = new Dependency<IInterface, ClassThatImplementsInterface>(Scope.Singleton, () => new ClassThatImplementsInterface());

            Assert.That(dependency.GetImplementationType(), Is.EqualTo(typeof(ClassThatImplementsInterface)));
        }

        [Test]
        public void Allows_Fetching_Scope()
        {
            var dependency = new Dependency<IInterface, ClassThatImplementsInterface>(Scope.Singleton, () => new ClassThatImplementsInterface());
            
            Assert.That(dependency.GetScope(), Is.EqualTo(Scope.Singleton));
        }

        [Test]
        public void Allows_Creating_An_Instance()
        {
            var dependency = new Dependency<IInterface, ClassThatImplementsInterface>(Scope.Singleton, () => new ClassThatImplementsInterface());

            var instance = dependency.CreateInstance();
            
            Assert.That(instance, Is.InstanceOf<ClassThatImplementsInterface>());
        }
    }
}