using NUnit.Framework;

namespace UIComponents.Tests
{
    [TestFixture]
    public class UIComponentTests
    {
        [TestFixture]
        public class GetTypeName
        {
            private class TestComponent : UIComponent {}
            
            [Test]
            public void ShouldReturnTypeName()
            {
                var component = new TestComponent();
                Assert.That(nameof(TestComponent), Is.EqualTo(component.GetTypeName()));
            }
        }
    }
}