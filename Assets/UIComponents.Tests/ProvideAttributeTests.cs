using NSubstitute;
using NUnit.Framework;
using UIComponents.Testing;

namespace UIComponents.Tests
{
    [TestFixture]
    public partial class ProvideAttributeTests
    {
        private interface IStringProperty
        {
            string Property { get; set; }
        }
        
        private class StringClass : IStringProperty
        {
            public string Property { get; set; }
        }
        
        private interface IFloatProperty
        {
            float Property { get; set; }
        }
        
        private class FloatClass : IFloatProperty
        {
            public float Property { get; set; }
        }

        [Dependency(typeof(IStringProperty), provide: typeof(StringClass))]
        [Dependency(typeof(IFloatProperty), provide: typeof(FloatClass))]
        private partial class ComponentWithDependencies : UIComponent
        {
            [Provide]
            public IStringProperty StringProperty;
            [Provide]
            public IFloatProperty FloatProperty;
            [Provide(CastFrom = typeof(IFloatProperty))]
            public FloatClass FloatClassInstance;
        }

        private ILogger _mockLogger;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = Substitute.For<ILogger>();
        }
        
        [Test]
        public void Provides_Dependencies_Automatically()
        {
            var testBed = new TestBed<ComponentWithDependencies>()
                .WithSingleton(_mockLogger);
            var component = testBed.CreateComponent();
            Assert.That(component.StringProperty, Is.InstanceOf<StringClass>());
            Assert.That(component.FloatProperty, Is.InstanceOf<FloatClass>());
        }

        [Test]
        public void Allows_Providing_Dependencies_With_A_Cast()
        {
            var testBed = new TestBed<ComponentWithDependencies>()
                .WithSingleton(_mockLogger);
            var component = testBed.CreateComponent();
            Assert.That(component.FloatClassInstance, Is.InstanceOf<FloatClass>());
        }
        
        [Dependency(typeof(IFloatProperty), provide: typeof(FloatClass))]
        private partial class ComponentWithInvalidDependency : UIComponent
        {
            [Provide]
            public IStringProperty StringProperty;

            [Provide(CastFrom = typeof(IFloatProperty))]
            public StringClass StringClassInstance;
        }
        
        [Test]
        public void Logs_Error_When_Provider_Is_Missing()
        {
            var testBed = new TestBed<ComponentWithInvalidDependency>()
                .WithSingleton(_mockLogger);
            var component = testBed.CreateComponent();
            Assert.That(component.StringProperty, Is.Null);
            _mockLogger.Received().LogError("Could not provide IStringProperty to StringProperty", component);
        }

        [Test]
        public void Logs_Error_On_Invalid_Cast()
        {
            var testBed = new TestBed<ComponentWithInvalidDependency>()
                .WithSingleton(_mockLogger);
            var component = testBed.CreateComponent();
            Assert.That(component.StringClassInstance, Is.Null);
            _mockLogger.Received().LogError("Could not cast IFloatProperty to StringClass", component);
        }
    }
}
