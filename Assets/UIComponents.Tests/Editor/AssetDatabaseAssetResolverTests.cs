using System.Collections;
using NUnit.Framework;
using UIComponents.Editor;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace UIComponents.Tests.Editor
{
    [TestFixture]
    public class AssetDatabaseAssetResolverTests : AssetResolverTestSuite<AssetDatabaseAssetResolver>
    {
        [UnityTest]
        public IEnumerator Should_Be_Able_To_Load_Existing_Asset()
        {
            yield return Assert_Loads_Existing_Asset<StyleSheet>(
                "Assets/UIComponents.Tests/Addressables/Assets/Component.uss"
            );
            yield return Assert_Loads_Existing_Asset<VisualTreeAsset>(
                "Assets/UIComponents.Tests/Addressables/Assets/Component.uxml"
            );
        }

        [UnityTest]
        public IEnumerator Should_Be_Able_To_Tell_If_Asset_Exists()
        {
            yield return Assert_Tells_If_Asset_Exists(
                "Assets/UIComponents.Tests/Addressables/Assets/Component.uss"
            );
            yield return Assert_Tells_If_Asset_Exists(
                "Assets/UIComponents.Tests/Addressables/Assets/Component.uxml"
            );
        }
    }
}
