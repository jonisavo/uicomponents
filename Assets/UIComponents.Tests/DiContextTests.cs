using System;
using NSubstitute;
using NUnit.Framework;
using UIComponents.DependencyInjection;
using UIComponents.Tests.Utilities;

namespace UIComponents.Tests
{
    [TestFixture]
    public class DiContextTests
    {
        [Test]
        public void Allows_Changing_Current_Context()
        {
            var previousContext = DiContext.Current;

            try
            {
                var context = new DiContext();
                DiContext.ChangeCurrent(context);
                Assert.That(DiContext.Current, Is.SameAs(context));
            }
            finally
            {
                DiContext.ChangeCurrent(previousContext);
            }
        }

        [Test]
        public void Does_Not_Allow_Changing_To_Null_Context()
        {
            Assert.Throws<ArgumentNullException>(() => DiContext.ChangeCurrent(null));
        }

        [Test]
        public void Allows_Getting_Singleton_Instances()
        {
            var diContext = new DiContext();
            
            diContext.SingletonInstanceDictionary.Add(typeof(string), "Hello world");

            var instances = diContext.GetSingletonInstances();
            
            Assert.That(instances[typeof(string)], Is.EqualTo("Hello world"));
        }

        [Test]
        public void Allows_Resetting_Context()
        {
            var diContext = new DiContext();
            
            diContext.SingletonInstanceDictionary.Add(typeof(string), "Hello world");
            diContext.InjectorDictionary.Add(typeof(string), new DependencyInjector(diContext));
            
            diContext.Clear();
            
            Assert.That(diContext.SingletonInstanceDictionary.Count, Is.EqualTo(0));
            Assert.That(diContext.InjectorDictionary.Count, Is.EqualTo(0));
        }

        [Test]
        public void Allows_Checking_Whether_An_Initialized_Injector_Exists()
        {
            var diContext = new DiContext();

            Assert.That(diContext.HasInjector(typeof(string)), Is.False);

            var injector = new DependencyInjector(diContext);

            diContext.InjectorDictionary.Add(typeof(string), injector);

            Assert.That(diContext.HasInjector(typeof(string)), Is.False);

            var consumer = Substitute.For<IDependencyConsumer>();

            injector.SetConsumer(consumer);

            Assert.That(diContext.HasInjector(typeof(string)), Is.True);
        }

        [TestFixture]
        public class GetInjector
        {
            private class Consumer {}
            
            [Test]
            public void It_Creates_A_New_Injector_If_One_Does_Not_Exist()
            {
                var diContext = new DiContext();

                var injector = diContext.GetInjector(typeof(Consumer));

                var injectionIsInContext = diContext.InjectorDictionary.ContainsValue(injector);
                
                Assert.That(injectionIsInContext);
            }

            [Test]
            public void It_Yields_Existing_Injector()
            {
                var diContext = new DiContext();

                var injector = new DependencyInjector(diContext);
                
                diContext.InjectorDictionary.Add(typeof(Consumer), injector);

                var receivedInjector = diContext.GetInjector(typeof(Consumer));
                
                Assert.That(receivedInjector, Is.SameAs(injector));
            }

            [Test]
            public void It_Throws_If_Consumer_Type_Is_Null()
            {
                var diContext = new DiContext();

                Assert.Throws<ArgumentNullException>(() => diContext.GetInjector(null));
            }
        }

        [TestFixture]
        public class RegisterConsumer
        {
            private interface IMockDependency {}
            public class MockDependency : IMockDependency {}
            
            [Test]
            public void It_Populates_Singleton_Instances()
            {
                var diContext = new DiContext();

                var consumer = MockUtilities.CreateDependencyConsumer(new IDependency[]
                {
                    new Dependency<IMockDependency, MockDependency>(Scope.Singleton, () => new MockDependency())
                });
                
                diContext.RegisterConsumer(consumer);

                var contextHasSingletonInstance =
                    diContext.SingletonInstanceDictionary.ContainsKey(typeof(MockDependency));
                
                Assert.That(contextHasSingletonInstance);
            }
        }
    }
}