using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace UIComponents.Tests
{
    [TestFixture]
    public class UIComponentTests
    {
        [TestFixture]
        public class GetTypeName
        {
            private class TestComponent : UIComponent {}

            [UnityTest]
            public IEnumerator It_Is_Initialized()
            {
                var component = new TestComponent();
                Assert.That(component.Initialized, Is.False);
                yield return component.WaitForInitializationEnumerator();
                Assert.That(component.Initialized, Is.True);
            }
            
            [Test]
            public void ShouldReturnTypeName()
            {
                var component = new TestComponent();
                Assert.That(nameof(TestComponent), Is.EqualTo(component.GetTypeName()));
            }
        }
    }
}
