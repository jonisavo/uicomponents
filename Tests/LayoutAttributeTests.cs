using NUnit.Framework;
using UIComponents.Core;
using UnityEngine.UIElements;

namespace UIComponents.Tests
{
    [TestFixture]
    public class LayoutAttributeTests
    {
        [Layout("UIComponentTests/LayoutAttributeTests")]
        private class UIComponentWithLayout : UIComponent {}

        [Test]
        public void Given_Layout_Is_Loaded()
        {
            var component = new UIComponentWithLayout();
            
            Assert.That(component.childCount, Is.EqualTo(1));

            var label = component.Q<Label>();

            Assert.That(label, Is.Not.Null);
            Assert.That(label.text, Is.EqualTo("Hello world!"));
        }
    }
}