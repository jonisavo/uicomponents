using System.Linq;
using NSubstitute;
using NUnit.Framework;
using UIComponents.Testing;
using UIComponents.Tests.Utilities;

namespace UIComponents.Tests
{
    [TestFixture]
    public class AssetPathAttributeTests
    {
        [AssetPath("Path1")]
        [AssetPath("Path2")]
        private class UIComponentWithAssetPaths : UIComponent {}
        
        [AssetPath("Path3")]
        [AssetPath("Path4")]
        private class UIComponentSubclassWithAssetPaths : UIComponentWithAssetPaths {}

        private TestBed _testBed;
        private IAssetResolver _mockResolver;
        
        [SetUp]
        public void SetUp()
        {
            _mockResolver = MockUtilities.CreateMockResolver();
            _testBed = TestBed.Create()
                .WithSingleton(_mockResolver)
                .Build();
        }
        
        [TearDown]
        public void TearDown()
        {
            _mockResolver.ClearReceivedCalls();
        }

        [Test]
        public void Specified_Asset_Paths_Are_Stored()
        {
            var component = _testBed.CreateComponent<UIComponentWithAssetPaths>();

            var assetPaths = component.GetAssetPaths().ToList();

            Assert.That(assetPaths.Count, Is.EqualTo(2));
            
            Assert.That(assetPaths[0], Is.EqualTo("Path1"));
            Assert.That(assetPaths[1], Is.EqualTo("Path2"));
        }

        [Test]
        public void Asset_Path_Are_Inherited_From_Parent_Classes()
        {
            var component = _testBed.CreateComponent<UIComponentSubclassWithAssetPaths>();

            var assetPaths = component.GetAssetPaths().ToList();
            
            Assert.That(assetPaths.Count, Is.EqualTo(4));
            
            Assert.That(assetPaths[0], Is.EqualTo("Path3"));
            Assert.That(assetPaths[1], Is.EqualTo("Path4"));
            Assert.That(assetPaths[2], Is.EqualTo("Path1"));
            Assert.That(assetPaths[3], Is.EqualTo("Path2"));
        }
    }
}