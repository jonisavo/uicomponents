using System.Linq;
using NSubstitute;
using NUnit.Framework;
using UIComponents.Core;

[assembly: AssetPath("AssemblyPath1")]
[assembly: AssetPath("AssemblyPath2")]

namespace UIComponents.Tests
{
    [TestFixture]
    public class AssetPathAttributeTests
    {
        [AssetPath("ComponentPath1")]
        [AssetPath("ComponentPath2")]
        private class UIComponentWithAssetPaths : UIComponent {}
        
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

            Assert.That(assetPaths.Count, Is.EqualTo(4));
            
            Assert.That(assetPaths[0], Is.EqualTo("ComponentPath1"));
            Assert.That(assetPaths[1], Is.EqualTo("ComponentPath2"));
            Assert.That(assetPaths[2], Is.EqualTo("AssemblyPath1"));
            Assert.That(assetPaths[3], Is.EqualTo("AssemblyPath2"));
        }
    }
}