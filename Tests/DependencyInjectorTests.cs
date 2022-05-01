using System;
using NUnit.Framework;
using UIComponents.Core;

namespace UIComponents.Tests
{
    [TestFixture]
    public class DependencyInjectorTests
    {
        private interface IDependency {}

        public class DependencyOne : IDependency {}

        public class DependencyTwo : IDependency {}
        
        [TestFixture]
        public class SetDependency
        {
            [Test]
            public void Switches_The_Dependency()
            {
                var injector = new DependencyInjector();
                
                injector.SetDependency<IDependency>(new DependencyOne());
                
                Assert.That(injector.Provide<IDependency>(), Is.InstanceOf<DependencyOne>());
                
                injector.SetDependency<IDependency>(new DependencyTwo());
                
                Assert.That(injector.Provide<IDependency>(), Is.InstanceOf<DependencyTwo>());
            }

            [Test]
            public void Throws_Exception_If_Null_Is_Given_As_Parameter()
            {
                var injector = new DependencyInjector();

                Assert.Throws<ArgumentNullException>(
                    () => injector.SetDependency<IDependency>(null)
                );
            }
        }
    }
}