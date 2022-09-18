using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace UIComponents.Tests
{
    [TestFixture]
    public class ResourcesAssetResolverTests : AssetResolverTestSuite<ResourcesAssetResolver>
    {
        [UnityTest]
        public IEnumerator Should_Be_Able_To_Load_Existing_Asset()
        {
            yield return Assert_Loads_Existing_Asset<VisualTreeAsset>(
                "UIComponentTests/LayoutAttributeTests"
            );
        }

        [UnityTest]
        public IEnumerator Should_Be_Able_To_Tell_If_Asset_Exists()
        {
            yield return Assert_Tells_If_Asset_Exists(
                "UIComponentTests/LayoutAttributeTests"
            );
        }
    }
}
