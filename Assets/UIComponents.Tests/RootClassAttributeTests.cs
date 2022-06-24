using NUnit.Framework;

namespace UIComponents.Tests
{
    [TestFixture]
    public class RootClassAttributeTests
    {
        [RootClass("test-class")]
        private class ComponentWithRootClass : UIComponent {}

        [Test]
        public void Adds_Class_To_Component()
        {
            var component = new ComponentWithRootClass();
            Assert.That(component.ClassListContains("test-class"), Is.True);
        }
        
        [RootClass("other-test-class")]
        [RootClass("final-test-class")]
        private class ChildComponentWithRootClass : ComponentWithRootClass {}
        
        [Test]
        public void Adds_Class_To_Component_And_Child_Component()
        {
            var component = new ChildComponentWithRootClass();
            Assert.That(component.ClassListContains("test-class"), Is.True);
            Assert.That(component.ClassListContains("other-test-class"), Is.True);
            Assert.That(component.ClassListContains("final-test-class"), Is.True);
        }
    }
}