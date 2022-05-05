using System.Linq;
using NSubstitute;
using NUnit.Framework;

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
        
        private IAssetResolver _resolver;
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _resolver = Substitute.For<IAssetResolver>();
            DependencyInjector.SetDependency<UIComponentWithAssetPaths, IAssetResolver>(_resolver);
        }
        
        [TearDown]
        public void TearDown()
        {
            _resolver.ClearReceivedCalls();
        }

        [Test]
        public void Specified_Asset_Paths_Are_Stored()
        {
            var component = new UIComponentWithAssetPaths();

            var assetPaths = component.GetAssetPaths().ToList();

            Assert.That(assetPaths.Count, Is.EqualTo(2));
            
            Assert.That(assetPaths[0], Is.EqualTo("Path1"));
            Assert.That(assetPaths[1], Is.EqualTo("Path2"));
        }

        [Test]
        public void Asset_Path_Are_Inherited_From_Parent_Classes()
        {
            var component = new UIComponentSubclassWithAssetPaths();

            var assetPaths = component.GetAssetPaths().ToList();
            
            Assert.That(assetPaths.Count, Is.EqualTo(4));
            
            Assert.That(assetPaths[0], Is.EqualTo("Path3"));
            Assert.That(assetPaths[1], Is.EqualTo("Path4"));
            Assert.That(assetPaths[2], Is.EqualTo("Path1"));
            Assert.That(assetPaths[3], Is.EqualTo("Path2"));
        }
    }
}