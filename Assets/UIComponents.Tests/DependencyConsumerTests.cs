using NUnit.Framework;
using UIComponents.DependencyInjection;
using UIComponents.Testing;

namespace UIComponents.Tests
{
    [TestFixture]
    public partial class DependencyConsumerTests
    {
        private interface IStringDependency
        {
            string GetValue();
        }
        
        private interface IIntDependency
        {
            int GetValue();
        }

        public class StringProvider : IStringDependency
        {
            public string GetValue() => "Hello world";
        }

        public class EmptyStringProvider : IStringDependency
        {
            public string GetValue() => "";
        }

        [Dependency(typeof(IStringDependency), provide: typeof(StringProvider))]
        private partial class StringDependencyConsumer : DependencyConsumer
        {
            [Provide]
            public IStringDependency StringDependency;

            public bool TryProvideStringDependency(out IStringDependency dependency)
            {
                return TryProvide(out dependency);
            }
            
            public bool TryProvideIntDependency(out IIntDependency dependency)
            {
                return TryProvide(out dependency);
            }
        }

        [Dependency(typeof(IStringDependency), provide: typeof(EmptyStringProvider))]
        private partial class EmptyStringDependencyConsumer : StringDependencyConsumer {}

        [Test]
        public void The_Correct_Class_Is_Provided_To_StringDependencyConsumer()
        {
            var consumer = new StringDependencyConsumer();
            Assert.That(consumer.StringDependency, Is.InstanceOf<StringProvider>());
        }
        
        [Test]
        public void The_Correct_Class_Is_Provided_To_EmptyStringDependencyConsumer()
        {
            var consumer = new EmptyStringDependencyConsumer();
            Assert.That(consumer.StringDependency, Is.InstanceOf<EmptyStringProvider>());
        }
        
        private class EmptyDependencyConsumer : DependencyConsumer {}

        [Test]
        public void DependencyConsumers_Have_No_Dependencies_By_Default()
        {
            var consumer = new EmptyDependencyConsumer();
            Assert.That(consumer.GetDependencies(), Is.Empty);
        }

        [Test]
        public void TryProvide_Works_For_EmptyStringDependencyConsumer()
        {
            var consumer = new EmptyStringDependencyConsumer();
            var couldProvideStringDependency = consumer.TryProvideStringDependency(out var stringDependency);
            var couldProvideIntDependency = consumer.TryProvideIntDependency(out var intDependency);
            
            Assert.That(couldProvideStringDependency, Is.True);
            Assert.That(stringDependency, Is.InstanceOf<EmptyStringProvider>());
            Assert.That(couldProvideIntDependency, Is.False);
            Assert.That(intDependency, Is.Null);
        }

        [Test]
        public void TestBed_Works_With_DependencyConsumers()
        {
            var stringProvider = new StringProvider();

            var consumer = new TestBed<StringDependencyConsumer>()
                .WithSingleton<IStringDependency>(stringProvider)
                .Instantiate();
            
            Assert.That(consumer.StringDependency, Is.SameAs(stringProvider));
        }
        
        [Dependency(typeof(StringProvider))]
        private partial class ConsumerWithConcreteDependency : DependencyConsumer
        {
            public readonly StringProvider StringProvider;
            
            [Provide]
            public StringProvider ProvidedStringProvider;
            
            public ConsumerWithConcreteDependency()
            {
                StringProvider = Provide<StringProvider>();
            }
        }
        
        [Test]
        public void Concrete_Dependencies_Are_Provided()
        {
            var consumer = new ConsumerWithConcreteDependency();
            var consumer2 = new ConsumerWithConcreteDependency();
            
            Assert.That(consumer.StringProvider, Is.InstanceOf<StringProvider>());
            Assert.That(consumer.ProvidedStringProvider, Is.InstanceOf<StringProvider>());
            Assert.That(consumer.StringProvider, Is.SameAs(consumer2.StringProvider));
            Assert.That(consumer.ProvidedStringProvider, Is.SameAs(consumer2.ProvidedStringProvider));
        }
    }
}