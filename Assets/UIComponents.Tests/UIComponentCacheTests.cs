using NUnit.Framework;
using UIComponents.Tests.Utilities;
using UIComponents.Utilities;

namespace UIComponents.Tests
{
    [TestFixture]
    public class UIComponentCacheTests
    {
        [Layout("TestLayout")]
        [Stylesheet("TestStylesheet")]
        private class TestComponent : UIComponent {}

        [Test]
        public void CacheIsCreatedForComponent()
        {
            var resolver = MockUtilities.CreateMockResolver();
            using (new DependencyScope<TestComponent, IAssetResolver>(resolver))
            {
                Assert.That(UIComponent.TryGetCache<TestComponent>(out _), Is.False);
                var component = new TestComponent();
                Assert.That(UIComponent.TryGetCache<TestComponent>(out var cache), Is.True);
                Assert.That(cache.LayoutAttribute, Is.Not.Null);
                Assert.That(cache.StylesheetAttributes.Count, Is.EqualTo(1));
            }
        }

        [Test]
        public void CacheCanBeCleared()
        {
            var resolver = MockUtilities.CreateMockResolver();
            using (new DependencyScope<TestComponent, IAssetResolver>(resolver))
            {
                var component = new TestComponent();
                Assert.That(UIComponent.TryGetCache<TestComponent>(out _), Is.True);
                UIComponent.ClearCache<TestComponent>();
                Assert.That(UIComponent.TryGetCache<TestComponent>(out _), Is.False);
            }
        }
    }
}