using NUnit.Framework;
using UIComponents.DependencyInjection;

namespace UIComponents.Tests
{
    [TestFixture]
    public class DependencyTests
    {
        private interface IInterface {}
        
        private class ClassThatImplementsInterface : IInterface {}
        
        // private class ClassThatDoesNotImplementInterface {}
        
        [Test]
        public void Constructor_Throws_On_Null_Factory()
        {
            Assert.That(() => new Dependency<IInterface>(Scope.Singleton, null), Throws.ArgumentNullException);
        }

        [Test]
        public void Allows_Fetching_Dependency_Type()
        {
            var dependency = new Dependency<IInterface>(Scope.Singleton, () => new ClassThatImplementsInterface());
            
            Assert.That(dependency.GetDependencyType(), Is.EqualTo(typeof(IInterface)));
        }

        [Test]
        public void Allows_Fetching_Scope()
        {
            var dependency = new Dependency<IInterface>(Scope.Singleton, () => new ClassThatImplementsInterface());
            
            Assert.That(dependency.GetScope(), Is.EqualTo(Scope.Singleton));
        }

        [Test]
        public void Allows_Creating_An_Instance()
        {
            var dependency = new Dependency<IInterface>(Scope.Singleton, () => new ClassThatImplementsInterface());

            var instance = dependency.CreateInstance();
            
            Assert.That(instance, Is.InstanceOf<ClassThatImplementsInterface>());
        }

        // [Test]
        // public void ChangeInstance_Throws_On_Invalid_Instance()
        // {
        //     var dependency = new Dependency(typeof(IInterface), typeof(ClassThatImplementsInterface), Scope.Singleton);
        //     
        //     Assert.That(() => dependency.ChangeInstance(null), Throws.ArgumentNullException);
        //     Assert.That(() => dependency.ChangeInstance(new ClassThatDoesNotImplementInterface()), Throws.ArgumentException);
        // }
        //
        // [Test]
        // public void Clear_Sets_Instance_To_Null()
        // {
        //     var dependency = new Dependency(typeof(IInterface), typeof(ClassThatImplementsInterface), Scope.Singleton);
        //     Assert.That(dependency.Instance, Is.Not.Null);
        //     dependency.Clear();
        //     Assert.That(dependency.Instance, Is.Null);
        // }
    }
}