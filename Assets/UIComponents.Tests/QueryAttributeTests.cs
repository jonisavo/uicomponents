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

        private class ChildComponentWithQueryAttribute : ComponentWithQueryAttribute
        {
            [Query("foldout-content")]
            public readonly Label FoldoutContent;
        }
        
        [Test]
        public void Should_Populate_Inherited_Fields()
        {
            var component = new ChildComponentWithQueryAttribute();
            
            Assert.That(component.HelloWorldLabel, Is.InstanceOf<Label>());
            Assert.That(component.TestFoldout, Is.InstanceOf<Foldout>());
            Assert.That(component.FoldoutContent, Is.InstanceOf<Label>());
            Assert.That(component.FoldoutContent.text, Is.EqualTo("Foldout content"));
        }
    }
}