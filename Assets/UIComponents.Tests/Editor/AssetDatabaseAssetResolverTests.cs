using NUnit.Framework;
using UIComponents.Editor;

namespace UIComponents.Tests.Editor
{
    [TestFixture]
    public class AssetDatabaseAssetResolverTests : AssetResolverTestSuite<AssetDatabaseAssetResolver>
    {
        [Test]
        public void Should_Be_Able_To_Load_Existing_Asset()
        {
            Assert_Should_Be_Able_To_Load_Existing_Asset(
                "Assets/UIComponents.Tests/Resources/UIComponentTests/LayoutAttributeTests.uxml"
            );
        }

        [Test]
        public void Should_Be_Able_To_Tell_If_Asset_Exists()
        {
            Assert_Should_Be_Able_To_Tell_If_Asset_Exists(
                "Assets/UIComponents.Tests/Resources/UIComponentTests/LayoutAttributeTests.uxml"
            );
        }
    }
}