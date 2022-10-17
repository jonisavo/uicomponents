using System;
using NUnit.Framework;
using UIComponents.DependencyInjection;

namespace UIComponents.Tests
{
    [TestFixture]
    public class DependencyInjectorTests
    {
        private interface IDependency {}

        public class DependencyOne : IDependency {}

        public class DependencyTwo : IDependency {}

        [Test]
        public void Can_Be_Created_Using_DependencyAttributes()
        {
            var dependencyAttributes = new[]
            {
                new DependencyAttribute(typeof(IDependency), typeof(DependencyOne))
            };

            var injector = new DependencyInjector(dependencyAttributes, DiContext.Current.Container);
            
            Assert.That(injector.Provide<IDependency>(), Is.InstanceOf<DependencyOne>());
        }

        [TestFixture]
        public class Provide
        {
            [Test]
            public void Returns_Desired_Dependency()
            {
                var injector = new DependencyInjector(DiContext.Current.Container);
                injector.SetDependency<IDependency>(new DependencyOne());
                Assert.That(injector.Provide<IDependency>(), Is.InstanceOf<DependencyOne>());
            }

            [Test]
            public void Returns_Desired_Dependency_With_Non_Generic_Method()
            {
                var injector = new DependencyInjector(DiContext.Current.Container);
                injector.SetDependency<IDependency>(new DependencyOne());
                Assert.That(injector.Provide(typeof(IDependency)), Is.InstanceOf<DependencyOne>());
            }

            [Test]
            public void Throws_If_No_Provider_Exists()
            {
                var injector = new DependencyInjector(DiContext.Current.Container);

                var exception = Assert.Throws<MissingProviderException>(
                    () => injector.Provide<IDependency>()
                );
                
                Assert.That(exception.Message, Is.EqualTo("No provider found for IDependency"));
            }
            
            [Test]
            public void Throws_If_No_Provider_Exists_With_Non_Generic_Method()
            {
                var injector = new DependencyInjector(DiContext.Current.Container);

                var exception = Assert.Throws<MissingProviderException>(
                    () => injector.Provide(typeof(IDependency))
                );
                
                Assert.That(exception.Message, Is.EqualTo("No provider found for IDependency"));
            }
        }

        [TestFixture]
        public class TryProvide
        {
            [Test]
            public void Returns_If_Dependency_Could_Be_Provided()
            {
                var injector = new DependencyInjector(DiContext.Current.Container);
                
                Assert.That(injector.TryProvide<IDependency>(out _), Is.False);
                
                injector.SetDependency<IDependency>(new DependencyOne());

                var wasProvided = injector.TryProvide<IDependency>(out var instance);
                
                Assert.That(wasProvided, Is.True);
                Assert.That(instance, Is.InstanceOf<DependencyOne>());
            }

            [Test]
            public void Yields_Null_If_Dependency_Can_Not_Be_Provided()
            {
                var injector = new DependencyInjector(DiContext.Current.Container);

                injector.TryProvide<IDependency>(out var instance);
                
                Assert.That(instance, Is.Null);
            }
        }
        
        [TestFixture]
        public class SetDependency
        {
            [Test]
            public void Switches_The_Dependency()
            {
                var injector = new DependencyInjector(DiContext.Current.Container);
                
                injector.SetDependency<IDependency>(new DependencyOne());
                injector.SetDependency<IDependency>(new DependencyTwo());
                
                Assert.That(injector.Provide<IDependency>(), Is.InstanceOf<DependencyTwo>());
            }

            [Test]
            public void Throws_Exception_If_Null_Is_Given_As_Parameter()
            {
                var injector = new DependencyInjector(DiContext.Current.Container);

                Assert.Throws<ArgumentNullException>(
                    () => injector.SetDependency<IDependency>(null)
                );
            }
        }

        [TestFixture]
        public class ClearDependency
        {
            [Test]
            public void Removes_Dependency_Instance()
            {
                var injector = new DependencyInjector(DiContext.Current.Container);
                
                injector.SetDependency<IDependency>(new DependencyOne());
                
                injector.ClearDependency<IDependency>();

                Assert.Throws<MissingProviderException>(() => injector.Provide<IDependency>());
            }

            [Test]
            public void Does_Not_Throw_If_Dependency_Does_Not_Exist()
            {
                var injector = new DependencyInjector(DiContext.Current.Container);
                
                Assert.DoesNotThrow(() => injector.ClearDependency<IDependency>());
            }
        }

        [TestFixture]
        public class ResetProvidedInstance
        {
            [SetUp]
            public void SetUp()
            {
                DiContext.Current.Container.Clear();
            }

            [Test]
            public void Throws_If_No_Default_Dependency_Exists()
            {
                var injector = new DependencyInjector(DiContext.Current.Container);
                
                Assert.Throws<InvalidOperationException>(
                    () => injector.ResetProvidedInstance<IDependency>()
                );
                
                var initialDependency = new DependencyOne();
                
                injector.SetDependency<IDependency>(initialDependency);
                
                Assert.DoesNotThrow(
                    () => injector.ResetProvidedInstance<IDependency>()
                );
            }

            [Test]
            public void Restores_Singleton_Instance()
            {
                var injector = new DependencyInjector(DiContext.Current.Container);

                var singletonInstance = new DependencyOne();
                
                injector.SetDependency<IDependency>(singletonInstance);
                injector.SetDependency<IDependency>(new DependencyTwo());
                injector.ResetProvidedInstance<IDependency>();

                Assert.That(injector.Provide<IDependency>(), Is.SameAs(singletonInstance));
            }

            [Test]
            public void Creates_New_Transient_Instance()
            {
                var injector = new DependencyInjector(DiContext.Current.Container);
                var transientInstance = new DependencyOne();
                
                injector.SetDependency<IDependency>(transientInstance, Scope.Transient);
                injector.SetDependency<IDependency>(new DependencyTwo());
                injector.ResetProvidedInstance<IDependency>();
                
                Assert.That(injector.Provide<IDependency>(), Is.InstanceOf<DependencyOne>());
                Assert.That(injector.Provide<IDependency>(), Is.Not.SameAs(transientInstance));
            }
        }
    }
}
