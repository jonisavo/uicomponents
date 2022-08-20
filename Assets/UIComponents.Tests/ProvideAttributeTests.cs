using NSubstitute;
using NUnit.Framework;
using UIComponents.Experimental;
using UIComponents.Testing;

namespace UIComponents.Tests
{
    [TestFixture]
    public class ProvideAttributeTests
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
        private class ComponentWithDependencies : UIComponent
        {
            [Provide]
            public readonly IStringProperty StringProperty;
            [Provide]
            public readonly IFloatProperty FloatProperty;
        }
        
        [Test]
        public void Provides_Dependencies_Automatically()
        {
            var component = new ComponentWithDependencies();
            Assert.That(component.StringProperty, Is.InstanceOf<StringClass>());
            Assert.That(component.FloatProperty, Is.InstanceOf<FloatClass>());
        }
        
        private class ComponentWithInvalidDependency : UIComponent
        {
            [Provide]
            public readonly IStringProperty StringProperty;
        }
        
        [Test]
        public void Does_Not_Throw_When_Provider_Is_Missing()
        {
            var logger = Substitute.For<IUIComponentLogger>();
            var testBed = TestBed.Create()
                .WithSingleton(logger)
                .Build();
            
            var component = testBed.CreateComponent<ComponentWithInvalidDependency>();

            Assert.That(component.StringProperty, Is.Null);
            logger.Received().LogError("Could not provide IStringProperty to StringProperty", component);
        }
    }
}