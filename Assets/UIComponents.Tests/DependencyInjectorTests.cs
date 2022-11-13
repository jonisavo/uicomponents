using NUnit.Framework;
using UIComponents.DependencyInjection;
using UIComponents.Tests.Utilities;

namespace UIComponents.Tests
{
    [TestFixture]
    public class DependencyInjectorTests
    {
        private interface IMockDependency {}

        public class DependencyOne : IMockDependency {}

        public class DependencyTwo : IMockDependency {}

        private IDependencyConsumer _mockConsumer;
        private DiContext _diContext;

        [SetUp]
        public void SetUp()
        {
            _mockConsumer = MockUtilities.CreateDependencyConsumer(new IDependency[]
            {
                new Dependency<IMockDependency>(Scope.Singleton, () => new DependencyOne())
            });

            _diContext = new DiContext();
            _diContext.RegisterConsumer(_mockConsumer);
        }

        [Test]
        public void Can_Be_Created_Without_Consumer()
        {
            var injector = new DependencyInjector(_diContext);
            
            Assert.That(injector.HasConsumer, Is.False);
        }

        [Test]
        public void Can_Be_Created_Using_Consumer()
        {
            var injector = new DependencyInjector(_mockConsumer, _diContext);

            Assert.That(injector.HasConsumer, Is.True);
        }

        [TestFixture]
        public class Provide
        {
            private DiContext _diContext;
            private IDependencyConsumer _mockConsumer;
            private IDependencyConsumer _mockConsumerWithTransientOverride;

            [SetUp]
            public void SetUp()
            {
                _diContext = new DiContext();
                _mockConsumer = MockUtilities.CreateDependencyConsumer(new IDependency[]
                {
                    new Dependency<IMockDependency>(Scope.Singleton, () => new DependencyOne())
                });
                _mockConsumerWithTransientOverride = MockUtilities.CreateDependencyConsumer(new IDependency[]
                {
                    new Dependency<IMockDependency>(Scope.Transient, () => new DependencyOne()),
                    new Dependency<IMockDependency>(Scope.Singleton, () => new DependencyTwo())
                });
                _diContext.RegisterConsumer(_mockConsumer);
                _diContext.RegisterConsumer(_mockConsumerWithTransientOverride);
            }
            
            [Test]
            public void Returns_Desired_Dependency()
            {
                var injector = new DependencyInjector(_mockConsumer, _diContext);
                Assert.That(injector.Provide<IMockDependency>(), Is.InstanceOf<DependencyOne>());
            }

            [Test]
            public void Returns_Transient_Dependency_If_Singleton_And_Transient_Are_Registered()
            {
                var injector = new DependencyInjector(_mockConsumerWithTransientOverride, _diContext);
                Assert.That(injector.Provide<IMockDependency>(), Is.InstanceOf<DependencyOne>()); 
            }

            // [Test]
            // public void Returns_Desired_Dependency_With_Non_Generic_Method()
            // {
            //     var injector = new DependencyInjector(DiContext.Current.Container);
            //     injector.SetDependency<IDependency>(new DependencyOne());
            //     Assert.That(injector.Provide(typeof(IDependency)), Is.InstanceOf<DependencyOne>());
            // }

            private interface IMissingDependency {}
            
            [Test]
            public void Throws_If_No_Provider_Exists()
            {
                var injector = new DependencyInjector(_mockConsumer, _diContext);

                var exception = Assert.Throws<MissingProviderException>(
                    () => injector.Provide<IMissingDependency>()
                );
                
                Assert.That(exception.Message, Is.EqualTo("No provider found for IMissingDependency"));
            }
            
            // [Test]
            // public void Throws_If_No_Provider_Exists_With_Non_Generic_Method()
            // {
            //     var injector = new DependencyInjector(DiContext.Current.Container);
            //
            //     var exception = Assert.Throws<MissingProviderException>(
            //         () => injector.Provide(typeof(IDependency))
            //     );
            //     
            //     Assert.That(exception.Message, Is.EqualTo("No provider found for IDependency"));
            // }
        }

        [TestFixture]
        public class TryProvide
        {
            private DiContext _diContext;
            private IDependencyConsumer _mockConsumer;
            
            [SetUp]
            public void SetUp()
            {
                _diContext = new DiContext();
                _mockConsumer = MockUtilities.CreateDependencyConsumer(new IDependency[]
                {
                    new Dependency<IMockDependency>(Scope.Singleton, () => new DependencyOne())
                });
                _diContext.RegisterConsumer(_mockConsumer);
            }

            private interface IMissingDependency {}

            [Test]
            public void Returns_If_Dependency_Could_Be_Provided()
            {
                var injector = new DependencyInjector(_mockConsumer, _diContext);
                
                Assert.That(injector.TryProvide<IMissingDependency>(out _), Is.False);

                var wasProvided = injector.TryProvide<IMockDependency>(out var instance);
                
                Assert.That(wasProvided, Is.True);
                Assert.That(instance, Is.InstanceOf<DependencyOne>());
            }

            [Test]
            public void Yields_Null_If_Dependency_Can_Not_Be_Provided()
            {
                var injector = new DependencyInjector(_mockConsumer, _diContext);

                injector.TryProvide<IMissingDependency>(out var instance);
                
                Assert.That(instance, Is.Null);
            }
        }

        [TestFixture]
        public class SetConsumer
        {
            private DiContext _diContext;
            private IDependencyConsumer _firstConsumer;
            private IDependencyConsumer _secondConsumer;
            
            private interface ISingletonDependency {}
            private class SingletonDependency : ISingletonDependency {}

            [SetUp]
            public void SetUp()
            {
                _diContext = new DiContext();
                _firstConsumer = MockUtilities.CreateDependencyConsumer(new IDependency[]
                {
                    new Dependency<IMockDependency>(Scope.Transient, () => new DependencyOne())
                });
                _secondConsumer = MockUtilities.CreateDependencyConsumer(new IDependency[]
                {
                    new Dependency<IMockDependency>(Scope.Transient, () => new DependencyTwo()),
                    new Dependency<ISingletonDependency>(Scope.Singleton, () => new SingletonDependency())
                });
                _diContext.RegisterConsumer(_firstConsumer);
                _diContext.RegisterConsumer(_secondConsumer);
            }

            [Test]
            public void Refreshes_Transient_Dependencies()
            {
                var injector = new DependencyInjector(_firstConsumer, _diContext);
                
                injector.SetConsumer(_secondConsumer);
                
                Assert.That(injector.Provide<IMockDependency>(), Is.InstanceOf<DependencyTwo>());
            }
        }

        [TestFixture]
        public class SetTransientInstance
        {
            private DiContext _diContext;
            private IDependencyConsumer _mockConsumer;
            
            [SetUp]
            public void SetUp()
            {
                _diContext = new DiContext();
                _mockConsumer = MockUtilities.CreateDependencyConsumer(new IDependency[]
                {
                    new Dependency<IMockDependency>(Scope.Transient, () => new DependencyOne())
                });
                _diContext.RegisterConsumer(_mockConsumer);
            }
            
            [Test]
            public void Switches_A_Transient_Instance()
            {
                var injector = new DependencyInjector(_mockConsumer, _diContext);
                
                injector.SetTransientInstance<IMockDependency>(new DependencyTwo());

                Assert.That(injector.Provide<IMockDependency>(), Is.InstanceOf<DependencyTwo>());
            }
        }
        
        // [TestFixture]
        // public class SetDependency
        // {
        //     [Test]
        //     public void Switches_The_Dependency()
        //     {
        //         var injector = new DependencyInjector(DiContext.Current.Container);
        //         
        //         injector.SetDependency<IDependency>(new DependencyOne());
        //         injector.SetDependency<IDependency>(new DependencyTwo());
        //         
        //         Assert.That(injector.Provide<IDependency>(), Is.InstanceOf<DependencyTwo>());
        //     }
        //
        //     [Test]
        //     public void Throws_Exception_If_Null_Is_Given_As_Parameter()
        //     {
        //         var injector = new DependencyInjector(DiContext.Current.Container);
        //
        //         Assert.Throws<ArgumentNullException>(
        //             () => injector.SetDependency<IDependency>(null)
        //         );
        //     }
        // }

        // [TestFixture]
        // public class ClearDependency
        // {
        //     [Test]
        //     public void Removes_Dependency_Instance()
        //     {
        //         var injector = new DependencyInjector(DiContext.Current.Container);
        //         
        //         injector.SetDependency<IDependency>(new DependencyOne());
        //         
        //         injector.ClearDependency<IDependency>();
        //
        //         Assert.Throws<MissingProviderException>(() => injector.Provide<IDependency>());
        //     }
        //
        //     [Test]
        //     public void Does_Not_Throw_If_Dependency_Does_Not_Exist()
        //     {
        //         var injector = new DependencyInjector(DiContext.Current.Container);
        //         
        //         Assert.DoesNotThrow(() => injector.ClearDependency<IDependency>());
        //     }
        // }

        // [TestFixture]
        // public class ResetProvidedInstance
        // {
        //     [SetUp]
        //     public void SetUp()
        //     {
        //         DiContext.Current.Container.Clear();
        //     }
        //
        //     [Test]
        //     public void Throws_If_No_Default_Dependency_Exists()
        //     {
        //         var injector = new DependencyInjector(DiContext.Current.Container);
        //         
        //         Assert.Throws<InvalidOperationException>(
        //             () => injector.ResetProvidedInstance<IDependency>()
        //         );
        //         
        //         var initialDependency = new DependencyOne();
        //         
        //         injector.SetDependency<IDependency>(initialDependency);
        //         
        //         Assert.DoesNotThrow(
        //             () => injector.ResetProvidedInstance<IDependency>()
        //         );
        //     }
        //
        //     [Test]
        //     public void Restores_Singleton_Instance()
        //     {
        //         var injector = new DependencyInjector(DiContext.Current.Container);
        //
        //         var singletonInstance = new DependencyOne();
        //         
        //         injector.SetDependency<IDependency>(singletonInstance);
        //         injector.SetDependency<IDependency>(new DependencyTwo());
        //         injector.ResetProvidedInstance<IDependency>();
        //
        //         Assert.That(injector.Provide<IDependency>(), Is.SameAs(singletonInstance));
        //     }
        //
        //     [Test]
        //     public void Creates_New_Transient_Instance()
        //     {
        //         var injector = new DependencyInjector(DiContext.Current.Container);
        //         var transientInstance = new DependencyOne();
        //         
        //         injector.SetDependency<IDependency>(transientInstance, Scope.Transient);
        //         injector.SetDependency<IDependency>(new DependencyTwo());
        //         injector.ResetProvidedInstance<IDependency>();
        //         
        //         Assert.That(injector.Provide<IDependency>(), Is.InstanceOf<DependencyOne>());
        //         Assert.That(injector.Provide<IDependency>(), Is.Not.SameAs(transientInstance));
        //     }
        // }
    }
}
