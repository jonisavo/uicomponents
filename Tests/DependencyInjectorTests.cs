using System;
using NUnit.Framework;
using UIComponents.Core;
using UIComponents.Core.Exceptions;

namespace UIComponents.Tests
{
    [TestFixture]
    public class DependencyInjectorTests
    {
        private interface IDependency {}

        public class DependencyOne : IDependency {}

        public class DependencyTwo : IDependency {}

        [TestFixture]
        public class Provide
        {
            [Test]
            public void Returns_Desired_Dependency()
            {
                var injector = new DependencyInjector();
                injector.SetDependency<IDependency>(new DependencyOne());
                Assert.That(injector.Provide<IDependency>(), Is.InstanceOf<DependencyOne>());
            }

            [Test]
            public void Throws_If_No_Provider_Exists()
            {
                var injector = new DependencyInjector();

                Assert.Throws<MissingProviderException>(
                    () => injector.Provide<IDependency>()
                );
            }
        }
        
        [TestFixture]
        public class SetDependency
        {
            [Test]
            public void Switches_The_Dependency()
            {
                var injector = new DependencyInjector();
                
                injector.SetDependency<IDependency>(new DependencyOne());
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