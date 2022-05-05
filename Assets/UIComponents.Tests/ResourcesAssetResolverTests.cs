using NUnit.Framework;
using UIComponents.Core;
using UnityEngine.UIElements;

namespace UIComponents.Tests
{
    [TestFixture]
    public class ResourcesAssetResolverTests : AssetResolverTestSuite<ResourcesAssetResolver>
    {
        [Test]
        public void Should_Be_Able_To_Load_Existing_Asset_Synchronously()
        {
            Assert_Loads_Existing_Asset<VisualTreeAsset>(
                "UIComponentTests/LayoutAttributeTests"
            );
        }

        [Test]
        public void Should_Be_Able_To_Tell_If_Asset_Exists()
        {
            Assert_Tells_If_Asset_Exists(
                "UIComponentTests/LayoutAttributeTests"
            );
        }
    }
}