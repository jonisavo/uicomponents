using System.Collections;
using NSubstitute;
using NUnit.Framework;
using UIComponents.Testing;
using UnityEngine.TestTools;

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
            [Provide(CastFrom = typeof(IFloatProperty))]
            public readonly FloatClass FloatClassInstance;
        }

        private TestBed _testBed;
        private IUIComponentLogger _mockLogger;

        [SetUp]
        public void SetUp()
        {
            _mockLogger = Substitute.For<IUIComponentLogger>();
            _testBed = TestBed.Create()
                .WithSingleton(_mockLogger)
                .Build();
        }
        
        [UnityTest]
        public IEnumerator Provides_Dependencies_Automatically()
        {
            var component = _testBed.CreateComponent<ComponentWithDependencies>();

            yield return component.WaitForInitializationEnumerator();
            
            Assert.That(component.StringProperty, Is.InstanceOf<StringClass>());
            Assert.That(component.FloatProperty, Is.InstanceOf<FloatClass>());
        }

        [UnityTest]
        public IEnumerator Allows_Providing_Dependencies_With_A_Cast()
        {
            var component = _testBed.CreateComponent<ComponentWithDependencies>();

            yield return component.WaitForInitializationEnumerator();
            
            Assert.That(component.FloatClassInstance, Is.InstanceOf<FloatClass>());
        }
        
        [Dependency(typeof(IFloatProperty), provide: typeof(FloatClass))]
        private class ComponentWithInvalidDependency : UIComponent
        {
            [Provide]
            public readonly IStringProperty StringProperty;

            [Provide(CastFrom = typeof(IFloatProperty))]
            public readonly StringClass StringClassInstance;
        }
        
        [UnityTest]
        public IEnumerator Logs_Error_When_Provider_Is_Missing()
        {
            var component = _testBed.CreateComponent<ComponentWithInvalidDependency>();
            
            yield return component.WaitForInitializationEnumerator();

            Assert.That(component.StringProperty, Is.Null);
            _mockLogger.Received().LogError("Could not provide IStringProperty to StringProperty", component);
        }

        [UnityTest]
        public IEnumerator Logs_Error_On_Invalid_Cast()
        {
            var component = _testBed.CreateComponent<ComponentWithInvalidDependency>();
            
            yield return component.WaitForInitializationEnumerator();
            
            Assert.That(component.StringClassInstance, Is.Null);
            
            _mockLogger.Received().LogError("Could not cast IFloatProperty to StringClass", component);
        }
    }
}
