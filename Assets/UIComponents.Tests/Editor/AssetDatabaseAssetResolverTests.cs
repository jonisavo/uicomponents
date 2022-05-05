using NUnit.Framework;
using UIComponents.Editor;
using UnityEngine.UIElements;

namespace UIComponents.Tests.Editor
{
    [TestFixture]
    public class AssetDatabaseAssetResolverTests : AssetResolverTestSuite<AssetDatabaseAssetResolver>
    {
        [Test]
        public void Should_Be_Able_To_Load_Existing_Asset()
        {
            Assert_Loads_Existing_Asset<VisualTreeAsset>(
                "Assets/UIComponents.Tests/Resources/UIComponentTests/LayoutAttributeTests.uxml"
            );
        }

        [Test]
        public void Should_Be_Able_To_Tell_If_Asset_Exists()
        {
            Assert_Tells_If_Asset_Exists(
                "Assets/UIComponents.Tests/Resources/UIComponentTests/LayoutAttributeTests.uxml"
            );
        }
    }
}