using System.Collections;
using NUnit.Framework;
using UIComponents.Testing;
using UIComponents.Tests.Utilities;
using UnityEngine.TestTools;

namespace UIComponents.Tests
{
    [TestFixture]
    public class RootClassAttributeTests
    {
        [RootClass("test-class")]
        private class ComponentWithRootClass : UIComponent {}

        private TestBed _testBed;
        
        [SetUp]
        public void SetUp()
        {
            _testBed = TestBed.Create().Build();
        }
        
        [UnityTest]
        public IEnumerator Adds_Class_To_Component()
        {
            var component = _testBed.CreateComponent<ComponentWithRootClass>();
            yield return component.WaitForInitialization().AsEnumerator();
            
            Assert.That(component.ClassListContains("test-class"), Is.True);
        }
        
        [RootClass("other-test-class")]
        [RootClass("final-test-class")]
        private class ChildComponentWithRootClass : ComponentWithRootClass {}
        
        [UnityTest]
        public IEnumerator Adds_Class_To_Component_And_Child_Component()
        {
            var component = _testBed.CreateComponent<ChildComponentWithRootClass>();
            yield return component.WaitForInitialization().AsEnumerator();
            
            Assert.That(component.ClassListContains("test-class"), Is.True);
            Assert.That(component.ClassListContains("other-test-class"), Is.True);
            Assert.That(component.ClassListContains("final-test-class"), Is.True);
        }
    }
}
