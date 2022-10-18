using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

namespace UIComponents.Tests.Roslyn
{
    [TestFixture]
    public class UxmlTraitTests
    {
        [Test]
        public void Generates_Traits_For_Non_UIComponent_Class()
        {
            var layout = Resources.Load<VisualTreeAsset>("RoslynTestVisualElement");

            var container = new VisualElement();
            layout.CloneTree(container);

            var testElement = container.Q<RoslynTestVisualElement>();

            Assert.That(testElement.Description, Is.EqualTo("None set"));
            Assert.That(testElement.Age, Is.EqualTo(20));
            Assert.That(testElement.MusicVolume, Is.EqualTo(0.5f));
            Assert.That(testElement.UnixTimestamp, Is.EqualTo(1234567890));
        }

        [Test]
        public void Generates_Traits_For_UIComponent_Class()
        {
            var layout = Resources.Load<VisualTreeAsset>("RoslynTestComponent");
            var container = new VisualElement();
            layout.CloneTree(container);

            var component = container.Q<RoslynTestComponent>();
            
            Assert.That(component.TextColor, Is.EqualTo(Color.red));
            Assert.That(component.Greeting, Is.EqualTo(RoslynTestComponent.Greetings.Morning));
            Assert.That(component.CurrentTime, Is.EqualTo(123.456));
            Assert.That(component.Enabled, Is.EqualTo(false));
        }

        [Test]
        public void Generates_Traits_For_Nested_Class()
        {
            var layout = Resources.Load<VisualTreeAsset>("NestedRoslynTestComponent");
            var container = new VisualElement();
            layout.CloneTree(container);

            var component = container.Q<NestParentClass.NestedRoslynTestComponent>();
            
            Assert.That(component.Trait, Is.EqualTo("Hello world"));
        }
    }
}
