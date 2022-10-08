using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIComponents.Tests.Roslyn
{
    [TestFixture]
    public class UxmlNameTests
    {
        [Test]
        public void Generates_Uxml_Factory_For_Name()
        {
            var layout = Resources.Load<VisualTreeAsset>("UxmlNameTestComponent");

            var container = new VisualElement();
            layout.CloneTree(container);

            var testElement = container.Q<UxmlNameTestComponent>();

            Assert.That(testElement, Is.Not.Null);
        }
    }
}
