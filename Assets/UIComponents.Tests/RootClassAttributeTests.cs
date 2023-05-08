using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace UIComponents.Tests
{
    [TestFixture]
    public partial class RootClassAttributeTests
    {
        [RootClass("test-class")]
        private partial class ComponentWithRootClass : UIComponent {}
        
        [UnityTest]
        public IEnumerator Adds_Class_To_Component()
        {
            var component = new ComponentWithRootClass();
            component.Initialize();
            yield return component.WaitForInitializationEnumerator();
            
            Assert.That(component.ClassListContains("test-class"), Is.True);
        }
        
        [RootClass("other-test-class")]
        [RootClass("final-test-class")]
        private partial class ChildComponentWithRootClass : ComponentWithRootClass {}
        
        [UnityTest]
        public IEnumerator Adds_Class_To_Component_And_Child_Component()
        {
            var component = new ChildComponentWithRootClass();
            component.Initialize();
            yield return component.WaitForInitializationEnumerator();
            
            Assert.That(component.ClassListContains("test-class"), Is.True);
            Assert.That(component.ClassListContains("other-test-class"), Is.True);
            Assert.That(component.ClassListContains("final-test-class"), Is.True);
        }
    }
}
