using NUnit.Framework;
using UIComponents.Testing;
using UIComponents.Tests.Utilities;

namespace UIComponents.Tests
{
    [TestFixture]
    public partial class UIComponentCacheTests
    {
        [Layout("TestLayout")]
        [Stylesheet("TestStylesheet")]
        private partial class TestComponent : UIComponent {}

        private TestBed _testBed;

        [SetUp]
        public void SetUp()
        {
            var resolver = MockUtilities.CreateMockResolver();
            
            _testBed = TestBed.Create()
                .WithSingleton(resolver)
                .Build();
        }

        [Test]
        public void CacheIsCreatedForComponent()
        {
            Assert.That(UIComponent.TryGetCache<TestComponent>(out _), Is.False);
            var component = _testBed.CreateComponent<TestComponent>();
            Assert.That(UIComponent.TryGetCache<TestComponent>(out var cache), Is.True);
        }

        [Test]
        public void CacheCanBeCleared()
        {
            var component = _testBed.CreateComponent<TestComponent>();
            Assert.That(UIComponent.TryGetCache<TestComponent>(out _), Is.True);
            UIComponent.ClearCache<TestComponent>();
            Assert.That(UIComponent.TryGetCache<TestComponent>(out _), Is.False);
        }
    }
}
