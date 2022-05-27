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
            using var scope = new DependencyScope<TestComponent, IAssetResolver>(
                MockUtilities.CreateMockResolver()
            );
            Assert.That(UIComponent.TryGetCache<TestComponent>(out _), Is.False);
            var component = new TestComponent();
            Assert.That(UIComponent.TryGetCache<TestComponent>(out var cache), Is.True);
            Assert.That(cache.LayoutAttribute, Is.Not.Null);
            Assert.That(cache.StylesheetAttributes.Count, Is.EqualTo(1));
        }

        [Test]
        public void CacheCanBeCleared()
        {
            using var scope = new DependencyScope<TestComponent, IAssetResolver>(
                MockUtilities.CreateMockResolver()
            );
            var component = new TestComponent();
            Assert.That(UIComponent.TryGetCache<TestComponent>(out _), Is.True);
            UIComponent.ClearCache<TestComponent>();
            Assert.That(UIComponent.TryGetCache<TestComponent>(out _), Is.False);
        }
    }
}