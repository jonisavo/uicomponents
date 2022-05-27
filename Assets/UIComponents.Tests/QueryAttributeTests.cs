using NUnit.Framework;
using UIComponents.Experimental;
using UnityEngine.UIElements;

namespace UIComponents.Tests
{
    [TestFixture]
    public class QueryAttributeTests
    {
        [Layout("UIComponentTests/LayoutAttributeTests")]
        private class ComponentWithQueryAttribute : UIComponent
        {
            [Query("hello-world-label")]
            public readonly Label HelloWorldLabel;
            
            [Query("test-foldout")]
            public readonly Foldout TestFoldout;
        }

        [Test]
        public void Should_Populate_Fields()
        {
            var component = new ComponentWithQueryAttribute();
            
            Assert.That(component.HelloWorldLabel, Is.InstanceOf<Label>());
            Assert.That(component.HelloWorldLabel.text, Is.EqualTo("Hello world!"));
            
            Assert.That(component.TestFoldout, Is.InstanceOf<Foldout>());
        }
    }
}